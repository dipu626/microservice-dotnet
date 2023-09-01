using Discount.GRPC.Protos;

namespace Basket.API.GRPCServices
{
    public interface IDiscountGRPCService
    {
        Task<CouponResponse> GetDiscount(string productId);
    }
}
