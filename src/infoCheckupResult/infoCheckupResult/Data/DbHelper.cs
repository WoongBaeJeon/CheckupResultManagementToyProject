using System;
using System.Data;
using System.Data.SqlClient;

namespace infoCheckupResult.Data
{
    public static class DbHelper
    {
        public static DataSet ExecuteDataSet(
            string storedProcedureName,
            params SqlParameter[] parameters)
        {
            ValidateStoredProcedureName(storedProcedureName);

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = CreateCommand(connection, storedProcedureName, parameters))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet;
            }
        }

        public static DataTable ExecuteDataTable(
            string storedProcedureName,
            params SqlParameter[] parameters)
        {
            ValidateStoredProcedureName(storedProcedureName);

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = CreateCommand(connection, storedProcedureName, parameters))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        private static SqlCommand CreateCommand(
            SqlConnection connection,
            string storedProcedureName,
            SqlParameter[] parameters)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 30;

            if (parameters != null && parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            return command;
        }

        private static void ValidateStoredProcedureName(string storedProcedureName)
        {
            if (string.IsNullOrWhiteSpace(storedProcedureName))
            {
                throw new ArgumentException(
                    "Stored Procedure 이름은 필수입니다.",
                    "storedProcedureName");
            }
        }
    }
}
