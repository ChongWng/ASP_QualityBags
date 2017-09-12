using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBags.Models
{
    public class CartItem
    {
        public int CartItemID { get; set; }
        public string CartID { get; set; }
        public int ItemCount { get; set; }

        public Bag Bag { get; set; }
    }
}
