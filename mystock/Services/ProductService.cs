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

            string sql = "SELECT p.id, p.product_code, p.product_name, p.price, p.stock, p.is_active, p.category_id, c.category_name " +
                "FROM products p " +
                "INNER JOIN categories c " +
                "ON p.category_id = c.id ";

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
                    Stock = (int)reader["stock"],
                    IsActive = (bool)reader["is_active"],
                    CategoryId = (int)reader["category_id"],
                    CategoryName = (string)reader["category_name"]
                });
            }
        }

        return list;
    }
}