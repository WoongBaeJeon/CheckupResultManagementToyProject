using System.Configuration;
using System.Data.SqlClient;

namespace infoCheckupResult.Data
{
    public static class DbConnectionFactory
    {
        private const string ConnectionStringName = "CheckupResultDb";

        public static SqlConnection CreateConnection()
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[ConnectionStringName];

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    "App.config에 CheckupResultDb 연결 문자열이 없습니다.");
            }

            return new SqlConnection(settings.ConnectionString);
        }
    }
}
