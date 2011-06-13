using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Blog.Web.ViewModels;
using Blog.Model;

namespace Blog.Web.App.AutoMapper.Profiles
{
    public class BlogConfigMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BlogConfig, BlogConfigViewModel>();
            Mapper.CreateMap<BlogConfigViewModel, BlogConfig>();
        }
    }
}