using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMU_EBurger.Models
{
    public class CartItem
    {
        public Menu Menu { get; set; }
        public int Quantity { get; set; }
    }
}