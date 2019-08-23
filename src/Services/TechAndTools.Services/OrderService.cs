﻿using System;
using System.Linq;
using TechAndTools.Data;
using TechAndTools.Data.Models;
using TechAndTools.Services.Contracts;
using TechAndTools.Services.Mapping;
using TechAndTools.Services.Models;
using TechAndTools.Web.ViewModels.Orders;

namespace TechAndTools.Services
{
    public class OrderService : IOrderService
    {

        private readonly IUserService userService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly ISupplierService supplierService;
        private readonly TechAndToolsDbContext context;

        public OrderService(IUserService userService,
            IShoppingCartService shoppingCartService,
            ISupplierService supplierService,
            TechAndToolsDbContext context)
        {
            this.userService = userService;
            this.shoppingCartService = shoppingCartService;
            this.supplierService = supplierService;
            this.context = context;
        }

        public OrderServiceModel Create(OrderServiceModel orderServiceModel, string username, decimal deliveryPrice)
        {
            var shoppingCartProducts = this.shoppingCartService
                .GetAllShoppingCartProducts(username)
                .ToList();

            if (shoppingCartProducts.Count == 0)
            {
                throw new ArgumentNullException("The cart is empty.");
            }

            var user = this.userService.GetUserByUsername(username);
            var supplier = this.supplierService.GetSupplierById(orderServiceModel.SupplierId);

            Order order = orderServiceModel.To<Order>();

            order.OrderProducts = shoppingCartProducts
                .Select(scp => new OrderProduct
                {
                    Order = order,
                    ProductId = scp.ProductId,
                    Price = scp.Product.Price,
                    Quantity = scp.Quantity
                })
                .ToList();

            this.shoppingCartService.DeleteAllProductFromShoppingCart(username);

            order.DeliveryPrice = deliveryPrice;
            order.OrderDate = DateTime.UtcNow;
            order.UserId = user.Id;
            order.ExpectedDeliveryDate = DateTime.UtcNow.AddDays(supplier.MaximumDeliveryTimeDays);
            order.Status = this.context.OrderStatuses.FirstOrDefault(x => x.Name == "Unprocessed");
            order.PaymentStatus = this.context.PaymentStatuses.FirstOrDefault(x => x.Name == "Unpaid");
            order.TotalPrice = order.OrderProducts.Sum(product => product.Price * product.Quantity);

            this.context.Orders.Add(order);
            this.context.SaveChanges();

            return order.To<OrderServiceModel>();
        }

        public OrderServiceModel GetOrderById(int orderId)
        {
            OrderServiceModel orderServiceModel = this.context.Orders
                .To<OrderServiceModel>()
                .SingleOrDefault(x => x.Id == orderId);

            if (orderServiceModel == null)
            {
                throw new ArgumentNullException("The Order not found!");
            }

            return orderServiceModel;
        }

        public IQueryable<OrderServiceModel> GetAllOrdersByUserId(string userId)
        {
            return this.context.Orders
                .Where(x => x.UserId == userId)
                .To<OrderServiceModel>();
        }

        public IQueryable<OrderServiceModel> GetUnprocessedOrders()
        {
            return this.context.Orders
                .Where(x => x.Status.Name == "Unprocessed")
                .To<OrderServiceModel>();
        }

        public IQueryable<OrderServiceModel> GetProcessedOrders()
        {
            return this.context.Orders
                .Where(x => x.Status.Name == "Processed")
                .To<OrderServiceModel>();
        }
    }
}
