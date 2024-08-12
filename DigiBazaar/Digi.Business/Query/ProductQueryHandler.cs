using AutoMapper;
using MediatR;
using Digi.Base;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Data.Domain;
using Digi.Data.UnitOfWork;
using Digi.Schema;
using Microsoft.VisualBasic;

namespace Digi.Business.Query;

public class ProductQueryHandler : 
    IRequestHandler<GetAllProductQuery, ApiResponse<List<ProductResponse>>>,
    IRequestHandler<GetProductByCategoryQuery, ApiResponse<List<ProductResponse>>>,
    IRequestHandler<GetProductByStatusQuery, ApiResponse<List<ProductResponse>>>

{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<ProductResponse>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        var entityList = await unitOfWork.ProductRepository.GetAll("Categories");
        var mappedList = mapper.Map<List<ProductResponse>>(entityList);
        
        return new ApiResponse<List<ProductResponse>>(mappedList);
    }

    public async Task<ApiResponse<List<ProductResponse>>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = unitOfWork.CategoryRepository.FirstOrDefault(x => x.Name == request.CategoryName);
        var entity = await unitOfWork.ProductRepository.Where(x => x.Categories.Contains(category));
        
        var mappedList = mapper.Map<List<ProductResponse>>(entity);
        
        return new ApiResponse<List<ProductResponse>>(mappedList);
    }

    public async Task<ApiResponse<List<ProductResponse>>> Handle(GetProductByStatusQuery request, CancellationToken cancellationToken)
    { 
        var entity = await unitOfWork.ProductRepository.Where(x => x.IsActive == request.IsActive);
        var mappedList = mapper.Map<List<ProductResponse>>(entity);
        
        return new ApiResponse<List<ProductResponse>>(mappedList);
    }
}