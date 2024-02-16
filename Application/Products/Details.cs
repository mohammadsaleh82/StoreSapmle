using Application.Core;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;
using Persistence.Context;

namespace Application.Products;

public class Details
{
    public class Query : IRequest<Result<Product>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Product>>
    {
        private readonly StoreContext _context;

        public Handler(StoreContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<Result<Product>> Handle(Query request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);

            if (product == null)
                return Result<Product>.Failure("Product not found.");

            return Result<Product>.Success(product);
        }
    }
}