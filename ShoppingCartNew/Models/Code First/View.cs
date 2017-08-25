using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartNew.Models.Code_First
{
    public class View
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int ItemId { get; set; }

        public virtual Item item { get; set; }
    }
}