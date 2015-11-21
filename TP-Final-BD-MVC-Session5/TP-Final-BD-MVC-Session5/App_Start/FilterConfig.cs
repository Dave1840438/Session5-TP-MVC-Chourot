using System.Web;
using System.Web.Mvc;

namespace TP_Final_BD_MVC_Session5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
