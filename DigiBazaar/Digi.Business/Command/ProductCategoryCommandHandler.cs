using AutoMapper;
using MediatR;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Data.Domain;
using Digi.Data.UnitOfWork;
using Digi.Schema;

namespace Digi.Business.Command;

public class ProductCategoryCommandHandler :
    IRequestHandler<AddProductToCategoryCommand, ApiResponse<CategoryResponse>>,
    IRequestHandler<DeleteProductCategoryCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ProductCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<CategoryResponse>> Handle(AddProductToCategoryCommand request, CancellationToken cancellationToken)
    {
        var entityProduct = unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == request.Request.ProductId, "Categories");
        var entityCategory = unitOfWork.CategoryRepository.FirstOrDefault(x => x.Id == request.Request.CategoryId, "Products");
        
        entityProduct.Categories.Add(entityCategory);
        entityCategory.Products.Add(entityProduct);

        await unitOfWork.Complete();
        
        var response = mapper.Map<Category, CategoryResponse>(entityCategory);
        
        return new ApiResponse<CategoryResponse>(response);
    }

    public async Task<ApiResponse> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var entityProduct = unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == request.Request.ProductId, "Categories");
        var entityCategory = unitOfWork.CategoryRepository.FirstOrDefault(x => x.Id == request.Request.CategoryId, "Products");
        
        entityProduct.Categories.Remove(entityCategory);
        entityCategory.Products.Remove(entityProduct);

        var mapped = mapper.Map<Category, CategoryResponse>(entityCategory);
        
        await unitOfWork.Complete();
        
        return new ApiResponse();
    }
}