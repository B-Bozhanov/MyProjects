namespace RealEstate.Web.Infrastructure.DatabaseModels
{
    public interface IDatabaseModel
    {
        public string ConnectionString { get; init; }

        public string UserId { get; init; }

        public string Password { get; init; }

        public int ConnectionPort { get; set; }
    }
}
