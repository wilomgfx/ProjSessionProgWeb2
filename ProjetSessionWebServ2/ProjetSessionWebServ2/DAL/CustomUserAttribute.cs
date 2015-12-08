using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace ProjetSessionWebServ2.DAL
{
    public class CustomUserAttribute : AuthorizeAttribute
    {
        //Permet de passer des paramètres
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            // ...
            // Vérification si l'utilisateur a les droit voulus
            // ...

            return true;
        }

        //Permet de rediriger vers une route précise
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var result = new ViewResult();
            result.ViewName = "BadRoleError";        //this can be a property you don't have to hard code it
            result.MasterName = "BadRoleError";
            filterContext.Result = result;
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = "Home",
                        action = "BadRoleError"
                    })
                );
        }
    }
}