using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace QualityBags.Models
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int Quantity { get; set; }
        public decimal OrderItemPrice { get; set; }

        public Bag Bag { get; set; }
        public Order Order { get; set; }
    }
}
