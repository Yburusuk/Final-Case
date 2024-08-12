using AutoMapper;
using MediatR;
using Digi.Base;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Data.Domain;
using Digi.Data.UnitOfWork;
using Digi.Schema;

namespace Digi.Business.Query;

public class OrderQueryHandler : 
    IRequestHandler<GetOrderByStatusQuery, ApiResponse<List<OrderResponse>>>

{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public OrderQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<CategoryResponse>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var entityList = await unitOfWork.CategoryRepository.GetAll();
        var mappedList = mapper.Map<List<CategoryResponse>>(entityList);
        
        return new ApiResponse<List<CategoryResponse>>(mappedList);
    }

    public async Task<ApiResponse<List<OrderResponse>>> Handle(GetOrderByStatusQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.OrderRepository.Where(x => x.IsActive == request.IsActive, "OrderDetail.Products");
        
        var response = mapper.Map<List<OrderResponse>>(entity);
        
        return new ApiResponse<List<OrderResponse>>(response);
    }
}