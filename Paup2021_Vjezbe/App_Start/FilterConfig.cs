using System.Web;
using System.Web.Mvc;

namespace Paup2021_Vjezbe
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
