
using artfriks.Data;
using artfriks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Services
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _shoppingCartId;


        private ShoppingCart(ApplicationDbContext dbContext, string id)
        {
            _dbContext = dbContext;
            _shoppingCartId = id;

        }

        public static ShoppingCart GetCart(ApplicationDbContext db, CartId context)
         => GetCart(db, GetCartId(context));

        public static ShoppingCart GetCart(ApplicationDbContext db, string gid)
            => new ShoppingCart(db, gid);

        public async Task AddToCart(ArtWork product, string productType, int productUnit, int count, string userId)
        {
            // Get the matching cart and product instances
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(
                c => c.CartId == _shoppingCartId
                && c.ProductId == product.Id
                && c.ProductUnit == productUnit);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = _shoppingCartId,
                    ProductType = productType,
                    ProductUnit = productUnit,
                    UserId = userId,
                    Count = count,
                    DateCreated = DateTime.Now
                };
                cartItem.totalPrice = cartItem.Count * product.Price;
                _dbContext.CartItems.Add(cartItem);
            }
            else
            {
                    // If the item does exist in the cart, then add one to the quantity
                    cartItem.Count++;
                    cartItem.totalPrice = cartItem.Count * cartItem.Product.Price;
            
            }
        }

        public int RemoveFromCart(int id, string productType, int productUnit)
        {
            // Get the cart
            var cartItem = _dbContext.CartItems.SingleOrDefault(
                cart => cart.CartId == _shoppingCartId
                && cart.CartItemId == id
          );
            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                    cartItem.totalPrice = cartItem.Count * cartItem.ProductPrice;
                }
                else
                {
                    _dbContext.CartItems.Remove(cartItem);
                }
            }
            return itemCount;
        }

        public async Task EmptyCart()
        {
            var cartItems = await _dbContext
                .CartItems
                .Where(cart => cart.CartId == _shoppingCartId)
                .ToArrayAsync();
            _dbContext.CartItems.RemoveRange(cartItems);
        }

        public Task<List<CartItem>> GetCartItems()
        {
            return _dbContext
                .CartItems
                .Where(cart => cart.CartId == _shoppingCartId)
                .Include(c => c.Product)
                .Select(x => new CartItem
                {
                    CartId = x.CartId,
                    CartItemId = x.CartItemId,
                    Count = x.Count,
                    DateCreated = x.DateCreated,
                    Product = x.Product,
                    ProductId = x.ProductId,
                    ProductPrice = x.ProductPrice,
                    ProductType = x.ProductType,
                    UserId = x.UserId,
                    ProductUnit = x.ProductUnit,
                    totalPrice = x.totalPrice,
                })
                .ToListAsync();
        }

        public Task<List<string>> GetProductTitles()
        {
            return _dbContext
                .CartItems
                .Where(cart => cart.CartId == _shoppingCartId)
                .Select(c => c.Product.Title)
                .OrderBy(n => n)
                .ToListAsync();
        }

        public Task<int> GetCount()
        {
            // Get the count of each item in the cart and sum them up
            return _dbContext
                .CartItems
                .Where(c => c.CartId == _shoppingCartId)
                .Select(c => c.Count)
                .SumAsync();
        }

        public Task<int> GetIteamTotal()
        {
            return _dbContext
                .CartItems
                .Include(c => c.Product)
                .Where(c => c.CartId == _shoppingCartId)
                .Select(c => c.Count)
                .SumAsync();
        }

        public Task<int> GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            return _dbContext
                .CartItems
                .Include(c => c.Product)
                .Where(c => c.CartId == _shoppingCartId)
                .Select(c => Convert.ToInt32( c.Product.Price * c.Count))
                .SumAsync(); ;
        }

        public async Task<int> CreateOrder(Order order, string userId)
        {
            int orderTotal = 0;
            order.UserId = userId;
            var cartItems = await GetCartItems();
            _dbContext.Orders.Add(order);
            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var product = await _dbContext.ArtWorks.SingleAsync(a => a.Id == item.ProductId);
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice =item.Product.Price ,
                    Quantity = item.Count,
                    ProductUnit = item.ProductUnit,
                    totalPrice = item.totalPrice,
                    ProductType = item.ProductType

                };
                // Set the order total of the shopping cart
                orderTotal += Convert.ToInt32( item.Product.Price);

                _dbContext.OrderDetails.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.OriginalAmount = orderTotal;
            order.Total = orderTotal - (orderTotal * order.DiscountPercentage / 100);
            order.DiscountedAmount = (orderTotal * order.DiscountPercentage / 100); ;
            OrderTransaction Ot = new Models.OrderTransaction();
            DeliveryDetails dd = new DeliveryDetails();
            await _dbContext.SaveChangesAsync();
            Ot.OrderId = order.OrderId;
            Ot.TransactionAmount = order.Total;
            Ot.TransactionStatus = 0;
            dd.OrderId = order.OrderId;
            dd.DeliveryAssignedTime = DateTime.Now.AddDays(5);
            _dbContext.DeliveryDetails.Add(dd);
            _dbContext.OrderTransactions.Add(Ot);
            await _dbContext.SaveChangesAsync();
            // Empty the shopping cart
            await EmptyCart();

            // Return the OrderId as the confirmation number
            return order.OrderId;
        }

        public async Task<int> OrderTransaction(OrderTransaction order, string userId)
        {
            try
            {
                int returnValue = 0;
                var cartItems = await GetCartItems();
                var Ot = _dbContext.OrderTransactions.Where(x => x.OrderId == order.OrderId).First();
                if (Ot == null) { returnValue = 0; return returnValue; }
                Ot.ModeOfPayment = order.ModeOfPayment;
                Order od = _dbContext.Orders.Where(x => x.OrderId == order.OrderId).First();
                _dbContext.OrderTransactions.Update(Ot);
                await _dbContext.SaveChangesAsync();
                if (Ot.TransactionAmount != order.TransactionAmount)
                {

                    returnValue = 1;
                    return returnValue;
                }
                if (order.ModeOfPayment == "1")
                {
                    od.Active = 1;
                    _dbContext.Orders.Update(od);
                    await _dbContext.SaveChangesAsync();
                    returnValue = 2; return returnValue;
                }
                if (order.ModeOfPayment == "2")
                { returnValue = 3; return returnValue; }
                // Return the OrderId as the confirmation number
                return returnValue;
            }
            catch 
            {
                return 5;
            }
        }

        // We're using HttpContextBase to allow access to sessions.
        public static string GetCartId(CartId cartId)
        {
            if (cartId.cartId == null)
                return Guid.NewGuid().ToString();
            else
                return cartId.cartId;
        }
    }
}
