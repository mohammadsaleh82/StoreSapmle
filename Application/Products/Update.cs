using Application.Core;
using Application.Dtos.Products;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence.Context;

namespace Application.Products;

public class Update
{
    public class Command : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
        public ProductDto ProductDto { get; set; }
        public IFormFile? ImageFile { get; set; }
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
            var product = await _context.Products.FindAsync(request.Id);

            if (product == null)
                return Result<Unit>.Failure("Product not found.");
            
            _mapper.Map(request.ProductDto, product);
            if (request.ImageFile is not null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Products", product.Image);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                product.Image = Guid.NewGuid().ToString("N") + Path.GetExtension(request.ImageFile.FileName);
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Products");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                filePath = Path.Combine(directoryPath, product.Image);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(fileStream, cancellationToken);
                }

            }

            _context.Products.Update(product);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (result)
                return Result<Unit>.Success(Unit.Value);
            else
                return Result<Unit>.Failure("Failed to update the product.");
        }
    }
}
