using Microsoft.AspNetCore.Mvc;
using NEWS_App.Models.IRepository;
using NEWS_App.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.Scripting;
using System.Security.Claims;
using NEWS_App.Models.IRepositoryImpl;
using Microsoft.AspNetCore.Http;

namespace NEWS_App.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        public UserController(IUserRepository userRepository, INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(User user)
        {
            user.CreatedAt = DateTime.Now;

            var user1 = await _userRepository.GetByUsernameAsync(user.Email);

                  if (user1 != null)
                 {
                        ModelState.AddModelError("Email", "Email already exists.");

                         return View(user);
                 }

                await _userRepository.AddAsync(user);
                return RedirectToAction("Login");
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string PasswordHash)
        {
            if (await _userRepository.ValidateUser(email, PasswordHash))
            {
               var user1 = await _userRepository.GetByUsernameAsync(email);

                var NotificationCount= _notificationRepository.CountByUserId(user1.Id);

                HttpContext.Session.SetInt32("UserId", user1.Id);
                HttpContext.Session.SetString("Username", email);
                HttpContext.Session.SetInt32("Notification", NotificationCount);
                //  HttpContext.Session.SetString("IsAuthenticated", "true");

                var returnUrl = HttpContext.Session.GetString("ReturnUrl");
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    HttpContext.Session.Remove("ReturnUrl");
                    return Redirect(returnUrl);
                }
                return RedirectToAction("index", "Articles");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Articles");
        }
    }
}
