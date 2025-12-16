using AutoMapper;
using OpenBooks.Application.DTOs.Auth;
using OpenBooks.Application.DTOs.Usuarios;
using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Profiles.Usuarios
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<UsuarioRegisterDto, Usuario>();

            CreateMap<Usuario, LoginResponseDto>().ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol != null ? src.Rol.Nombre : string.Empty))
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.NombreUsuario))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Token, opt => opt.Ignore());

            CreateMap<Usuario, UsuarioResponseDto>()
                .ForMember(dest => dest.Rol,
                    opt => opt.MapFrom(src => src.Rol.Nombre));

            CreateMap<UsuarioCreateDto, Usuario>().ForMember(dest => dest.Contrasena, opt => opt.Ignore());

            CreateMap<UsuarioUpdateDto, Usuario>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null)
            );

            CreateMap<UsuarioUpdatePerfilDto, Usuario>()
                .ForMember(d => d.Correo, opt => opt.Ignore())
                .ForMember(d => d.Contrasena, opt => opt.Ignore())
                .ForMember(d => d.RolId, opt => opt.Ignore())
                .ForMember(d => d.Rol, opt => opt.Ignore())
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null)
                );
        }
    }
}
