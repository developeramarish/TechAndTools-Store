﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TechAndTools.Services.Mapping;
using TechAndTools.Services.Models;
using TechAndTools.Web.InputModels.Commons;

namespace TechAndTools.Web.InputModels.Products
{
    public class ProductEditInputModel : IMapFrom<ProductServiceModel>, IMapTo<ProductServiceModel>
    {
        [Display(Name = "Име")]
        [Required(ErrorMessage = InputModelsConstants.RequiredMessage)]
        [StringLength(25, ErrorMessage = @"""{0}"" може да бъде между {2} и {1} символа.", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = InputModelsConstants.RequiredMessage)]
        public int ProductCategoryId { get; set; }

        [Display(Name = "Марка")]
        [Required(ErrorMessage = InputModelsConstants.RequiredMessage)]
        public int BrandId { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = InputModelsConstants.RequiredMessage)]
        [StringLength(255, ErrorMessage = @"""{0}"" може да бъде между {2} и {1} символа.", MinimumLength = 10)]
        public string Description { get; set; }

        [Display(Name = "Линк към документация")]
        public string DocumentationUrl { get; set; }

        [Display(Name = "Месеци гаранция")]
        [Required(ErrorMessage = InputModelsConstants.RequiredMessage)]
        [Range(1, 120, ErrorMessage = @"""{0}"" може да бъде цяло число между {1} и {2}.")]
        public int Warranty { get; set; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = InputModelsConstants.RequiredMessage)]
        [Range(typeof(decimal), "0.1", "25000", ErrorMessage = "{0} може да бъде число между {1} и {2}.")]
        public decimal Price { get; set; }

        [Display(Name = "Бройки")]
        [Required(ErrorMessage = InputModelsConstants.RequiredMessage)]
        [Range(1, 10000, ErrorMessage = @"""{0}"" може да бъде цяло число между {1} и {2}.")]
        public int QuantityInStock { get; set; }

        [Display(Name = "Снимка")]
        public IFormFile ImageFormFile { get; set; }
    }
}
