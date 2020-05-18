using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using jwtproject.web.Models;
using jwtproject.web.ApiService;
using jwtproject.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

namespace jwtproject.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly TestApiService _testApiService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, TestApiService testApiService)
        {
            _logger = logger;
            _testApiService = testApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View(new UserViewModel {Email="enisyavas@gmail.com",Password="12345" });
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserViewModel model)
        {
            Token token = await _testApiService.SignIn(model);
            if (token != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, token.User.Name),
                    new Claim(ClaimTypes.Surname, token.User.Surname),
                    new Claim(ClaimTypes.Role, "member"),
                    new Claim("apitoken", token.AccessToken)
                };

                var userIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                userIdentity.AddClaims(claims);


                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal, new AuthenticationProperties{IsPersistent=true});


                return RedirectToAction("TestPage");
            }
            else
            {
                ModelState.AddModelError("", "Hatalı kullanıcı adı ya da şifre");
                return View(model);
            }
           
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("SignIn");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles ="member")]
        public async Task<IActionResult> TestPage()
        {
            var returnData = await _testApiService.Test();

            var b = JsonConvert.DeserializeObject<List<string>>(returnData);

            return View(b);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
