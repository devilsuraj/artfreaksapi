using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using artfriks.Models;
using artfriks.Models.AccountViewModels;
using artfriks.Services;
using Microsoft.AspNetCore.Http;
using artfriks.Data;

namespace artfriks.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext db,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _db = db;
            _smsSender = smsSender;
            _context = context;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            _emailSender.SendEmailAsync("shaheen@aeonsoftware.net", "Hi", "Hello");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }



        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FullName = model.FullName,
                    Address = model.Address,
                    Phone = model.Phone,
                    Profession = model.Profession,
                    CountryCode = model.CountryCode,
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, "Polardevil#1");
                if (result.Succeeded)
                {
                    UserModel useme = new UserModel();
                    useme.UserId = user.Id;
                    _db.UserModel.Add(useme);
                    _db.SaveChanges();
                    var OTP = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.Phone);
                    //  await _emailSender.SendEmailAsync(model.Email, "Artfreaks Confirm your account",
                    //    $"Please confirm your account by Using this verification code " + OTP + " .");
                    ////await _smsSender.SendSmsAsync(model.Phone, "Your OTP for CocoSpices is " + OTP + ".");
                    //await _smsSender.SendSmsAsync(model.Phone, "Your OTP for Artfreaksglobal is " + OTP + ". Thank you for using Artfreaksglobal!");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return View();
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpPost]
        [Route("api/account/verifyOtp")]
        public async Task<IActionResult> VerifyPhoneCode(string username, string code)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return Ok(new { status = 0, Message = "User Does Not Exists" });
            }
            if (await _userManager.VerifyChangePhoneNumberTokenAsync(user, code, username))
            {
                await _userManager.ChangePhoneNumberAsync(user, username, code);
                return Ok(new { status = 1, Message = "Success" });
            }
            else
            {
                return Ok(new { status = 2, Message = "Wrong Vrification Code" });
            }
        }

        [Route("api/account/ResetToOriginalPassword")]
        [HttpPost]
        public async Task<IActionResult> ResetToPassword(ChangePasswordViewModel model)
        {

            var user = await _userManager.FindByNameAsync(model.username);

            if (user == null)
            {
                return Ok(new { status = 0, Message = "User Does Not Exists" });
            }

            var code = await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, model.password);
            if (result.Succeeded)
                return Ok(new { status = 1, Message = "Success" });
            else
                return Ok(new { status = 0, Message = "Failed" });


        }
        // POST: /Account/ResetPassword
        [Route("api/account/resetPassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {

            var user = await _userManager.FindByNameAsync(model.username);

            if (user == null)
            {
                return Ok(new { status = 0, Message = "User Does Not Exists" });
            }
            if (await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.code, user.Phone))
            {
                await _userManager.ChangePhoneNumberAsync(user, user.Phone, model.code);
                var code = await _userManager.RemovePasswordAsync(user);
                var result = await _userManager.AddPasswordAsync(user, model.password);
                if (result.Succeeded)
                    return Ok(new { status = 1, Message = "Success" });
                else
                    return Ok(new { status = 0, Message = "Failed" });
            }
            else
            {
                return Ok(new { status = 2, Message = "Wrong Vrification Code" });
            }

        }

        [Route("api/account/updateprofile")]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileModel model)
        {
            var user = await _userManager.FindByNameAsync(model.username);
            if (user == null)
            {
                return Ok(new { status = 0, Message = "User Does Not Exists" });
            }
            if (model.password != null && model.oldpassword != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.oldpassword, model.password);
                if (result.Succeeded && model.Email != null)
                {
                    user.FullName = model.fullame;
                    await _userManager.UpdateAsync(user);


                    var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //  await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                    //     $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                    return Ok(new { status = 1, Message = "Success" });
                }
                else if (result.Succeeded)
                    return Ok(new { status = 1, Message = "Updated Successfully" });
                else
                    return Ok(new { status = 0, Message = "Wrong Password" });
            }
            else
            {
                user.FullName = model.fullame;
                await _userManager.UpdateAsync(user);
                if (model.Email != null)
                {
                    var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    // await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                    //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                }
                return Ok(new { status = 1, Message = "Updated Successfully" });
            }
        }
        [Route("api/account/forgotpassword")]
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> forgotPassword(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                var OTP = await _userManager.GenerateChangePhoneNumberTokenAsync(user, username);
                var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");
                //await _smsSender.SendSmsAsync(username, "Your OTP for Artfreaksglobal is " + OTP + ". Thank you for using Artfreaksglobal!");
                return Ok(new { status = 1, Message = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogDebug(3, ex.Message);
                return BadRequest(new { status = 0, Message = "failed" });
            }
        }

        //
        // custom register
        [Route("api/account/register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterHttp([FromBody]RegisterViewModel dto)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { status = 2, errors = ModelState.Values });
            }
            var checkuser = await _userManager.FindByEmailAsync(dto.Email);
            //if (checkuser.AccessFailedCount == 0)
            //{
            //if (checkuser != null && await _userManager.IsPhoneNumberConfirmedAsync(checkuser) == false)
            //{
            //    var OTP = await _userManager.GenerateChangePhoneNumberTokenAsync(checkuser, checkuser.Phone);
            //    await _smsSender.SendSmsAsync(dto.CountryCode + dto.Phone, "Your OTP for Artfreaksglobal is " + OTP + ". Thank you for using Artfreaksglobal!");
            //    return Ok(new { status = 99, error = "Verify your mobile number" });
            //}
            if (checkuser != null)// && await _userManager.IsPhoneNumberConfirmedAsync(checkuser) == true)
            {
                return Ok(new { status = 44, error = "User already exists" });
            }
            //}

            try
            {
                var user = new ApplicationUser
                {
                    UserName = dto.Email,
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Address = dto.Address,
                    FormattedAddress = dto.FormattedAddress,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    PinCode = dto.PinCode,
                    Country = dto.Country,
                    State = dto.State,
                    City = dto.City,
                    Profession = dto.Profession,
                    Phone = dto.Phone,
                    CountryCode = dto.CountryCode,
                    PhoneNumber = dto.Phone,
                    Gender = dto.Gender,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                };
                user.SecurityStamp = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(user, "Polardevil#1");

                /*  var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                  var OTP = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.Phone);
                  var callbackUrl = $"https://bo.Artfriksglobal.com/Account/ConfirmEmail?userId={ user.Id}&code={code}";
                  */
                //await _smsSender.SendSmsAsync(dto.CountryCode + dto.Phone, "Your OTP for Artfreaksglobal is " + OTP + ". Thank you for using Artfreaksglobal!");

                // await _signInManager.SignInAsync(user, isPersistent: false);
                //  _logger.LogInformation(3, "User created a new account with password.");


                if (result.Succeeded)
                {
                    UserModel useme = new UserModel();
                    useme.UserId = user.Id;
                    _db.UserModel.Add(useme);
                    _db.SaveChanges();
                    //  await _emailSender.SendEmailAsync(dto.Email, "Artfreaks - Confirm your account",
                    //  $"Your verification code for Artfreaksglobal is " + OTP + ". Thank you for using Artfreaksglobal!");
                    return Ok(new { status = 0, message = "Success", user = user });
                    /*
                    var result2 = await _userManager.AddToRoleAsync(user, "Admin");
                    if (result2.Succeeded)
                    {
                        return Ok(new { status = 0, message = "Success" });
                    }
                    else
                    {
                        return Ok(new { status = 1, result2.Errors });
                    }*/
                }
                else
                {
                    return Ok(new { status = 1, result.Errors });
                }

            }
            catch (Exception ex)
            {
                _logger.LogDebug(5, ex.Message);
                return Ok(new { status = 5, errors = new { description = ex.Message } });

            }
        }

        [Route("api/account/updateUser")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> updateUser([FromBody]RegisterViewModel username)
        {
            var check = await _userManager.FindByNameAsync(username.UserName);
            if (check != null)// && await _userManager.IsPhoneNumberConfirmedAsync(check) == true)
            {
                return Ok(new { status = 5, Message = "Please choose another UserName" });
            }
            var aa = username.Email;

            try
            {
                var user = await _userManager.FindByEmailAsync(username.Email);
                /* if (await _userManager.VerifyChangePhoneNumberTokenAsync(user, username.OTP, username.Phone) == false)
                 {
                     return Ok(new { status = 4, Message = "Wrong verification code " });
                 }
                 else
                 {
                     user.PhoneNumberConfirmed = true;
                 }*/
                user.PhoneNumberConfirmed = true;
                user.FullName = username.FullName;
                user.Address = username.Address;
                user.UserName = username.UserName;
                var res = await _userManager.ChangePasswordAsync(user, "Polardevil#1", username.Password);
                // await _userManager.RemovePasswordAsync(user);
                //await _userManager.AddPasswordAsync(user, username.Password);


                string Emailtext = String.Format(" <div class='container emailer'> <div class='row'> <h5>Hello " + user.UserName + "-" + user.Email + ",</h5> <p>Welcome to <strong>artfreaksglobal.com. </strong> We are simply thrilled to see you here! </p> <p>Art Freaks is a <strong>Non Commission </strong> based online Art Gallery where - <strong>Artists can showcase and sell their works online to buyers directly; & Art Collectors and Gallery Owners can contact the Artists directly. </strong> </p> <h5>If you are an Artist you can... </h5> <p> - Upload images of your artworks to create your own art gallery and have Art Collectors contact you directly in your own private inbox! <a href='http://alpha.artfreaksglobal.com' class='red-text' style='font-family:Century751 BT; font-style:italic;'> upload now </a> </p> <p>- Not in mood to deal with buyer directly? No issues! Let us handle the transaction for you for a small fee/percentage. <a href='#' class='red-text' style='font-family:Century751 BT; font-style:italic;'>write to us here</a> </p> <p>- Browse and connect with other artists from around the world for free flowing creative exchang. </p> <h5>If you are an Art Collector you can... </h5> <p>- Browse through our extensive online collection of original Artworks, by Name or by Genre, and connect to the Artists directly! <a href='http://alpha.artfreaksglobal.com' class='red-text' style='font-family:Century751 BT; font-style:italic;'> browse now </a> </p> <p>- Not sure what you looking for... We have customised search module, 'Find Your Art' section, which finds art according to your specific requirement <a href='http://alpha.artfreaksglobal.com' class='red-text' style='font-family:Century751 BT; font-style:italic;'>try it... </a> </p> <p>Still not satisfied? Then connect with our Art Specialists Panel, who will personally advise and curate the artworks for that Special Wall! A free, personalised initiative by Art Freaks India. <a href='http://alpha.artfreaksglobal.com' class='red-text' style='font-family:Century751 BT; font-style:italic;'>write to us...</a> </p> <p>We are truly glad that you are a part of this Global Art Community! </p> <br /> <p>Cheers!!</p> <p>Art Freaks India Team </p> </div> <div class='row'> <div class='col s6 m6 l6'><img src='http://base.kmtrt.in/images/newlogo.png' width='580' height='65' alt='' class='responsive-img'/> </div> <div class='col s6 m6 l6 right-align' > <a href='#'><img src='http://base.kmtrt.in/images/fb.png' width='30' height='30' alt=''/> </a> <a href='#'> <img src='http://base.kmtrt.in/images/tw.png' width='30' height='30' alt=''/> </a> <a href='#'> <img src='http://base.kmtrt.in/images/insta.png' width='30' height='30' alt=''/> </a> <a href='#'> <img src='http://base.kmtrt.in/images/pin.png' width='30' height='30' alt=''/> </a> </div> </div> </div> </body>");
                await _emailSender.SendEmailAsync(user.Email, "Artfreaks Global - Thank you for registration", Emailtext);
                if (res.Succeeded)
                {
                    await _userManager.UpdateAsync(user);

                    return Ok(new { status = 0, Message = "success" });
                }
                else
                {
                    return Ok(new { status = 1, Message = "Use of space and special character is not allowed" });
                }

            }
            catch (Exception ex)
            {
                var user = await _userManager.FindByEmailAsync(username.Email);
                _logger.LogDebug(3, ex.Message);
                _db.Users.Remove(user);
                return BadRequest(new { status = 1, Message = ex.Message });
            }
        }


        [Route("api/account/sendOtp")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> sendOtp(string email, string countrycode, string username)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var OTP = await _userManager.GenerateChangePhoneNumberTokenAsync(user, username);
                //await _smsSender.SendSmsAsync(countrycode + username, "Your OTP for Artfreaksglobal is " + OTP + ". Thank you for using Artfreaksglobal!");
                return Ok(new { status = 0, Message = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogDebug(3, ex.Message);
                return BadRequest(new { status = 1, Message = "failed" });
            }
        }

        [Route("api/account/sendOtpbyuser")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> sendOtpbyuser(string username)
        {
            try
            {

                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return Ok(new { status = 1, Message = "User does not exist" });
                }
                var OTP = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.Phone);
                //await _smsSender.SendSmsAsync(user.CountryCode + user.Phone, "Your OTP for Artfreaksglobal is " + OTP + ". Thank you for using Artfreaksglobal!");
                //  await _emailSender.SendEmailAsync(user.Email, "Artfreaks verification code", "Your verification code for Artfreaksglobal is " + OTP + ". Thank you for using Artfreaksglobal!");
                return Ok(new { status = 0, Message = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogDebug(3, ex.Message);
                return BadRequest(new { status = 1, Message = ex.Message });
            }
        }




        //custom external access use anything solid rather than 123456
        [Route("api/account/externalAccess")]
        [AllowAnonymous]
        [HttpGet]
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Forgot()
        {
            return View();
        }

        [Route("api/account/Forgot")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Forgot(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return Ok(new { status = 3, Message = "User does not exist" });
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(Email, "Reset Password",
               $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

            return Ok(new { status = 1, Message = "success" });
        }
        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Redirect("http://alpha.artfreaksglobal.com/v2/#/account/login");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Redirect("http://alpha.artfreaksglobal.com/v2/#/account/login");
            }
            AddErrors(result);
            return View();

        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }

            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                //  await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
            }
            else if (model.SelectedProvider == "Phone")
            {
                //await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
            }

            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View(model);
            }
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
