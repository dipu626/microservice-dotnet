using Dapper;
using Discount.GRPC.Models;
using Npgsql;

namespace Discount.GRPC.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly IConfiguration configuration;

        public CouponRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Coupon> GetDiscountAsync(string productId)
        {
            string connectionString = this.configuration.GetConnectionString("DiscountDB");
            using NpgsqlConnection connection = new(connectionString);

            string sql = $@"SELECT 
                                    * 
                            FROM 
                                    {nameof(Coupon)} 
                            WHERE 
                                    {nameof(Coupon.ProductId)} = @ProductId ";
            var parameters = new
            {
                ProductId = productId,
            };

            Coupon coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(sql, parameters);

            if (coupon is null)
            {
                return new Coupon
                {
                    Amount = 0,
                    ProductName = "No discount"
                };
            }

            return coupon;
        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            string connectionString = this.configuration.GetConnectionString("DiscountDB");
            using NpgsqlConnection connection = new(connectionString);

            string sql = $@"INSERT INTO 
                                        {nameof(Coupon)} 
                            (
                                        {nameof(Coupon.ProductId)},
                                        {nameof(Coupon.ProductName)},
                                        {nameof(Coupon.Description)},
                                        {nameof(Coupon.Amount)}
                            )
                            VALUES
                            (
                                        @ProductId,
                                        @ProductName,
                                        @Description,
                                        @Amount
                            ) ";

            var parameters = new
            {
                ProductId = coupon.ProductId,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount,
            };

            var affected = await connection.ExecuteAsync(sql, parameters);

            return affected > 0;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            string connectionString = this.configuration.GetConnectionString("DiscountDB");
            using NpgsqlConnection connection = new(connectionString);

            string sql = $@"UPDATE 
                                        {nameof(Coupon)} 
                            SET 
                                        {nameof(Coupon.ProductId)} = @ProductId,
                                        {nameof(Coupon.ProductName)} = @ProductName,
                                        {nameof(Coupon.Description)} = @Description,
                                        {nameof(Coupon.Amount)} = @Amount 
                            WHERE 
                                        {nameof(Coupon.Id)} = @Id ";

            var parameters = new
            {
                ProductId = coupon.ProductId,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount,
            };

            int affected = await connection.ExecuteAsync(sql, parameters);

            return affected > 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productId)
        {
            string connectionString = this.configuration.GetConnectionString("DiscountDB");
            using NpgsqlConnection connection = new(connectionString);

            string sql = $@"DELETE FROM 
                                        {nameof(Coupon)} 
                            WHERE 
                                        {nameof(Coupon.ProductId)} = @ProductId ";

            var parameters = new
            {
                ProductId = productId,
            };

            int affected = await connection.ExecuteAsync(sql, parameters);

            return affected > 0;
        }
    }
}
