using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mystock.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsOutOfStock
        {
            get { return Stock <= 0; }
        }
        public bool IsLowStock
        {
            get { return Stock > 0 && Stock <= 5; }
        }
        public bool CanSell
        {
            get { return Stock > 0; }
        }
    }
}
