using System.Web;
using System.Web.Mvc;

namespace MVCHomework_20170703
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
