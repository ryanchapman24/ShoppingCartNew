using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartNew.Models.Code_First
{
    public class Order
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string Zipcode { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime EstimatedDelivery { get; set; }
        public string CustomerId { get; set; }
        public int CreditCardId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Taxes { get; set; }
        public decimal FinalTotal { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        public virtual State State { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public int? DeliveryStage
        {
            get
            {
                if ((EstimatedDelivery - OrderDate).Days == 4)
                {
                    return 1;
                }
                else if ((EstimatedDelivery - OrderDate).Days == 3)
                {
                    return 2;
                }
                else if ((EstimatedDelivery - OrderDate).Days == 2)
                {
                    return 3;
                }
                else if ((EstimatedDelivery - OrderDate).Days == 1)
                {
                    return 4;
                }
                else if ((EstimatedDelivery - OrderDate).Days == 0)
                {
                    return 5;
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}