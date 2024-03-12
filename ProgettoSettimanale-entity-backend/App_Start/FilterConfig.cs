using System.Web;
using System.Web.Mvc;

namespace ProgettoSettimanale_entity_backend
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
