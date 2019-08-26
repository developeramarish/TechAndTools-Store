﻿using AutoMapper;
using System;
using System.Linq;
using TechAndTools.Services.Mapping;
using TechAndTools.Services.Models;

namespace TechAndTools.Web.ViewModels.Articles
{
    public class AllArticleViewModel : IMapFrom<ArticleServiceModel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public string ImgUrl { get; set; }

        public int TimesRead { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Author { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ArticleServiceModel, AllArticleViewModel>()
                .ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.Image.ImageUrl))
                .ForMember(dest => dest.Author,
                    opt => opt.MapFrom(src => src.Author.FirstName + " " + src.Author.LastName));
        }
    }
}
