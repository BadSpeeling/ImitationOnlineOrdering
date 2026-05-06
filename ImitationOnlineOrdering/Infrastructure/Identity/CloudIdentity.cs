namespace ImitationOnlineOrdering.Infrastructure
{
    public class CloudIdentity : IIdentity
    {

        private IHttpContextAccessor _httpContextAccessor;

        public CloudIdentity(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserID()
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
