namespace ImitationOnlineOrdering.Infrastructure
{
    public class AppRoles
    {

        public static class AppRole
        {
            public const string FranchiseOwner = "FranchiseOwner";
        }

        public static class AuthorizationPolicies
        {
            public const string AssignmentToFranchiseOwnerRequired = "AssignmentToFranchiseOwnerRequired";
        }

    }
}
