using AutoMapper;
using MediatR;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Business.TotalAmount;
using Digi.Data.Domain;
using Digi.Data.UnitOfWork;
using Digi.Schema;

namespace Digi.Business.Command;

public class ProductCommandHandler :
    IRequestHandler<CreateProductCommand, ApiResponse<ProductResponse>>,
    IRequestHandler<UpdateProductCommand, ApiResponse>,
    IRequestHandler<UpdateProductStockCommand, ApiResponse>,
    IRequestHandler<DeleteProductCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<ProductRequest, Product>(request.Request);
        await unitOfWork.ProductRepository.Insert(mapped);
        await unitOfWork.Complete();

        var response = mapper.Map<ProductResponse>(mapped);
        
        return new ApiResponse<ProductResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<ProductRequest, Product>(request.Request);
        var entity = unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == request.ProductId);

        entity.Name = mapped.Name;
        entity.Description = mapped.Description;
        entity.Price = mapped.Price;
        entity.Stock = mapped.Stock;
        entity.MaxPoints = mapped.MaxPoints;
        entity.PointsPercentage = mapped.PointsPercentage;
        entity.IsActive = request.IsActive;
        
        if(request.CategoryId != 0)
        {
            var category = unitOfWork.CategoryRepository.FirstOrDefault(x => x.Id == request.CategoryId);
            entity.Categories.Add(category);
        }

        unitOfWork.ProductRepository.Update(entity);
        await unitOfWork.Complete();
     
        var orders = await unitOfWork.OrderRepository.Where(x => x.OrderDetail.ProductOrderDetails
            .Any(y => y.ProductId == request.ProductId), "OrderDetail.ProductOrderDetails");

        foreach (var order in orders)
        {
            var calculate = new CalculateTotalAmount(unitOfWork);
            await calculate.Calculate(order.Id);
        }
        
        return new ApiResponse();
    }
    
    public async Task<ApiResponse> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
    {
        var entity = unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == request.ProductId);

        entity.Stock = request.Stock;
        
        unitOfWork.ProductRepository.Update(entity);
        await unitOfWork.Complete();
        
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var orders = await unitOfWork.OrderRepository.Where(x => x.OrderDetail.ProductOrderDetails
            .Any(y => y.ProductId == request.ProductId), "OrderDetail.ProductOrderDetails");
        
        await unitOfWork.ProductRepository.Delete(request.ProductId);
        await unitOfWork.Complete();

        foreach (var order in orders)
        {
            var calculate = new CalculateTotalAmount(unitOfWork);
            await calculate.Calculate(order.Id);
        }
        
        return new ApiResponse();
    }
}