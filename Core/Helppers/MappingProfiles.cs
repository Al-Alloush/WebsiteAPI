using AutoMapper;
using Core.Dtos.Identity;
using Core.Models.Identity;
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

        }
    }
}
