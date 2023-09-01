using AutoMapper;
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
        private readonly IMapper mapper;

        public DiscountService(ICouponRepository couponRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            this.couponRepository = couponRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public override async Task<CouponResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            Coupon coupon = await this.couponRepository.GetDiscountAsync(request.ProductId);

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Discount not found"));
            }

            this.logger.LogInformation($"Discount is retrived for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");

            //return new CouponResponse
            //{
            //    Id = coupon.Id,
            //    ProductId = coupon.ProductId,
            //    ProductName = coupon.ProductName,
            //    Description = coupon.Description,
            //    Amount = coupon.Amount,
            //};

            return this.mapper.Map<CouponResponse>(coupon);
        }

        public override async Task<CouponResponse> CreateDiscount(CouponRequest request, ServerCallContext context)
        {
            Coupon coupon = this.mapper.Map<Coupon>(request);
            bool isCreated = await this.couponRepository.CreateDiscountAsync(coupon);

            if (!isCreated)
            {
                this.logger.LogInformation("Discount save failed.");

                throw new RpcException(new Status(StatusCode.Internal, "Discount saved failed."));
            }

            this.logger.LogInformation($"Discount is successfully created. ProductName: {coupon.ProductName}");

            return this.mapper.Map<CouponResponse>(coupon);
        }

        public override async Task<CouponResponse> UpdateDiscount(CouponRequest request, ServerCallContext context)
        {
            Coupon coupon = this.mapper.Map<Coupon>(request);
            bool isUpdated = await this.couponRepository.UpdateDiscountAsync(coupon);

            if (!isUpdated)
            {
                this.logger.LogInformation("Discount update failed.");

                throw new RpcException(new Status(StatusCode.Internal, "Discount update failed"));
            }

            this.logger.LogInformation($"Discount is successfully updated. ProductName: {coupon.ProductName}");

            return this.mapper.Map<CouponResponse>(coupon);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            bool isDeleted = await this.couponRepository.DeleteDiscountAsync(request.ProductId);

            if (!isDeleted)
            {
                this.logger.LogInformation("Discount delete failed.");

                throw new RpcException(new Status(StatusCode.Internal, "Discount delete failed."));
            }

            this.logger.LogInformation("Discount deleted successfully.");

            return new DeleteDiscountResponse
            {
                Success = true,
            };
        }
    }
}
