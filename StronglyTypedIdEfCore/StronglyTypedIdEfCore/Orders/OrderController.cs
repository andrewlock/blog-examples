using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StronglyTypedId.Shop.Data;

namespace StronglyTypedId.Shop.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;

        public OrderController(OrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<Order> Post(Order order)
        {
            _service.AddOrder(order);

            return Ok(order);
        }

        [HttpGet("{orderId}")]
        public ActionResult<Order> Get(OrderId orderId)
        {
            var order = _service.GetOrder(orderId);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}