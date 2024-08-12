using AutoMapper;
using MediatR;
using Digi.Base.Response;
using Digi.Base.Sessions;
using Digi.Business.Cqrs;
using Digi.Business.TotalAmount;
using Digi.Business.CardConfirmation;
using Digi.Data.Domain;
using Digi.Data.UnitOfWork;
using Digi.Schema;

namespace Digi.Business.Command;

public class OrderCommandHandler :
    IRequestHandler<BuyOrderCommand, ApiResponse<OrderResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly ISessionContext sessionContext;

    public OrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ISessionContext sessionContext)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.sessionContext = sessionContext;
    }

    public async Task<ApiResponse<OrderDetailResponse>> Handle(AddProductToOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var product = unitOfWork.ProductRepository.FirstOrDefault(x=> x.Id == request.Request.ProductId && x.IsActive == true);
        
        var orderDetail = unitOfWork.OrderDetailRepository.FirstOrDefault(x=> x.UserName == request.Request.UserName && x.IsActive == true, "Products", "Order");
        
        var productOrderDetail = new ProductOrderDetail
        {
            ProductId = product.Id,
            OrderDetailId = orderDetail.Id
        };
        
        await unitOfWork.ProductOrderDetailRepository.Insert(productOrderDetail);
        
        await unitOfWork.Complete();

        var order = unitOfWork.OrderRepository.FirstOrDefault(x => x.Id == orderDetail.OrderId && x.IsActive == true, "OrderDetail");
        
        var calculate = new CalculateTotalAmount(unitOfWork);

        await calculate.Calculate(order.Id);

        var response = mapper.Map<OrderDetailResponse>(orderDetail);
        
        return new ApiResponse<OrderDetailResponse>(response);
    }
    
    public async Task<ApiResponse<OrderResponse>> Handle(BuyOrderCommand request, CancellationToken cancellationToken)
    {
        var confirmCard = new ConfirmCard();
        
        bool checkCard = confirmCard.Confirm(request.Request.NameSurname, request.Request.CardNumber, 
                                            request.Request.Cvv, request.Request.ExpirationYear, request.Request.ExpirationMonth);
        
        if (!checkCard)
        {
            throw new Exception("Invalid Card Information. Check your card information and try again.");
        }
        
        List<Product> products = new List<Product>();
        decimal couponAmount = 0;
        decimal usedWalletBalance = 0;
        decimal totalAmount = unitOfWork.OrderRepository.FirstOrDefault(x => x.UserName == sessionContext.Session.UserName && x.IsActive == true).TotalAmount;
        List<decimal> pricePercentages = new List<decimal>();
        var order = unitOfWork.OrderRepository.FirstOrDefault(x => x.UserName == request.Request.UserName && x.IsActive == true, "OrderDetail");
        
        var productOrderDetails = await unitOfWork.ProductOrderDetailRepository
            .Where(x => x.OrderDetail.UserName == request.Request.UserName && x.OrderDetail.IsActive == true, "OrderDetail");

        foreach (var productOrderDetail in productOrderDetails)
        {
            var product = unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == productOrderDetail.ProductId && x.IsActive == true);
            products.Add(product);
        }

        if (request.Request.CouponCode != "")
        {
            var coupon = unitOfWork.CouponRepository.FirstOrDefault(x =>
                x.CouponCode == request.Request.CouponCode && x.IsActive == true);

            couponAmount = coupon.CouponAmount;

            order.CouponCode = coupon.CouponCode;
            order.CouponAmount = couponAmount;
        }

        if (request.Request.UseWalletBalance)
        {
            var walletBalance = sessionContext.Session.WalletBalance;
            
            usedWalletBalance = walletBalance;
        }
        
        //Deducting couponAmount from every product's price proportionally if coupon is applied.
        foreach (var product in products)
        {
            decimal discountCouponAmount = (product.Price/totalAmount) * couponAmount;
            product.Price -= discountCouponAmount;
        }

        totalAmount = totalAmount - couponAmount;
        
        //Deducting walletBalance from every product's price proportionally if walletBalance is used.
        foreach (var product in products)
        {
            decimal discountWalletBalanceAmount = (product.Price/totalAmount) * usedWalletBalance;
            product.Price -= discountWalletBalanceAmount;
        }

        totalAmount = totalAmount - usedWalletBalance;

        foreach (var product in products)
        {
            var points = product.Price * product.PointsPercentage;

            if (points > product.MaxPoints)
            {
                points = product.MaxPoints;
            }

            order.PointsEarned += points;
        }

        sessionContext.Session.WalletBalance = order.PointsEarned;

        order.PointsSpent = usedWalletBalance;
        order.IsActive = false;
        order.OrderDetail.IsActive = false;

        unitOfWork.OrderRepository.Update(order);
        await unitOfWork.Complete();

        await unitOfWork.Complete();
        
        
        foreach (var productOrderDetail in productOrderDetails)
        {
            productOrderDetail.IsActive = false;
            unitOfWork.ProductOrderDetailRepository.Update(productOrderDetail);
            await unitOfWork.Complete();
        }
        
        var newOrder = new Order
        {
            UserId = sessionContext.Session.UserId,
            UserName = sessionContext.Session.UserName,
            OrderNumber = Guid.NewGuid().ToString("N").Substring(0, 9),
            TotalAmount = 0,
            CouponCode = "",
            CouponAmount = 0,
            PointsSpent = 0,
            PointsEarned = 0
        };

        await unitOfWork.OrderRepository.Insert(newOrder);
        await unitOfWork.Complete();
        
        var createdOrder = unitOfWork.OrderRepository.FirstOrDefault(x => x.OrderNumber == newOrder.OrderNumber);
        
        var newOrderDetail = new OrderDetail
        {
            UserId = sessionContext.Session.UserId,
            UserName = sessionContext.Session.UserName,
            OrderId = createdOrder.Id,
        };
        
        await unitOfWork.OrderDetailRepository.Insert(newOrderDetail);
        await unitOfWork.Complete();

        var response = mapper.Map<OrderResponse>(order);
        
        return new ApiResponse<OrderResponse>(response);
    }
}