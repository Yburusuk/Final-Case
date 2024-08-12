using AutoMapper;
using Digi.Data.Domain;
using Digi.Schema;

namespace Digi.Business.Mapper;

public class MapperConfig : Profile
{

    public MapperConfig()
    {
        CreateMap<Category, CategoryResponse>();
        CreateMap<CategoryRequest, Category>();
        
        CreateMap<Coupon, CouponResponse>();
        CreateMap<CouponRequest, Coupon>();
        
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.Products,
                opt => opt.MapFrom(src => src.OrderDetail.ProductOrderDetails
                    .GroupBy(p => p.Product.Name).Select(g => new ProductDetail
                    {
                        ProductName = g.Key,
                        Quantity = g.Count()
                    }).ToList()));
        
        CreateMap<OrderDetail, OrderDetailResponse>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.OrderNumber,
                opt => opt.MapFrom(src => src.Order.OrderNumber.ToString()))
            .ForMember(dest => dest.Products,
                opt => opt.MapFrom(src => src.ProductOrderDetails
                    .GroupBy(p => p.Product.Name).Select(g => new ProductDetail
                    {
                        ProductName = g.Key,
                        Quantity = g.Count()
                    }).ToList()));

        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.CategoryNames,
                opt => opt.MapFrom(src => src.Categories.Select(x => x.Name).ToList()));
        CreateMap<ProductRequest, Product>();

        // CreateMap<User, UserResponse>();
        // CreateMap<UserRequest, User>();
    }
}