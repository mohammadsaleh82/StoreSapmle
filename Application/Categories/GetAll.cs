using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Categories;

public class GetAll
{
    public class Query : IRequest<Result<List<Domain.Entities.Products.Category>>>
    {
    }

    public class Handler : IRequestHandler<Query, Result<List<Domain.Entities.Products.Category>>>
    {
        private readonly StoreContext _context;

        public Handler(StoreContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Domain.Entities.Products.Category>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var categories = _context.Categories;
            return Result<List<Domain.Entities.Products.Category>>.Success(await categories.ToListAsync());
        }
    }
}