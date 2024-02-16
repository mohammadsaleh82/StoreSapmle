using Application.Core;
using MediatR;
using Persistence.Context;

namespace Application.Products;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly StoreContext _context;

        public Handler(StoreContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);

            if (product == null)
                return Result<Unit>.Failure("Product not found.");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Products", product.Image);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            _context.Products.Remove(product);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (result)
                return Result<Unit>.Success(Unit.Value);
            else
                return Result<Unit>.Failure("Failed to delete the product.");
        }
    }
}