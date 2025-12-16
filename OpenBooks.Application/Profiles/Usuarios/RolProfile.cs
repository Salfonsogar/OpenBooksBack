using AutoMapper;
using OpenBooks.Application.DTOs.Usuarios;
using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Profiles.Usuarios
{
    public class RolProfile : Profile
    {
        public RolProfile()
        {
            CreateMap<Rol, RolResponseDto>();
            CreateMap<RolCreateDto, Rol>();
            CreateMap<RolUpdateDto, Rol>();
        }
    }
}
