using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace SimpleASPNetMVC.KeyAuth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorizationAttribute : TypeFilterAttribute
    {
        public ApiKeyAuthorizationAttribute():base(typeof(ApiKeyAuthorizationFilter)) 
        {

        }

    }
}
