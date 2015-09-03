using System.Data;
using System.Data.SqlClient;
using System.Text;
using Xnlab.SQLMon.Logic;

namespace Xnlab.SQLMon.Common
{
    class SqlHelper
    {
        internal static SqlConnection CreateNewConnection(ServerInfo info)
        {
            var builder = new SqlConnectionStringBuilder
            {
                ApplicationName = Settings.Title,
                IntegratedSecurity = info.AuthType == AuthTypes.Windows,
                DataSource = info.Server,
                UserID = info.User,
                Password = info.Password,
                InitialCatalog = info.Database ?? string.Empty,
                ConnectTimeout = Settings.Instance.ConnectionTimeout
            };
            return new SqlConnection(builder.ConnectionString);
        }

        internal static DataSet QuerySet(string sql, ServerInfo info)
        {
            string message;
            return QuerySet(sql, info, out message);
        }

        internal static DataSet QuerySet(string sql, ServerInfo info, out string message)
        {
            using (var connection = CreateNewConnection(info))
            {
                var result = new StringBuilder();
                connection.InfoMessage += (s, e) => { result.AppendLine(e.Message); };
                var command = new SqlCommand(sql, connection);
                command.StatementCompleted += (s, e) => { result.AppendLine(string.Format("{0} row(s) affected.", e.RecordCount)); };
                var adapter = new SqlDataAdapter(command);
                var data = new DataSet();
                //adapter.FillSchema(data, SchemaType.Mapped);
                adapter.Fill(data);
                connection.Close();
                message = result.ToString();
                return data;
            }
        }

        internal static DataTable Query(string sql, ServerInfo info)
        {
            var data = QuerySet(sql, info);
            if (data != null && data.Tables.Count > 0)
                return data.Tables[0];
            return null;
        }

        internal static string ExecuteNonQuery(string sql, ServerInfo server)
        {
            using (var connection = CreateNewConnection(server))
            {
                var result = new StringBuilder();
                connection.InfoMessage += (s, e) => { result.AppendLine(e.Message); };
                var command = new SqlCommand(sql, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return result.ToString();
            }
        }

        internal static object ExecuteScalar(string sql, ServerInfo server)
        {
            object result;
            using (var connection = CreateNewConnection(server))
            {
                var command = new SqlCommand(sql, connection);
                connection.Open();
                result = command.ExecuteScalar();
                connection.Close();
            }
            return result;
        }

    }
}
