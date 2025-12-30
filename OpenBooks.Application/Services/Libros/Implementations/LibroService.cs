using AutoMapper;
using FluentValidation;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.Services.Libros.Interfaces;
using OpenBooks.Domain.Entities.Libros;

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

        public async Task<Result<LibroDetailDto>> CreateAsync(LibroCreateDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result<LibroDetailDto>.Failure(validation.Errors.First().ErrorMessage);

            var libro = _mapper.Map<Libro>(dto);

            await _unitOfWork.Libros.AddAsync(libro);
            await _unitOfWork.CommitAsync();

            var libroDto = await GetLibroDetailDtoAsync(libro.Id);

            return libroDto;
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
        public async Task<Result<PagedResult<LibroCardDto>>> GetRecommendedAsync(PaginationParams pagination)
        {
            var query = _unitOfWork.Libros
                .Query()
                .OrderBy(_ => Guid.NewGuid())
                .Select(l => new LibroCardDto
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    Portada = l.Portada,
                    ValoracionPromedio = l.ValoracionPromedio
                });

            var pagedResult = query.ToPagedResult(
                pagination.Page,
                pagination.PageSize
            );

            return Result<PagedResult<LibroCardDto>>.Success(pagedResult);
        }
        public async Task<Result<PagedResult<LibroCardDto>>> GetTopRatedAsync(PaginationParams pagination)
        {
            var query = _unitOfWork.Libros
                .Query()
                .Where(l => l.ValoracionPromedio > 0)
                .OrderByDescending(l => l.ValoracionPromedio)
                .Select(l => new LibroCardDto
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    Portada = l.Portada,
                    ValoracionPromedio = l.ValoracionPromedio
                });

            var pagedResult = query.ToPagedResult(
                pagination.Page,
                pagination.PageSize
            );

            return Result<PagedResult<LibroCardDto>>.Success(pagedResult);
        }

        public async Task<Result<PagedResult<LibroCardDto>>> SearchAsync(LibroSearchParams searchParams)
        {
            var query = _unitOfWork.Libros
                .Query(
                    l =>
                        (string.IsNullOrEmpty(searchParams.Search) ||
                            l.Titulo.Contains(searchParams.Search)) &&
                        (!searchParams.CategoriaId.HasValue ||
                            l.LibroCategorias.Any(lc => lc.CategoriaId == searchParams.CategoriaId)) &&
                        (string.IsNullOrEmpty(searchParams.Autor) ||
                            l.Autor.Contains(searchParams.Autor)),
                    l => l.LibroCategorias
        );

            query = searchParams.OrderBy switch
            {
                LibroOrderBy.TituloAsc => query.OrderBy(l => l.Titulo),
                LibroOrderBy.TituloDesc => query.OrderByDescending(l => l.Titulo),
                LibroOrderBy.ValoracionAsc => query.OrderBy(l => l.ValoracionPromedio),
                LibroOrderBy.ValoracionDesc => query.OrderByDescending(l => l.ValoracionPromedio),
                _ => query.OrderBy(l => l.Id) 
            };

            var paged = query.Select(l => new LibroCardDto
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Portada = l.Portada,
                ValoracionPromedio = l.ValoracionPromedio
            })
            .ToPagedResult(searchParams.Page, searchParams.PageSize);

            return Result<PagedResult<LibroCardDto>>.Success(paged);
        }
        private async Task<Result<LibroDetailDto>> GetLibroDetailDtoAsync(int libroId)
        {
            var libroDto = await _unitOfWork.Libros.GetDetailAsync(libroId);

            if (libroDto == null)
                return Result<LibroDetailDto>.Failure("El libro no existe");

            return Result<LibroDetailDto>.Success(libroDto);

        }
    }
}
