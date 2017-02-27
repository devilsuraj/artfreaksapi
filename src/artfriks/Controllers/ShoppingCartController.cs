using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using artfriks.Services;
using Microsoft.AspNetCore.Authorization;
using artfriks.Models;
using artfriks.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace artfriks.Controllers
{
    [Produces("application/json")]
    public class ShoppingCartController : Controller
    {
        private readonly ILogger<ShoppingCartController> _logger;
        private ReturnMessage _returnMessage = new ReturnMessage();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        public ShoppingCartController(ApplicationDbContext dbContext, ILogger<ShoppingCartController> logger, IEmailSender emailSender,
            ISmsSender smsSender, UserManager<ApplicationUser> userManager)
        {
            DbContext = dbContext;
            _logger = logger;
            _userManager = userManager; _emailSender = emailSender;
            _smsSender = smsSender;
        }

        public ApplicationDbContext DbContext { get; }
        //
        // GET: /ShoppingCart/
        [HttpPost, Route("cart/getcart")]
        public async Task<IActionResult> Index([FromBody] CartId cartId)
        {
            try
            {
                var cart = ShoppingCart.GetCart(DbContext, cartId);
                // Set up our ViewModel
                var viewModel = new ShoppingCartViewModel
                {
                    CartItems = await cart.GetCartItems(),
                    CartTotal = await cart.GetTotal(),
                    ItemTotal = await cart.GetIteamTotal()
                };
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return Ok(new { result = ex.Message });
            }
            // Return the view

        }

        //
        // GET: /ShoppingCart/AddToCart/5
        [HttpPost, Route("cart/addtocart")]
        public async Task<IActionResult> AddToCart([FromBody] CartId cartId)
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = "";
            if (user == null)
                userId = "";
            else
                userId = user.Id;
            if (cartId.cartId == "" || cartId.cartId == null)
                cartId.cartId = ShoppingCart.GetCartId(cartId);

            var cartid = cartId.cartId;
            // Retrieve the product from the database
            var addedProduct = DbContext.ArtWorks
                .Where(product => product.Id == cartId.id).First();
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(DbContext, cartId.cartId);
            await cart.AddToCart(addedProduct, cartId.productType, cartId.productUnit, 1, userId);
            await DbContext.SaveChangesAsync();
            _logger.LogInformation("Product {0} was added to the cart.", addedProduct.Id);
            _returnMessage.Status = "0";
            _returnMessage.Message = string.Format("Product {0} was added to the cart.", addedProduct.Title);
            _returnMessage.CartId = cartId.cartId;
            // Go back to the main store page for more shopping
            return Ok(_returnMessage);
        }

        [HttpPost, Route("cart/synckart")]
        public async Task<IActionResult> SyncKart([FromBody] ShoppingCartViewModel CurrentCart)
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = "";
            if (user == null)
                userId = "";
            else
                userId = user.Id;
            var a = CurrentCart.ItemTotal;
            var b = CurrentCart.CartTotal;
            CartId cartId = new CartId();
            // Retrieve the product from the database
            foreach (CartItem item in CurrentCart.CartItems)
            {
                if (item.CartId == "" || item.CartId == null)
                    cartId.cartId = ShoppingCart.GetCartId(cartId);
                else
                    cartId.cartId = item.CartId;
                var addedProduct = DbContext.ArtWorks
                .Where(product => product.Id == item.ProductId).First();
                // Add it to the shopping cart
                var cart = ShoppingCart.GetCart(DbContext, cartId.cartId);
                if (item.Count == 0)
                {
                    await RemoveFromCart(item.ProductId, item.CartId, item.ProductType, item.ProductUnit);
                }
                await cart.AddToCart(addedProduct, item.ProductType, item.ProductUnit, item.Count, userId);
                await DbContext.SaveChangesAsync();
                _logger.LogInformation("Product {0} was added to the cart.", addedProduct.Id);
                _returnMessage.Status = "0";
                _returnMessage.Message = string.Format("Product {0} was added to the cart.", addedProduct.Title);
                _returnMessage.CartId = cartId.cartId;
            }
            // Go back to the main store page for more shopping
            return Ok(_returnMessage);
        }

        [HttpPost, Authorize, Route("cart/createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = "";
            if (user == null)
            {
                _returnMessage.Status = "0";
                _returnMessage.Message = "Access Denied";
                return BadRequest(_returnMessage);
            }
            else
            { userId = user.Id; }
            if (order.CartId == null)
            {
                _returnMessage.Status = "0";
                _returnMessage.Message = "Empty Cart";

            }
            else
            {
                var cart = ShoppingCart.GetCart(DbContext, order.CartId);
                await cart.CreateOrder(order, userId);
                // await DbContext.SaveChangesAsync();
                _logger.LogInformation("Order {0} was added to the cart.", order.OrderId);
                _returnMessage.Status = "0";
                _returnMessage.Message = string.Format("Order Id {0} .", order.OrderId.ToString());
                _returnMessage.CartId = order.OrderId.ToString();
            }
            // Go back to the main store page for more shopping
            return Ok(_returnMessage);
        }


        [HttpPost, Authorize, Route("cart/placetransaction")]
        public async Task<IActionResult> Transaction([FromBody] OrderTransaction order)
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = "";
            if (user == null)
            {
                _returnMessage.Status = "0";
                _returnMessage.Message = "Access Denied";
                return BadRequest(_returnMessage);
            }
            else
            { userId = user.Id; }
            if (order.OrderId == 0)
            {
                _returnMessage.Status = "0";
                _returnMessage.Message = "No Order Id";

            }
            else
            {
                var cart = ShoppingCart.GetCart(DbContext, order.CartId);
                var result = await cart.OrderTransaction(order, userId);
                if (result == 0)
                {
                    _returnMessage.Status = "0";
                    _returnMessage.Message = "Order Not found";
                }
                else if (result == 1)
                {
                    _returnMessage.Status = "1";
                    _returnMessage.Message = "Amount Mismatched";
                }
                else if (result == 3)
                {
                    _returnMessage.Status = "3";
                    _returnMessage.Message = "Payment failed";
                }
                else if (result == 2)
                {
                    _returnMessage.Status = "2";
                    _returnMessage.Message = "Success";

                    string Emailtext = "<!DOCTYPE html> <html> <head> <meta charset='utf-8' /> <title>Cocospices</title> <style> li { display:inline-block; padding:5px 20px 5px 20px; font-family:'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif; color:#ffffff } h4,h5,h3,span, p { padding:5px 20px 0px 20px; } body { font-family:Arial; } p { font-size:13px; } section,header,footer { max-width:600px; margin-left:auto; margin-right:auto;color:#272626 } #Products { font-size:13px; } </style> </head> <body> <header style='text-align:center; '> <div style='background-color:#1a7627;'> <div> <img src='http://bo.cocospices.com/images/logo1.png' /> </div> <ul style='display:inline'> <li> Track Order </li> <li> Cancel Order </li> <li> Download App </li> </ul> </div> </header> <main> <section style='border:1px dashed #3d3509;'> <h4>Dear Suraj Lokhande,</h4> <h3 style='font-family:'Monotype Corsiva''>Thank you for your Order !</h3> <p> We are pleased to inform that the following items in your order OD403996707386806000 have been dispatached. Thank you for shopping! </p> <p>We will send you another email once the items in your order have been shipped. Meanwhile, you can check the status of your order on Cocospices.com</p> <p>Please find below, the summary of your order OD403996707386806000</p> </section> <section id='Products'> <table width='100%'> <tr> <td> <img src='http://bo.cocospices.com/images/logo1.png' /> </td> <td> ProductName </td> <td> item Price<br /> 5999 </td> <td> Qty<br/> </td> <td> Subtotal<br /> Rs.5999 </td> </tr> <tr> <td> <img src='http://bo.cocospices.com/images/logo1.png' /> </td> <td> ProductName </td> <td> item Price<br /> 5999 </td> <td> Qty<br /> </td> <td> Subtotal<br /> Rs.5999 </td> </tr> <tr><td colspan='5' style='text-align:center'><span style='background-color:#edf88a; width:100%; border:1px solid #e59d44'> Your order will be shipped by 12/12/1212 </span></td></tr> </table> <hr /> <h2 style='text-align:right'>Total Rs. 5999</h2> <hr /> <p>Outstanding Amount Payable on Delivery:<b>Rs. 5999.</b></p> <p>DELIVERY ADDRESS</p> <h3>Suraj Lokhande</h3> <p>Advy Chemical Pvt Ltd. Plot No A-334/336/338, Road No 26, Wagle Estate, Thane - 400604, INdia</p> </section> <footer> <hr /> <p style='text-align:center'>Cocospices.com</p> <p>Unsubscribe</p> </footer> </main> </body> </html>";
                    await _emailSender.SendEmailAsync(user.Email, "Order Confirmation - Your Order with Cocospices.com [" + order.OrderId + "] has been successfully placed!", Emailtext);
                    await _smsSender.SendSmsAsync(user.UserName, "Dear Customer, Your order " + order.OrderId + " for Rs." + order.TransactionAmount + " has been received and will be delivered by " + DateTime.Now.AddDays(7).ToString("dd/MM/yyyy") + ". Thank you for using CocoSpices.");

                }
                _logger.LogInformation("Order {0} was added to the cart.", order.OrderId);
            }
            // Go back to the main store page for more shopping
            return Ok(_returnMessage);
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpGet]
        [Route("cart/removefromcart")]
        public async Task<IActionResult> RemoveFromCart(int id, string cartId, string productType, int productUnit)
        {
            // Retrieve the current user's shopping cart
            var cart = ShoppingCart.GetCart(DbContext, cartId);

            // Get the name of the product to display confirmation
            var cartItem = await DbContext.CartItems
                .Where(item => item.ProductId == id && item.CartId == cartId )
                .Include(c => c.Product)
                .SingleOrDefaultAsync();

            int itemCount;
            if (cartItem != null)
            {
                // Remove from cart
                itemCount = cart.RemoveFromCart(cartItem.CartItemId, productType, productUnit);
                await DbContext.SaveChangesAsync();
                string removed = (itemCount > 0) ? " 1 copy of " : string.Empty;
                _returnMessage.Status = "0";
                _returnMessage.Message = removed + cartItem.Product.Title + " has been removed from your shopping cart.";
            }
            else
            {
                itemCount = 0;
                _returnMessage.Message = "Could not find this item, nothing has been removed from your shopping cart.";
            }
            _logger.LogInformation("Product {id} was removed from a cart.", id);
            return Json(_returnMessage);
        }
    }
}