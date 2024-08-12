namespace Digi.Business.TotalAmount;

public interface ICalculateTotalAmount
{
    public Task Calculate(long orderId);
}