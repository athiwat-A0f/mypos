using System.Data.SqlClient;

public class DbService
{
    private string connectionString =
        "Server=.\\SQLEXPRESS;Database=POSDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
}