using Microsoft.AspNetCore.Mvc;

namespace identity_client_web_app.Controllers
{
    public class OnlineOrderingController : Controller
    {

        private IHttpContextAccessor _httpContextAccessor;

        public OnlineOrderingController (IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected string GetUserID()
        {

            if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
            {
                throw new Exception("HttpContext was unexpectedly null");
            }

            return _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "oid").Value;

        }

    }
}
