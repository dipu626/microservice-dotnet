using CoreApiResponse;
using Discount.API.Models;
using Discount.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DiscountController : BaseController
    {
        private readonly ICouponRepository couponRepository;

        public DiscountController(ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDiscount(string productId)
        {
            try
            {
                Coupon coupon = await this.couponRepository.GetDiscountAsync(productId);

                return CustomResult("Get discount successfully", coupon);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDiscount([FromBody] Coupon coupon)
        {
            try
            {
                bool isSaved = await this.couponRepository.CreateDiscountAsync(coupon);

                if (isSaved)
                {
                    return CustomResult("Coupon saved successfully", coupon);
                }

                return CustomResult("Coupon saved failed", coupon, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            try
            {
                bool isUpdated = await this.couponRepository.UpdateDiscountAsync(coupon);

                if (isUpdated)
                {
                    return CustomResult("Coupon has been modified", coupon);
                }

                return CustomResult("Coupon update failed", coupon, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDiscount(string productId)
        {
            try
            {
                bool isDeleted = await this.couponRepository.DeleteDiscountAsync(productId);

                if (isDeleted)
                {
                    return CustomResult("Coupon has been deleted");
                }

                return CustomResult("Coupon deleted failed", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}
