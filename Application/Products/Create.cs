using System.ComponentModel.DataAnnotations;
using Application.Core;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence.Context;

namespace Application.Products;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public ProductDto ProductDto { get; set; }
        [Required] public IFormFile ImageFile { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public Handler(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.ProductDto);
            if (request.ImageFile is not null)
            {
                product.Image = Guid.NewGuid().ToString()
                    .Replace("-", "," )+ Path.GetExtension(request.ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Products");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                filePath = Path.Combine(filePath, product.Image);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(fileStream, cancellationToken);
                }
            }

            _context.Products.Add(product);
            var result = await _context.SaveChangesAsync() > 0;
            if (result)
            {
                return Result<Unit>.Success(Unit.Value);
            }

            return Result<Unit>.Failure("");
        }
    }
}