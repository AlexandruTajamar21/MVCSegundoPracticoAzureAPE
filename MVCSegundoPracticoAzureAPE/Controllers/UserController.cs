using APISEgundoPracticoAzureAPE.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVCSegundoPracticoAzureAPE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVCSegundoPracticoAzureAPE.Controllers
{
    public class UserController : Controller
    {

        private ServiceCliente service;

        public UserController(ServiceCliente service)
        {
            this.service = service;
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string UserName, string Password)
        {
            string token =
                await this.service.GetTokenAsync(UserName, Password);
            if (token == null)
            {
                ViewData["MENSAJE"] = "Nombre/Apellido incorrectos";
                return View();
            }
            else
            {
                //SI EL USUARIO EXISTE, ALMACENAMOS EL TOKEN EN SESSION
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim("TOKEN", token));

                identity.AddClaim(new Claim("userName", UserName));
                identity.AddClaim(new Claim("password", Password));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , principal, new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(Usuario user)
        {
            if(await this.service.CreateUser(user.Nombre, user.Apellidos, user.Email, user.UserName, user.Password))
            {
                string token =
                await this.service.GetTokenAsync(user.UserName, user.Password);
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim("TOKEN", token));

                identity.AddClaim(new Claim("userName", user.UserName));
                identity.AddClaim(new Claim("password", user.Password));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , principal, new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
