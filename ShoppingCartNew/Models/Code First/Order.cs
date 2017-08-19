﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartNew.Models.Code_First
{
    public class Order
    {
        public int Id { get; set; }
        public bool Completed { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Taxes { get; set; }
        public decimal FinalTotal { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }


    }
}