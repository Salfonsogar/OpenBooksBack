using AutoMapper;
using FluentValidation;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.Services.Libros.Interfaces;
using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Services.Libros.Implementations
{
    public class LibroService : ILibroService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<LibroCreateDto> _createValidator;
        private readonly IValidator<LibroUpdateDto> _updateValidator;
        private readonly IValidator<LibroPatchDto> _patchValidator;

        public LibroService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<LibroCreateDto> createValidator,
            IValidator<LibroUpdateDto> updateValidator,
            IValidator<LibroPatchDto> patchValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _patchValidator = patchValidator;
        }

        public async Task<Result<int>> CreateAsync(LibroCreateDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result<int>.Failure(validation.Errors.First().ErrorMessage);

            var libro = _mapper.Map<Libro>(dto);

            await _unitOfWork.Libros.AddAsync(libro);
            await _unitOfWork.CommitAsync();

            return Result<int>.Success(libro.Id);
        }
        public async Task<Result> UpdateAsync(int id, LibroUpdateDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result.Failure(validation.Errors.First().ErrorMessage);

            var libro = await _unitOfWork.Libros.GetByIdWithCategoriasAsync(id);
            if (libro == null)
                return Result.Failure("El libro no existe");

            libro.LibroCategorias.Clear();

            _mapper.Map(dto, libro);

            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
        public async Task<Result> PatchAsync(int id, LibroPatchDto dto)
        {
            var validation = await _patchValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result.Failure(validation.Errors.First().ErrorMessage);

            var libro = await _unitOfWork.Libros.GetByIdWithCategoriasAsync(id);
            if (libro == null)
                return Result.Failure("El libro no existe");

            if (dto.CategoriasIds != null)
            {
                libro.LibroCategorias.Clear();
                libro.LibroCategorias = dto.CategoriasIds
                    .Select(c => new LibroCategoria { CategoriaId = c })
                    .ToList();
            }

            _mapper.Map(dto, libro);

            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
        public async Task<Result<LibroDetailDto>> GetByIdAsync(int id)
        {
            var libro = await _unitOfWork.Libros.GetByIdWithCategoriasAsync(id);
            if (libro == null)
                return Result<LibroDetailDto>.Failure("El libro no existe");

            return Result<LibroDetailDto>.Success(
                _mapper.Map<LibroDetailDto>(libro)
            );
        }
        public async Task<Result<PagedResult<LibroCardDto>>> GetCardsAsync(PaginationParams pagination)
        {
            var query = _unitOfWork.Libros
                .Query()
                .Select(l => new LibroCardDto
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    Portada = l.Portada,
                    ValoracionPromedio = l.ValoracionPromedio
                });

            var pagedResult = query.ToPagedResult(pagination.Page, pagination.PageSize);

            return Result<PagedResult<LibroCardDto>>.Success(pagedResult);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var libro = await _unitOfWork.Libros.GetByIdAsync(id);
            if (libro == null)
                return Result.Failure("El libro no existe");

            _unitOfWork.Libros.Remove(libro);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }
}
