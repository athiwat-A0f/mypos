using mystock.Helper;
using mystock.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mystock.Pages
{

    public partial class ProductPage : Page
    {
        DbService db = new DbService();

        Product selectedProduct;

        List<Category> categories = new List<Category>();
        ProductService productService = new ProductService();

        public ProductPage()
        {
            InitializeComponent();
            LoadCategories();

            LoadProducts();

        }

        void LoadProducts()
        {
            ProductGrid.ItemsSource = productService.GetProducts();
        }

        private void ClearForm()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtPrice.Text = "";
            cmbCategories.SelectedIndex = 0;
            cmbStatus.SelectedValue = 1;
        }

        private void LoadCategories()
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT id, category_name FROM categories", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["category_name"].ToString()
                    });
                }
            }

            cmbCategories.ItemsSource = categories;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                txtCode.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                txtPrice.Focus();
                return;
            }

            using (var conn = db.GetConnection())
            {
                conn.Open();

                SqlTransaction tran = conn.BeginTransaction();

                try
                {

                    string sql = @"INSERT INTO products
                                (
                                    product_code,
                                    product_name,
                                    price,
                                    is_active,
                                    category_id
                                )
                                VALUES
                                (
                                    @code,
                                    @name,
                                    @price,
                                    @is_active,
                                    @category_id
                                )";

                    SqlCommand cmd = new SqlCommand(sql, conn, tran);

                    cmd.Parameters.AddWithValue("@code", txtCode.Text);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@category_id", cmbCategories.SelectedValue);
                    cmd.Parameters.AddWithValue("@is_active", Convert.ToInt32(cmbStatus.SelectedValue));

                    cmd.ExecuteNonQuery();

                    tran.Commit();

                    Alert.Success("บันทึกสำเร็จ");

                    LoadProducts();
                    ClearForm();
                }
                catch
                {
                    tran.Rollback();

                    Alert.Error("เกิดข้อผิดพลาด!");
                }
            }
        }

        private void ProductGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProduct = ProductGrid.SelectedItem as Product;

            if (selectedProduct == null) return;

            txtCode.Text = selectedProduct.ProductCode;
            txtName.Text = selectedProduct.ProductName;
            txtPrice.Text = selectedProduct.Price.ToString();

            cmbCategories.SelectedValue = selectedProduct.CategoryId;
            cmbStatus.SelectedValue = selectedProduct.IsActive ? 1 : 0;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null) return;

            using (var conn = db.GetConnection())
            {
                conn.Open();

                SqlTransaction tran = conn.BeginTransaction();

                try
                {

                    string sql = @"UPDATE products 
                       SET product_code = @code,
                           product_name = @name,
                           price = @price,
                           is_active = @is_active,
                           category_id = @category_id
                       WHERE id = @id";

                    SqlCommand cmd = new SqlCommand(sql, conn, tran);

                    cmd.Parameters.AddWithValue("@id", selectedProduct.Id);
                    cmd.Parameters.AddWithValue("@code", txtCode.Text);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@category_id", cmbCategories.SelectedValue);
                    cmd.Parameters.AddWithValue("@is_active", Convert.ToInt32(cmbStatus.SelectedValue));

                    cmd.ExecuteNonQuery();

                    tran.Commit();

                    Alert.Success("บันทึกสำเร็จ");

                    LoadProducts();
                    ClearForm();
                }
                catch
                {
                    tran.Rollback();

                    Alert.Error("เกิดข้อผิดพลาด!");
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }
    }
}
