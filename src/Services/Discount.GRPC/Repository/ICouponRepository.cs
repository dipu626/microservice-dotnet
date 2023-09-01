using Discount.GRPC.Models;

namespace Discount.GRPC.Repository
{
    public interface ICouponRepository
    {
        Task<Coupon> GetDiscountAsync(string productId);
        Task<bool> CreateDiscountAsync(Coupon coupon);
        Task<bool> UpdateDiscountAsync(Coupon coupon);
        Task<bool> DeleteDiscountAsync(string productId);
    }
}
