namespace ImitationOnlineOrdering.Infrastructure
{
    public class FakeIdentity : IIdentity
    {
        public Guid GetUserID()
        {
            return new Guid("c74b6980-0e05-4d67-9731-996a4d5f0e1c");
        }
    }
}
