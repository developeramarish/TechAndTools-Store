﻿using System;
using System.Collections.Generic;
using TechAndTools.Data.Models;
using TechAndTools.Services.Mapping;

namespace TechAndTools.Services.Models
{
    public class ProductServiceModel : IMapFrom<Product>, IMapTo<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProductCategoryId { get; set; }
        public virtual CategoryServiceModel ProductCategory { get; set; }

        public int BrandId { get; set; }
        public virtual BrandServiceModel Brand { get; set; }

        public virtual ICollection<ImageServiceModel> Images { get; set; }

        public string Description { get; set; }

        public string ManualUrl { get; set; }

        public int Warranty { get; set; }

        public decimal Price { get; set; }

        public int QuantityInStock { get; set; }

        public int SalesCount { get; set; }

        public bool IsOutOfStock { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<ReviewServiceModel> Reviews { get; set; }
    }
}