using AutoMapper;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Profiles.Libros
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            CreateMap<Categoria, CategoriaResponseDto>();

            CreateMap<CategoriaCreateDto, Categoria>();

            CreateMap<CategoriaUpdateDto, Categoria>();

            CreateMap<CategoriaPatchDto, Categoria>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
