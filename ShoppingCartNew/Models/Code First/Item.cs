using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartNew.Models.Code_First
{
    public class Item
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string MediaURL { get; set; }
        public string Description { get; set; }
        public bool OnSale { get; set; }
        public decimal? SalePrice { get; set; }
    }
}