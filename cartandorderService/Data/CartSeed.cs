using cartandorderService.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cartandorderService.Data
{
    public class CartSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.Cart.Any())
            {
                var cart = new List<Cart> {
                new Cart
                {
                    CreatedBy="dhruvchawla0101@gmail.com",
                    ProductId=1,
                    Quantity=3
                },
                new Cart
                {
                    CreatedBy="dhruvchawla0101@gmail.com",
                    ProductId=2,
                    Quantity=3
                },
                };
                context.Cart.AddRange(cart);
                await context.SaveChangesAsync();
            }
            if (!context.Order.Any())
            {
                var ord = new List<Orders> {
                new Orders
                {
                    OrderOf="dhruvchawla0101@gmail.com",
                    ProductId=3,
                    Quantity=3
                },
                new Orders
                {
                    OrderOf="dhruvchawla0101@gmail.com",
                    ProductId=2,
                    Quantity=3
                },
                };
                context.Order.AddRange(ord);
                await context.SaveChangesAsync();
            }
        }
    }
}
