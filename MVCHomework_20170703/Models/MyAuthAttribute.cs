using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomework_20170703.Models
{
    public class MyAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        { 
            if (!HttpContext.Current.User.Identity.IsAuthenticated) filterContext.Result = new HttpUnauthorizedResult(); 
        }
    }
}