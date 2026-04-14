using Microsoft.AspNetCore.Mvc;

namespace ImitationOnlineOrdering.Controllers
{
    public class OnlineOrderingController : Controller
    {

        private IHttpContextAccessor _httpContextAccessor;

        public OnlineOrderingController (IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected Guid GetUserID()
        {

            if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
            {
                throw new Exception("HttpContext was unexpectedly null");
            }

            string oidValue;

            try
            { 
                oidValue = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "oid").Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get user claim oid: " + ex.Message);
            }

            Guid userID;

            try
            {
                userID = Guid.Parse(oidValue);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not parse user claim oid into GUID: " + ex.Message);
            }

            return userID;

        }

    }
}
