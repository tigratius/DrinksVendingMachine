using System;
using System.Web;
using System.Web.Mvc;
using DrinksVendingMachine.Models.Classes;

namespace DrinksVendingMachine.Models.Attributes
{
    public class AuthorizeByToken : AuthorizeAttribute
    {
        private const string ParamTokenName = "token";

        public override void OnAuthorization(AuthorizationContext actionContext)
        {

            if (Authorize())
            {
                return;
            }
            HandleUnauthorizedRequest(actionContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }

        private bool Authorize()
        {
            try
            {
                var context = new HttpContextWrapper(HttpContext.Current);
                HttpRequestBase request = context.Request;
                string token = request.Params[ParamTokenName];

                return ValidatingTokens.IsTokenValid(token); 
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}