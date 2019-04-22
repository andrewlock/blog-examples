using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StronglyTypedId.Shop.Data
{
    public class Order
    {
        public OrderId OrderId { get; set; }
        public string Name { get; set; }

        public ICollection<OrderLine> OrderLines { get; set; }
    }
}