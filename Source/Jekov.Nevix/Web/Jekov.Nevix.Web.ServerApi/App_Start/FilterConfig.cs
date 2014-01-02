using System.Web;
using System.Web.Mvc;

namespace Jekov.Nevix.Web.ServerApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
