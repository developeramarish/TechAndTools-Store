﻿using System.Linq;
using TechAndTools.Data;
using TechAndTools.Data.Models;
using TechAndTools.Services.Mapping;
using TechAndTools.Services.Models;

namespace TechAndTools.Services
{
    public class ShoppingCartsService : IShoppingCartsService
    {
        private readonly TechAndToolsDbContext context;
        private readonly IProductService productService;
        private readonly IUserService userService;

        public ShoppingCartsService(TechAndToolsDbContext context,
            IProductService productService,
            IUserService userService)
        {
            this.context = context;
            this.productService = productService;
            this.userService = userService;
        }

        public void AddToShoppingCart(int productId, string username, int quantity)
        {
            var product = this.productService.GetProductById(productId);
            var user = this.userService.GetUserByUsername(username);

            if (product == null || user == null)
            {
                return;
            }

            var shoppingCartProduct = this.GetShoppingCartProduct(productId, user.ShoppingCartId);
         
            if (shoppingCartProduct != null)
            {
                return;
            }

            shoppingCartProduct = new ShoppingCartProduct
            {
                ProductId = productId,
                Quantity = quantity,
                ShoppingCartId = user.ShoppingCartId
            };

            this.context.ShoppingCartProducts.Add(shoppingCartProduct);
            this.context.SaveChanges();

        }
        
        private ShoppingCartProduct GetShoppingCartProduct(int productId, int shoppingCartId)
        {
            return this.context.ShoppingCartProducts.FirstOrDefault(x => x.ShoppingCartId == shoppingCartId && x.ProductId == productId);
        }

        public IQueryable<ShoppingCartProductServiceModel> GetAllShoppingCartProducts(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            return this.context.ShoppingCartProducts.Where(x => x.ShoppingCartId == user.ShoppingCartId).To<ShoppingCartProductServiceModel>();
        }

        public void DeleteProductFromShoppingCart(int id, string username)
        {
            var product = this.productService.GetProductById(id);
            var user = this.userService.GetUserByUsername(username);

            if (product == null || user == null)
            {
                return;
            }

            var shoppingCart = GetShoppingCartProduct(product.Id, user.ShoppingCartId);

            this.context.ShoppingCartProducts.Remove(shoppingCart);
            this.context.SaveChanges();
        }
    }
}