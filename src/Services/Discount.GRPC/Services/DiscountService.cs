using Discount.GRPC.Models;
using Discount.GRPC.Protos;
using Discount.GRPC.Repository;
using Grpc.Core;

namespace Discount.GRPC.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ICouponRepository couponRepository;
        private readonly ILogger<DiscountService> logger;

        public DiscountService(ICouponRepository couponRepository, ILogger<DiscountService> logger)
        {
            this.couponRepository = couponRepository;
            this.logger = logger;
        }

        public override async Task<CouponResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            Coupon coupon = await this.couponRepository.GetDiscountAsync(request.ProductId);

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Discount not found"));
            }

            this.logger.LogInformation($"Discount is retrived for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");

            return new CouponResponse
            {
                Id = coupon.Id,
                ProductId = coupon.ProductId,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount,
            };
        }
    }
}
