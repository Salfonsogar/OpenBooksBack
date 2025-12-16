using AutoMapper;
using FluentValidation;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.DTOs.Usuarios;
using OpenBooks.Application.Services.Libros.Interfaces;
using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Services.Libros.Implementations
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoriaUpdateDto> _updateValidator;
        private readonly IValidator<CategoriaCreateDto> _createValidator;
        private readonly IValidator<CategoriaPatchDto> _patchValidator;

        public CategoriaService(
            IUnitOfWork unit,
            IMapper mapper,
            IValidator<CategoriaUpdateDto> updateValidator,
            IValidator<CategoriaCreateDto> createValidator,
            IValidator<CategoriaPatchDto> patchValidator)
        {
            _unit = unit;
            _mapper = mapper;
            _updateValidator = updateValidator;
            _createValidator = createValidator;
            _patchValidator = patchValidator;
        }

        public async Task<Result<IEnumerable<CategoriaResponseDto>>> GetAllAsync()
        {
            var categorias = await _unit.Categorias.GetAllAsync();
            var data = _mapper.Map<IEnumerable<CategoriaResponseDto>>(categorias);

            return Result<IEnumerable<CategoriaResponseDto>>.Success(data);
        }
        public async Task<Result<CategoriaResponseDto>> GetByIdAsync(int id)
        {
            var categoria = await _unit.Categorias.GetByIdAsync(id);

            if (categoria == null)
                return Result<CategoriaResponseDto>.Failure("Categoría no encontrada");

            return Result<CategoriaResponseDto>.Success(
                _mapper.Map<CategoriaResponseDto>(categoria)
            );
        }


        public async Task<Result<CategoriaResponseDto>> CreateAsync(CategoriaCreateDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var nombreResult = await ValidarNombreUnicoAsync(dto.Nombre);
            if (!nombreResult.IsSuccess)
                return Result<CategoriaResponseDto>.Failure(nombreResult.Error!);

            var categoria = _mapper.Map<Categoria>(dto);

            await _unit.Categorias.AddAsync(categoria);
            await _unit.CommitAsync();

            return Result<CategoriaResponseDto>.Success(
                _mapper.Map<CategoriaResponseDto>(categoria)
            );
        }

        public async Task<Result<CategoriaResponseDto>> UpdateAsync(int id, CategoriaUpdateDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var categoria = await _unit.Categorias.GetByIdAsync(id);
            if (categoria == null)
                return Result<CategoriaResponseDto>.Failure("Categoría no encontrada");

            var nombreResult = await ValidarNombreUnicoAsync(dto.Nombre, id);
            if (!nombreResult.IsSuccess)
                return Result<CategoriaResponseDto>.Failure(nombreResult.Error!);

            categoria.Nombre = dto.Nombre;

            _unit.Categorias.Update(categoria);
            await _unit.CommitAsync();

            return Result<CategoriaResponseDto>.Success(
                _mapper.Map<CategoriaResponseDto>(categoria)
            );
        }


        public async Task<Result> DeleteAsync(int id)
        {
            var categoria = await _unit.Categorias.GetByIdAsync(id);
            if (categoria == null)
                return Result.Failure("Categoría no encontrada");

            _unit.Categorias.Remove(categoria);
            await _unit.CommitAsync();

            return Result.Success();
        }

        public async Task<Result<CategoriaResponseDto>> PatchAsync(int id, CategoriaPatchDto dto)
        {
            await _patchValidator.ValidateAndThrowAsync(dto);

            var categoria = await _unit.Categorias.GetByIdAsync(id);
            if (categoria == null)
                return Result<CategoriaResponseDto>.Failure("Categoría no encontrada");

            if (dto.Nombre != null)
            {
                var nombreResult = await ValidarNombreUnicoAsync(dto.Nombre, id);
                if (!nombreResult.IsSuccess)
                    return Result<CategoriaResponseDto>.Failure(nombreResult.Error!);
            }

            _mapper.Map(dto, categoria);

            _unit.Categorias.Update(categoria);
            await _unit.CommitAsync();

            return Result<CategoriaResponseDto>.Success(
                _mapper.Map<CategoriaResponseDto>(categoria)
            );
        }

        private async Task<Result> ValidarNombreUnicoAsync(string nombre, int? categoriaId = null)
        {
            var existente = await _unit.Categorias.GetByNombreAsync(nombre);

            if (existente != null && existente.Id != categoriaId)
                return Result.Failure("Ya existe una categoría con ese nombre");

            return Result.Success();
        }
        public async Task<Result<PagedResult<CategoriaResponseDto>>> GetAllAsync(PaginationParams pagination)
        {
            var query = _unit.Categorias
                .Query()
                .OrderBy(c => c.Id);

            var pagedResult = query.ToPagedResult(pagination.Page, pagination.PageSize);

            return Result<PagedResult<CategoriaResponseDto>>.Success(
                new PagedResult<CategoriaResponseDto>
                {
                    Items = _mapper.Map<IEnumerable<CategoriaResponseDto>>(pagedResult.Items),
                    TotalItems = pagedResult.TotalItems,
                    Page = pagedResult.Page,
                    PageSize = pagedResult.PageSize
                }
            );
        }
    }
}
