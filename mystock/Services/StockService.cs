using mystock.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class StockService
{
    DbService db = new DbService();

    public List<Stock> GetStocks()
    {
        var list = new List<Stock>();

        using (var conn = db.GetConnection())
        {
            conn.Open();

            string sql = "SELECT s.id, s.qty, s.cost, s.type, s.remark, p.id AS product_id, p.product_name, p.stock, p.price " +
                "FROM products p " +
                "LEFT JOIN stock s " +
                "ON s.id = p.id " +
                "WHERE p.is_active = 1 ";

            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Stock
                {
                    Id = reader["id"] == DBNull.Value ? 0 : (int)reader["id"],
                    ProductId = (int)reader["product_id"],
                    ProductName = reader["product_name"].ToString(),
                    Amount = (int)reader["stock"],
                    Price = (decimal)reader["price"],
                    Qty = reader["qty"] == DBNull.Value ? 0 : (int)reader["qty"],
                    Cost = reader["cost"] == DBNull.Value ? 0 : (decimal)reader["cost"],
                    Type = reader["type"] == DBNull.Value ? "" : reader["type"].ToString(),
                    Remark = reader["remark"] == DBNull.Value ? "" : reader["remark"].ToString(),
                });
            }
        }

        return list;
    }
}