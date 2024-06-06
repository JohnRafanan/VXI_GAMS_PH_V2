using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VXI_GAMS_US.VIEWS.Entities;
using VXI_GAMS_US.VIEWS.View;

namespace VXI_GAMS_US.HELPER
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public static class GlobalApi
    {
        public static async Task<ApiViewModel<Bulk>> GetBulkEmployeeInfoFromApi(string completeUrl)
        {
            var api = await HttpHelper<ApiViewModel<Bulk>>.GetApiViewModelAccount(completeUrl);
            var pangalawa = false;
            GotoRetry:
            if (string.IsNullOrEmpty(api?.Table?.FirstOrDefault()?.ID))
            {
                if (pangalawa)
                    throw new Exception("No data found on api");
                api = await HttpHelper<ApiViewModel<Bulk>>.GetApiViewModelAccount(completeUrl, true);
                pangalawa = true;
                goto GotoRetry;
            }

            return api;
        }
        public static async Task<ApiViewModel<GlobalSites>> GetLocationsFromApi(string completeUrl)
        {
            var api = await HttpHelper<ApiViewModel<GlobalSites>>.GetApiViewModelAccount(completeUrl);
            var pangalawa = false;
            GotoRetry:
            if (string.IsNullOrEmpty(api?.Table?.FirstOrDefault()?.LocationDesc))
            {
                if (pangalawa)
                    throw new Exception("No data found on api");
                api = await HttpHelper<ApiViewModel<GlobalSites>>.GetApiViewModelAccount(completeUrl, true);
                pangalawa = true;
                goto GotoRetry;
            }

            return api;
        }
        public static async Task<ApiViewModel<User>> GetEmployeeInfoFromApi(string completeUrl)
        {
            var api = await HttpHelper<ApiViewModel<User>>.GetApiViewModelAccount(completeUrl);
            var pangalawa = false;
            GotoRetry:
            if (api?.Table?.FirstOrDefault()?.ID == null)
            {
                if (pangalawa)
                {
                    throw new Exception("No data found on api");
                }

                api = await HttpHelper<ApiViewModel<User>>.GetApiViewModelAccount(completeUrl, true);
                pangalawa = true;
                goto GotoRetry;
            }
            return api;
        }
        public static async Task<ApiViewModel<ITApiCount>> GetEmployeeCountFromApi(string completeUrl)
        {
            var api = await HttpHelper<ApiViewModel<ITApiCount>>.GetApiViewModelAccount(completeUrl);
            var pangalawa = false;
            GotoRetry:
            if (api?.Table?.FirstOrDefault()?.EmployeeCount == null)
            {
                if (pangalawa)
                {
                    throw new Exception("No data found on api");
                }

                api = await HttpHelper<ApiViewModel<ITApiCount>>.GetApiViewModelAccount(completeUrl, true);
                pangalawa = true;
                goto GotoRetry;
            }

            return api;
        }
        public static async Task<T> GetTest<T>(string completeUrl) where T : class
        {
            var url = $"{completeUrl}";
            url = await HttpHelper.GetAsync(url);
            var tt = JsonConvert.DeserializeObject<T>(url);
            return tt;
        }
    }
    public class AppDevApi
    {
        public AppDevApiProperty id { get; set; }
        public AppDevApiProperty firstName { get; set; }
        public AppDevApiProperty lastName { get; set; }
        public AppDevApiProperty middleName { get; set; }
        public AppDevApiProperty role { get; set; }
        public AppDevApiProperty position { get; set; }
        public AppDevApiProperty lob { get; set; }
        public AppDevApiProperty dob { get; set; }
        public AppDevApiProperty gender { get; set; }
        public AppDevApiProperty emailAddress { get; set; }
        public AppDevApiProperty hireDate { get; set; }
        public AppDevApiProperty address { get; set; }
        public AppDevApiProperty city { get; set; }
        public AppDevApiProperty contactNumber { get; set; }
        public AppDevApiProperty tin { get; set; }
        public AppDevApiProperty sss { get; set; }
        public AppDevApiProperty phic { get; set; }
        public AppDevApiProperty hdmf { get; set; }
        public AppDevApiProperty civilStatus { get; set; }
        public AppDevApiProperty exemption { get; set; }
    }
    public class AppDevApiProperty
    {
        public string value { get; set; }
        public string datatype { get; set; }
        public bool is_editable { get; set; }
    }
}
