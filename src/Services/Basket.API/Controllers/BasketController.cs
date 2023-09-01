using Basket.API.GRPCServices;
using Basket.API.Models;
using Basket.API.Repositories;
using CoreApiResponse;
using Discount.GRPC.Protos;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketController : BaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IDiscountGRPCService discountGRPCService;

        public BasketController(IBasketRepository basketRepository, IDiscountGRPCService discountGRPCService)
        {
            this.basketRepository = basketRepository;
            this.discountGRPCService = discountGRPCService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBasket(string userName)
        {
            try
            {
                ShoppingCart basket = await basketRepository.GetBasketAsync(userName);

                return CustomResult("Basket data load successfully.", basket);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
        {
            try
            {
                // Communicate Discount.GRPC to check if this product has any discount or not
                // Calculate final price
                // Create DiscountGRPC Service

                foreach (ShoppingCartItem item in basket.Items)
                {
                    CouponResponse coupon = await this.discountGRPCService.GetDiscount(item.ProductId);
                    item.Price -= coupon.Amount;
                }

                ShoppingCart updatedBasket = await basketRepository.UpdateBasketAsync(basket);

                return CustomResult("Basket modified done.", updatedBasket);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            try
            {
                await basketRepository.DeleteBasketAsync(userName);

                return CustomResult("Basket has been deleted.");
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}
