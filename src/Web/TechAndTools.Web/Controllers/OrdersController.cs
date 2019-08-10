﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TechAndTools.Data.Models;
using TechAndTools.Services.Contracts;
using TechAndTools.Services.Mapping;
using TechAndTools.Services.Models;
using TechAndTools.Web.InputModels.Administration.Orders;
using TechAndTools.Web.ViewModels.Addresses;
using TechAndTools.Web.ViewModels.PaymentMethods;
using TechAndTools.Web.ViewModels.ShoppingCart;
using TechAndTools.Web.ViewModels.Suppliers;

namespace TechAndTools.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IUserService userService;
        private readonly ISupplierService supplierService;
        private readonly IAddressService addressService;
        private readonly IPaymentMethodService paymentMethodService;

        public OrdersController(IShoppingCartService shoppingCartService,
            IUserService userService,
            ISupplierService supplierService,
            IAddressService addressService,
            IPaymentMethodService paymentMethodService)
        {
            this.shoppingCartService = shoppingCartService;
            this.userService = userService;
            this.supplierService = supplierService;
            this.addressService = addressService;
            this.paymentMethodService = paymentMethodService;
        }

        public IActionResult Create()
        {
            if (!this.shoppingCartService.AnyProducts(this.User.Identity.Name))
            {
                return RedirectToAction("Index", "Home");
            }

            List<AddressViewModel> addressesViewModel = this.addressService
                .GetAllByUserId(this.User.FindFirstValue(ClaimTypes.NameIdentifier))
                .To<AddressViewModel>()
                .ToList();

            TechAndToolsUser user = this.userService
                .GetUserByUsername(this.User.Identity.Name);

            List<SupplierViewModel> supplierViewModels = this.supplierService
                .GetAllSuppliers()
                .To<SupplierViewModel>()
                .ToList();

            List<ShoppingCartProductServiceModel> shoppingCartProductsServiceModels = this.shoppingCartService
                .GetAllShoppingCartProducts(this.User.Identity.Name)
                .ToList();

            List<PaymentMethodViewModel> paymentMethodViewModels = this.paymentMethodService
                .GetAllPaymentMethods()
                .To<PaymentMethodViewModel>().ToList();

            var shoppingCartProductsViewModels =
                shoppingCartProductsServiceModels.Select(x => x.To<ShoppingCartProductViewModel>());

            var createOrderViewModel = new OrderCreateInputModel
            {
                AddressesViewModels = addressesViewModel.ToList(),
                PhoneNumber = user.PhoneNumber,
                SuppliersViewModel = supplierViewModels,
                ShoppingCartProductViewModels = shoppingCartProductsViewModels,
                PaymentMethodViewModels = paymentMethodViewModels
            };
            ;
            return this.View(createOrderViewModel);
        }
    }
}