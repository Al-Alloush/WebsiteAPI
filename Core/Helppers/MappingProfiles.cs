using AutoMapper;
using Core.Dtos.Blogs;
using Core.Dtos.Identity;
using Core.Models.Blogs;
using Core.Models.Identity;
using Core.Models.Uploads;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helppers
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            // Identity
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppUser, LoginSuccessDto>();
            CreateMap<AppUser, UserDto>();
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();
            CreateMap<Address, AddressDto>();

            // Blogs
            CreateMap<Blog, BlogCardDto>() .ForMember(b => b.Language, m => m.MapFrom(l => l.Language.Name));
            CreateMap<Blog, BlogDto>() .ForMember(b => b.Language, m => m.MapFrom(l=>l.Language.Name)) ;
            CreateMap<BlogCardDto, Blog>();
            CreateMap<BlogDto, Blog>();
            CreateMap<BlogCreateDto, Blog>();
            CreateMap<Blog, BlogCreateDto>();
            CreateMap<BlogCategoryList, BlogCategoryListDto>();
            CreateMap<BlogCategoryListDto, BlogCategoryList>();
            CreateMap<BlogComment, BlogCommentDto>().ForMember(b => b.UserName, m => m.MapFrom(l => l.User.UserName));
            CreateMap<BlogCommentDto, BlogComment>();
            CreateMap<BlogCategory, BlogCategoryDto>();
            CreateMap<BlogCategoryDto, BlogCategory>();
            CreateMap<BlogUpdateDto, Blog>();
            CreateMap<Blog, BlogUpdateDto>();
            
            //
            CreateMap<UploadBlogImagesList, BlogImageDto>();

        }
    }
}
