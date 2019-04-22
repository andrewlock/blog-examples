namespace StronglyTypedId.Shop.Data
{
    public class OrderLine
    {
        public OrderId OrderId { get; set; }
        public OrderLineId OrderLineId { get; set; }
        public string ProductName { get; set; }
    }
}