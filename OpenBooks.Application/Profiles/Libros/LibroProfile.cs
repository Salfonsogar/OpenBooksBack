using AutoMapper;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Profiles.Libros
{
    public class LibroProfile : Profile
    {
        public LibroProfile()
        {
            CreateMap<LibroCreateDto, Libro>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ValoracionPromedio, opt => opt.Ignore())
                .ForMember(dest => dest.Resenas, opt => opt.Ignore())
                .ForMember(dest => dest.BibliotecaLibros, opt => opt.Ignore())
                .ForMember(dest => dest.EstanteriaLibros, opt => opt.Ignore())
                .ForMember(dest => dest.LibroCategorias, opt => opt.MapFrom(src =>
                    src.CategoriasIds.Select(id => new LibroCategoria
                    {
                        CategoriaId = id
                    }).ToList()
                ));


            CreateMap<LibroUpdateDto, Libro>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ValoracionPromedio, opt => opt.Ignore())
                .ForMember(dest => dest.Resenas, opt => opt.Ignore())
                .ForMember(dest => dest.BibliotecaLibros, opt => opt.Ignore())
                .ForMember(dest => dest.EstanteriaLibros, opt => opt.Ignore())
                .ForMember(dest => dest.LibroCategorias, opt => opt.MapFrom(src =>
                    src.CategoriasIds.Select(id => new LibroCategoria
                    {
                        CategoriaId = id
                    }).ToList()
                ));

            CreateMap<LibroPatchDto, Libro>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null)
                );

            CreateMap<Libro, LibroCardDto>();

            CreateMap<LibroCategoria, LibroCategoriaDto>()
                .ForMember(dest => dest.CategoriaId, opt => opt.MapFrom(src => src.CategoriaId))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Categoria.Nombre));

            CreateMap<Libro, LibroDetailDto>()
                .ForMember(dest => dest.Categorias, opt => opt.MapFrom(src => src.LibroCategorias));

        }
    }
}
