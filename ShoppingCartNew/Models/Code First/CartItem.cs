using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartNew.Models.Code_First
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public string CustomerId { get; set; }
        public DateTime Created { get; set; }

        public virtual Item Item { get; set; }
        public virtual ApplicationUser Customer { get; set; }

        public decimal UnitTotal
        {
            get
            {
                if (Item.OnSale == true)
                {
                    return Count * Item.SalePrice.Value;
                }
                else
                {
                    return Count * Item.Price;
                }
            }
        }
    }
}