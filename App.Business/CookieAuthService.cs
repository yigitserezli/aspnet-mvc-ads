using App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.Business
{

    public interface IAuthService
    {
        Task<ServiceResult> AuthenticateAsync(LoginDto model);
    }

    public class CookieAuthService :  IAuthService
    {
        public CookieAuthService(ILogger<CookieAuthService> logger,
          IHashService hashService,
          IUserService userService,
          IHttpContextAccessor httpContextAccessor
          )
        {
            Logger = logger;
            HashService = hashService;
            UserService = userService;
            HttpContextAccessor = httpContextAccessor;
        }


        public ILogger<CookieAuthService> Logger { get; }
        public IHashService HashService { get; }
        public IUserService UserService { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }


        public async Task<ServiceResult> AuthenticateAsync(LoginDto model)
        {
            try
            {
                var userResult = await UserService.GetUserByUsernameAsync(model.Username);
                if (!userResult.IsSuccess)
                {
                    return userResult;
                }
                var hashingResult = HashService.HashString(model.Password);
                if (!hashingResult.IsSuccess)
                {
                    return hashingResult;
                }

                var user = userResult.Data;

                if (user is null || user.PasswordHash != hashingResult.Data)
                {
                    return ServiceResult.Fail("Username or password is incorrect", StatusCodes.Status401Unauthorized);
                }
                var RoleList = await UserService.GetUserRolesFromUserId(user.Id);
                List<Claim> claims =new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("c-time", DateTime.UtcNow.ToString()),
                  
                };
                foreach (var item in RoleList.Data)
                { 
                claims.Add(new Claim(ClaimTypes.Role,item));
                }


                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                };

                var httpContext = HttpContextAccessor.HttpContext;
                if (httpContext is null)
                {
                    return ServiceResult.Fail("HttpContext is null", StatusCodes.Status424FailedDependency);
                }

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                return ServiceResult.Success();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error authenticating user");
                return ServiceResult.Fail("Error authenticating user", StatusCodes.Status500InternalServerError);
            }
        }

    }



}
