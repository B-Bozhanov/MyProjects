namespace RealEstate.Web.Infrastructure.DatabaseModels
{
    using System;

    public class SqlDatabaseModel : IDatabaseModel
    {
        public SqlDatabaseModel()
        {
        }

        public SqlDatabaseModel(string connectionString, string userId, string password, int connectionPort) : this()
        {
            this.ConnectionString = connectionString;
            this.UserId = userId;
            this.Password = password;
            this.ConnectionPort = connectionPort;

            this.DataValidator();
        }

        public string ConnectionString { get; init; }

        public string UserId { get; init; }

        public string Password { get; init; }

        public int ConnectionPort { get; set; }

        private void DataValidator()
        {
            if (this.ConnectionString == null || this.UserId == null || this.Password == null || this.ConnectionPort <= 0)
            {
                throw new ArgumentNullException("The database data can not be null!");
            }
        }
    }
}
