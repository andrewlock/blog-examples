using StronglyTypedId.Shop.Data;
using StronglyTypedIdEfCore.Data;
using System.Linq;

namespace StronglyTypedId.Shop.Orders
{
    public class OrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }

        public Order GetOrder(OrderId orderId)
        {
            //return _dbContext.Orders.Find(orderId);
            return _dbContext.Orders
                .Where(order => order.OrderId == orderId)
                .FirstOrDefault();
        }
    }
}