using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI_GAMS_US.DATA.Sql
{
    public class Query
    {
        public static string GetReport(string s)
        {
            int.TryParse(s, out var sId);
            return $"EXEC dbo.usp_PivotData @SurveyId = {sId}";
        }

        public static string GetReport(string f, string t, string s)
        {
            f = string.IsNullOrEmpty(f) ? "NULL" : $"'{f}'";
            t = string.IsNullOrEmpty(t) ? "NULL" : $"'{t}'";
            int.TryParse(s, out var siteId);
            return $"EXEC [dbo].[spGenerateRawReport] @from = {f}, @to = {t}, @site = {siteId}";
        }
    }
}
