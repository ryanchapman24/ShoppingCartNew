using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartNew.Models.Code_First
{
    public class CreditCard
    {
        public int Id { get; set; }
        public int CardTypeId { get; set; }
        public string CardNumber { get; set; }
        public int CVC { get; set; }
        public int MonthId { get; set; }
        public int YearId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string Zipcode { get; set; }
        public string CustomerId { get; set; }

        public virtual CardType CardType { get; set; }
        public virtual Month Month { get; set; }
        public virtual Year Year { get; set; }
        public virtual State State { get; set; }
        public virtual ApplicationUser Customer { get; set; }

        public bool? Expired
        {
            get
            {
                if ((Convert.ToInt32(Year.YearName) < System.DateTime.Now.Year) || (Convert.ToInt32(Year.YearName) == System.DateTime.Now.Year && Convert.ToInt32(Month.MonthName) < System.DateTime.Now.Month))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}