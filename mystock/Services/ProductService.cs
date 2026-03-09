using mystock.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

public class ProductService
{
    DbService db = new DbService();

    public List<Product> GetProducts()
    {
        var list = new List<Product>();

        using (var conn = db.GetConnection())
        {
            conn.Open();

            string sql = "SELECT id, product_code, product_name, price, stock FROM products";

            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Product
                {
                    Id = (int)reader["id"],
                    ProductCode = reader["product_code"].ToString(),
                    ProductName = reader["product_name"].ToString(),
                    Price = (decimal)reader["price"],
                    Stock = (int)reader["stock"]
                });
            }
        }

        return list;
    }
}