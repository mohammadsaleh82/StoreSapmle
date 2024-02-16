using System.Diagnostics;
using Application.Dtos.Products;
using AutoMapper;
using Domain;
using Domain.Entities.Products;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
    }
}
