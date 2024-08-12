using AutoMapper;
using MediatR;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Data.Domain;
using Digi.Data.UnitOfWork;
using Digi.Schema;

namespace Digi.Business.Command;

public class CategoryCommandHandler :
    IRequestHandler<CreateCategoryCommand, ApiResponse<CategoryResponse>>,
    IRequestHandler<UpdateCategoryCommand, ApiResponse>,
    IRequestHandler<DeleteCategoryCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
    {
        var entity = unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == request.ProductId);

        entity.Stock = request.Stock;
        
        unitOfWork.ProductRepository.Update(entity);
        await unitOfWork.Complete();
        
        return new ApiResponse();
    }

    public async Task<ApiResponse<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<CategoryRequest, Category>(request.Request);
        await unitOfWork.CategoryRepository.Insert(mapped);
        await unitOfWork.Complete();

        var response = mapper.Map<CategoryResponse>(mapped);
        
        return new ApiResponse<CategoryResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<CategoryRequest, Category>(request.Request);
        var entity = unitOfWork.CategoryRepository.FirstOrDefault(x => x.Id == request.CategoryId);
        entity.Name = mapped.Name;
        
        unitOfWork.CategoryRepository.Update(entity);
        await unitOfWork.Complete();
        
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = unitOfWork.CategoryRepository.FirstOrDefault(x => x.Id == request.CategoryId);
        var products = await unitOfWork.ProductRepository.Where(x => x.Categories.Contains(category));

        if (products.Any())
        {
            return new ApiResponse("Category cannot be deleted because it has products.");   
        }
        
        await unitOfWork.CategoryRepository.Delete(request.CategoryId);
        await unitOfWork.Complete();
        
        return new ApiResponse();
    }
}