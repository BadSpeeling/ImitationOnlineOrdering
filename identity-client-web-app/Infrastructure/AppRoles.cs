namespace identity_client_web_app.Infrastructure
{
    public class AppRoles
    {

        public static class AppRole
        {
            public const string RestaurantOwnerAll = "RestaurantOwner.All";
        }

        public static class AuthorizationPolicies
        {
            public const string AssignmentToRestaurantOwnerRequired = "AssignmentToRestaurantOwnerRequired";
        }

    }
}
