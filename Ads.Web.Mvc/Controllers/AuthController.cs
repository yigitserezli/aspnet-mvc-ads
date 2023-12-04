using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Ads.Web.Mvc.Models;
using App.Business;
using AutoMapper;
using App.Data;
using App.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ads.Web.Mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AppDbContext DbContext { get; }


        public AuthController(AppDbContext dbContext,
            IUserService userService,
            IAuthService authService,
            IMapper mapper
            )
        {
            DbContext = dbContext;
            _userService = userService;
            _authService = authService;
            _mapper = mapper;
        }



        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var createResult = await _userService.CreateUser(model.Username,model.Password,model.Name,model.Surname,model.Address);

            if (!createResult.IsSuccess)
            {
                ViewBag.ErrorMessage = createResult.ErrorMessage;
                return View();
            }

            
            TempData["UserRegistered"] = true;
            return RedirectToAction(nameof(Login));


            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Message = TempData["UserRegistered"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var loginDto = _mapper.Map<LoginDto>(model);

            var authResult = await _authService.AuthenticateAsync(loginDto);

            if (!authResult.IsSuccess)
            {
                return StatusCode(authResult.StatusCode, authResult.ErrorMessage);
            }

            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}

