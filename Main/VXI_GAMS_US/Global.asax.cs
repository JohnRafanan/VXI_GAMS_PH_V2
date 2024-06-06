using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using MoreLinq;
using VXI_GAMS_US.DATA.Data;
using VXI_GAMS_US.HELPER;
using VXI_GAMS_US.VIEWS.Entities;
using static System.Web.HttpContext;

namespace VXI_GAMS_US
{
    [SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
    [SuppressMessage("ReSharper", "BadControlBracesIndent")]
    public class MvcApplication : HttpApplication
    {
        public static BackgroundWorker Worker = new BackgroundWorker();
        public static bool IsWorkerCompleted;
        public static string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        public static string ErrorDir = "public/";
        public static string ErrorFile = "error.txt";
        public MvcApplication()
        {
            this.RegisterWindowsAuthentication();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            try
            {
                int.TryParse(ConfigurationManager.AppSettings["PRODUCTION"], out var isProduction);
                if (isProduction == 1)
                {
                    IsWorkerCompleted = true;
                    Task.WaitAll(Task.Run(async () => { await ChangeEmployeeUpdateStatus(false); }));
                    //fetch employee in background - when application starts
                    Worker.DoWork += DoWork;
                    Worker.WorkerReportsProgress = true;
                    Worker.WorkerSupportsCancellation = true;
                    Worker.RunWorkerCompleted += WorkerCompleted;
                    Worker.RunWorkerAsync();
                }
            }
            catch
            {
                //ignore
            }
        }

        protected void Application_End()
        {
            try
            {
                Worker?.CancelAsync();
            }
            catch
            {
                //ignore
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var cInfo = new CultureInfo("en-PH");
            Thread.CurrentThread.CurrentCulture = cInfo;
            Thread.CurrentThread.CurrentUICulture = cInfo;
        }

        protected void Application_PostAuthorizeRequest()
        {
            //check if request is a web api request
            if (Current.Request.AppRelativeCurrentExecutionFilePath != null && Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative))
                Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                IsWorkerCompleted = false;
                //sleep for 30min
                Thread.Sleep(1800000);
                Worker.RunWorkerAsync();
            }
            catch
            {
                //ignore
            }
        }

        /// <summary>
        /// Create a file that contains logs of errors on the tool
        /// </summary>
        /// <param name="error">The error to add on the logs</param>
        /// <returns></returns>
        public static async Task ErrorLogAsync(string error)
        {
            try
            {
                error = $"[Timestamp::{DateTime.Now:F}]========================================" +
                        Environment.NewLine +
                        error +
                        Environment.NewLine +
                        "=====================================================================";
                var aa = Path.Combine(BaseDir, ErrorDir);
                Directory.CreateDirectory(aa);
                aa = Path.Combine(BaseDir, ErrorDir, ErrorFile);
                using (var writer = new StreamWriter(aa, true))
                    await writer.WriteLineAsync(error);
            }
            catch
            {
                //ignore
            }
        }


        private static async Task ChangeEmployeeUpdateStatus(bool yesNo = true)
        {
            try
            {
                using (var a = new DataContext())
                {
                    var b = await a.EmployeeUpdateStatus.FirstOrDefaultAsync();
                    if (b != null)
                    {
                        b.IsActive = yesNo;
                        if (b.IsActive)
                            b.Timestamp = DateTime.Now;
                        a.Entry(b).State = EntityState.Modified;
                        await a.SaveChangesAsync();
                    }
                    if (yesNo)
                        await a.Database.ExecuteSqlCommandAsync("UPDATE dbo.Bulks SET IsUpdated = 0");
                }
            }
            catch (Exception ex)
            {
                var error = ExceptionHandler.GetMessages(ex) + Environment.NewLine + "in ChangeEmployeeUpdateStatus function";
                await ErrorLogAsync(error);
            }
        }


        private static void DoWork(object sender, DoWorkEventArgs e)
        {
            Task.WaitAll(Task.Run(async () =>
            {
                try
                {
                    var newList = new ConcurrentBag<Bulk>();
                    var oldList = new ConcurrentBag<Bulk>();
                    var dateTime = DateTime.Now;
                    using (var c = new DataContext())
                    {
                    recheck:
                        var update = await c.EmployeeUpdateStatus.FirstOrDefaultAsync();
                        if (update == null)
                        {
                            c.EmployeeUpdateStatus.Add(new EmployeeUpdateStatus { Timestamp = new DateTime(2020, 10, 1, 3, 55, 12) });
                            await c.SaveChangesAsync();
                            goto recheck;
                        }

                        if (update.IsActive)
                            throw new Exception("Cancel operation. Update is currently Ongoing");
                        var tmp = DateTime.Now - update.Timestamp;
                        if (tmp.TotalHours >= 0 && tmp.TotalHours <= 5.0 && update.IsActive == false)
                        {
                            await ChangeEmployeeUpdateStatus(false);
                            throw new Exception("do not update yet");
                        }

                        c.EmployeeUpdateHistory.Add(new EmployeeUpdateHistory { Timestamp = dateTime });
                        await c.SaveChangesAsync();
                    }
                    await ChangeEmployeeUpdateStatus();
                    var count = await GlobalApi
                        .GetEmployeeCountFromApi(ConfigurationManager.AppSettings["GLOBAL_API_BULK_COUNT"]);
                    var employees = await GlobalApi
                        .GetBulkEmployeeInfoFromApi(
                            string.Format(
                                ConfigurationManager.AppSettings["GLOBAL_API_BULK"], count.Table?.FirstOrDefault()?.EmployeeCount + 5 ?? 0
                            )
                        );
                    var list = employees.Table;
                    List<DbApi> dbApi;
                    using (var db = new DataContext())
                    {
                        dbApi = await db.BulkApiEmployee.Select(x => new DbApi
                        {
                            ID = x.ID,
                            UserId = x.UserId
                        }).ToListAsync();
                    }
                    if (list.Any())
                    {
                        var groupedList = list
                            .GroupBy(x => new { x.ID })
                            .Select(y => y.First())
                            .ToList();
                        IEnumerable<IEnumerable<Bulk>> groupBatches;
                        if (groupedList.Count > 1500)
                            groupBatches = groupedList.Batch(1000);
                        else if (groupedList.Count > 1000)
                            groupBatches = groupedList.Batch(500);
                        else if (groupedList.Count > 500)
                            groupBatches = groupedList.Batch(250);
                        else if (groupedList.Count > 250)
                            groupBatches = groupedList.Batch(125);
                        else if (groupedList.Count > 125)
                            groupBatches = groupedList.Batch(60);
                        else if (groupedList.Count > 60)
                            groupBatches = groupedList.Batch(30);
                        else if (groupedList.Count > 30)
                            groupBatches = groupedList.Batch(15);
                        else groupBatches = groupedList.Batch(5);
                        var groupTasks = groupBatches.Select(b => b.ToList())
                            .Select(bList => Task.Run(async () =>
                            {
                                if (dbApi.Any())
                                {
                                    foreach (var api in bList)
                                    {
                                        try
                                        {
                                            var employee = dbApi.FirstOrDefault(x => x.ID == api.ID);
                                            if (employee == null)
                                                newList.Add(api);
                                            else
                                            {
                                                api.UserId = employee.UserId;
                                                oldList.Add(api);
                                            }
                                            Thread.Sleep(1000);
                                        }
                                        catch (Exception exception)
                                        {
                                            var error = ExceptionHandler.GetMessages(exception);
                                            await ErrorLogAsync(error);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var api in bList)
                                        newList.Add(api);
                                }
                            }
                            )).ToList();
                        await Task.WhenAll(groupTasks);
                        if (newList.Any())
                        {
                            IEnumerable<IEnumerable<Bulk>> batches;
                            if (newList.Count > 1500)
                                batches = newList.Batch(1000);
                            else if (newList.Count > 1000)
                                batches = newList.Batch(500);
                            else if (newList.Count > 500)
                                batches = newList.Batch(250);
                            else if (newList.Count > 250)
                                batches = newList.Batch(125);
                            else if (newList.Count > 125)
                                batches = newList.Batch(60);
                            else if (newList.Count > 60)
                                batches = newList.Batch(30);
                            else if (newList.Count > 30)
                                batches = newList.Batch(15);
                            else batches = newList.Batch(5);
                            var tasks = batches.Select(b => b.ToList())
                                .Select(bList => Task.Run(async () =>
                                {
                                    try
                                    {
                                        using (var context = new DataContext())
                                        {
                                            context.BulkApiEmployee.AddRange(bList);
                                            await context.SaveChangesAsync();
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        var error = ExceptionHandler.GetMessages(exception);
                                        await ErrorLogAsync(error);
                                    }
                                }
                                )).ToList();
                            await Task.WhenAll(tasks);
                            Console.WriteLine(@"waited");
                        }

                        if (oldList.Any())
                        {
                            IEnumerable<IEnumerable<Bulk>> batches;
                            if (oldList.Count > 1500)
                                batches = oldList.Batch(1000);
                            else if (oldList.Count > 1000)
                                batches = oldList.Batch(500);
                            else if (oldList.Count > 500)
                                batches = oldList.Batch(250);
                            else if (oldList.Count > 250)
                                batches = oldList.Batch(125);
                            else if (oldList.Count > 125)
                                batches = oldList.Batch(60);
                            else if (oldList.Count > 60)
                                batches = oldList.Batch(30);
                            else if (oldList.Count > 30)
                                batches = oldList.Batch(15);
                            else batches = oldList.Batch(5);
                            var tasks = batches.Select(b => b.ToList())
                                .Select(bList => Task.Run(async () =>
                                {
                                    try
                                    {
                                        using (var context = new DataContext())
                                        {
                                            foreach (var entity in bList)
                                            {
                                                context.Set<Bulk>().Attach(entity);
                                                context.Entry(entity).State = EntityState.Modified;
                                            }
                                            await context.SaveChangesAsync();
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        var error = ExceptionHandler.GetMessages(exception);
                                        await ErrorLogAsync(error);
                                    }
                                }
                                )).ToList();
                            await Task.WhenAll(tasks);
                            Console.WriteLine(@"waited");
                        }
                    }
                    using (var db = new DataContext())
                    {
                        await db.Database.ExecuteSqlCommandAsync("UPDATE dbo.Bulks SET IsActive = IsUpdated");
                    }
                }
                catch (Exception ex)
                {
                    var error = ExceptionHandler.GetMessages(ex);
                    await ErrorLogAsync(error);
                }
                finally
                {
                    await ChangeEmployeeUpdateStatus(false);
                }
            }));
            Console.WriteLine(@"i have waited");
        }
        public class DbApi
        {
            public string ID { get; set; }
            public Guid UserId { get; set; }
        }
    }
}
