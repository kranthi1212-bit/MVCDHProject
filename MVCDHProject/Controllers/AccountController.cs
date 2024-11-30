using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using MVCDHProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NuGet.Protocol;
using Microsoft.Build.Framework;


namespace MVCDHProject.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser identityUser = new IdentityUser
                {
                    UserName = userModel.Name,
                    Email = userModel.Email,
                    PhoneNumber = userModel.Mobile
                };
                var result = await userManager.CreateAsync(identityUser, userModel.Password);
                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(identityUser, false);
                    //return RedirectToAction("Index", "Home");
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    var confirmationUrlLink = Url.Action("confirmEmail", "Account", new { Userid = identityUser.Id, Token = token }, Request.Scheme);
                    SendMail(identityUser, confirmationUrlLink, "Email Confirmation link");
                    TempData["Title"] = "Email Confirmation link";
                    TempData["Message"] = "A confirm email link has been sent to your registered mail, click on it to confirm";
                    return View("DisplayMessages");
                }
                else
                {
                    foreach (var Error in result.Errors)
                    {
                        ModelState.AddModelError("", Error.Description);
                    }
                }
            }
            return View(userModel);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await userManager.FindByNameAsync(loginModel.Name);
            if (user != null && (await userManager.CheckPasswordAsync(user, loginModel.Password)) && user.EmailConfirmed == false)
            {
                ModelState.AddModelError("", "Your email is not confirmed");
                return View(loginModel);
            }
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginModel.Name, loginModel.Password, loginModel.RememberMe, false);
                if (result.Succeeded)
                {
                    if (String.IsNullOrEmpty(loginModel.ReturnUrl))
                        return RedirectToAction("Index", "Home");
                    else
                        return LocalRedirect(loginModel.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Creadentials");
                }
            }
            return View(loginModel);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public void SendMail(IdentityUser identityUser, string requestlink, string subject)
        {
            StringBuilder mailbody = new StringBuilder();
            mailbody.Append("Hello " + identityUser.UserName + "<br/> <br/>");
            if (subject == "Email Confirmation link")
            {
                mailbody.Append("Click on the link below to confirm your email:");
            }
            else if (subject == "Change Password link")
            {
                mailbody.Append("Click on the link below to reset your password:");
            }
            mailbody.Append("<br/>");
            mailbody.Append(requestlink);
            mailbody.Append("<br/><br/>");
            mailbody.Append("Regards");
            mailbody.Append("<br/><br/>");
            mailbody.Append("Customer Support");

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailbody.ToString();
            MailboxAddress fromAddress = new MailboxAddress("Customer Support", "kannajikirankumar@gmail.com");
            MailboxAddress toAddress = new MailboxAddress(identityUser.UserName, identityUser.Email);
            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(fromAddress);
            mailMessage.To.Add(toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = bodyBuilder.ToMessageBody();

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 465, true);
            smtpClient.Authenticate("kannajikirankumar@gmail.com", "bgrm esae mqkc folv");
            smtpClient.Send(mailMessage);

        }
        public async Task<ActionResult> ConfirmEmail(string userid, string token)
        {
            if (userid != null && token != null)
            {
                var User = await userManager.FindByIdAsync(userid);
                if (User != null)
                {
                    var result = await userManager.ConfirmEmailAsync(User, token);
                    if (result.Succeeded)
                    {
                        TempData["Title"] = "Email Confirmation Success.";
                        TempData["Message"] = "Email confirmation is completed. You can now login into the application.";
                        return View("DisplayMessages");
                    }
                    else
                    {
                        StringBuilder Errors = new StringBuilder();
                        foreach (var Error in result.Errors)
                        {
                            Errors.Append(Error.Description + ". ");
                        }
                        TempData["Title"] = "Confirmation Email Failure";
                        TempData["Message"] = Errors.ToString();
                        return View("DisplayMessages");
                    }
                }
                else
                {
                    TempData["Title"] = "Invalid User Id.";
                    TempData["Message"] = "User Id which is present in confirm email link is in-valid.";
                    return View("DisplayMessages");
                }
            }
            else
            {
                TempData["Title"] = "Invalid Email Confirmation Link.";
                TempData["Message"] = "Email confirmation link is invalid, either missing the User Id or Confirmation Token.";
                return View("DisplayMessages");
            }
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await userManager.FindByNameAsync(model.Name);
                if (User != null && await userManager.IsEmailConfirmedAsync(User))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(User);
                    var confirmationUrlLink = Url.Action("ChangePassword", "Account", new { Userid = User.Id, Token = token }, Request.Scheme);
                    SendMail(User, confirmationUrlLink, "Change Password link");
                    TempData["Title"] = "Change Password Link";
                    TempData["Message"] = "Change password link has been sent to your mail, click on it and change password.";
                    return View("DisplayMessages");
                }
                else
                {
                    TempData["Title"] = "Change Password Mail Generation Failed.";
                    TempData["Message"] = "Either the Username you have entered is in-valid or your email is not confirmed.";
                    return View("DisplayMessages");
                }
            }
            return View(model);
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await userManager.FindByIdAsync(model.UserId);
                if (User != null)
                {
                    var result= await userManager.ResetPasswordAsync(User,model.Token,model.Password);
                    if(result.Succeeded)
                    {
                        TempData["Title"] = "Reset Password Success";
                        TempData["Message"] = "Your password has been reset successfully.";
                        return View("DisplayMessages");
                    }
                    else
                    {
                        foreach(var Error in result.Errors)
                        {
                            ModelState.AddModelError("",Error.Description);
                        }
                    }
                }
                else
                {
                    TempData["Title"] = "Invalid User";
                    TempData["Message"] = "No user exists with the given User Id.";
                    return View("DisplayMessages");
                }
            }
            return View(model);
        }
    }
}
