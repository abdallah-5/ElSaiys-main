using AutoMapper;
using ElSaiys.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegisterDTO, User>()
                .ForMember(e => e.Id, op => op.MapFrom(e => Guid.NewGuid()))
                .ForMember(e => e.Slug, op => op.MapFrom(e => Guid.NewGuid()));

            CreateMap<User, UserResult>();
        }
    }
}
