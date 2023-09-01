using Discount.GRPC.Protos;

namespace Basket.API.GRPCServices
{
    public class DiscountGRPCService : IDiscountGRPCService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient;

        public DiscountGRPCService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            this.discountProtoServiceClient = discountProtoServiceClient;
        }

        public async Task<CouponResponse> GetDiscount(string productId)
        {
            var getDiscountRequest = new GetDiscountRequest
            {
                ProductId = productId
            };
            return await this.discountProtoServiceClient.GetDiscountAsync(getDiscountRequest);
        }
    }
}
