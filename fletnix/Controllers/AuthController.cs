using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using fletnix.Models.Auth;
using fletnix.ViewModels;
using IdentityModel.Client;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace fletnix.Controllers
{
    public class AuthController : Controller
    {
        private IConfigurationRoot _config;
        private IHostingEnvironment _env;

        //private SignInManager<ApplicationUser> _signInManager;

        public AuthController(IConfigurationRoot config, IHostingEnvironment env)
        {
            _config = config;
            _env = env;
            //_signInManager = signInManager;
        }

        public ActionResult Index()
        {
            return View();
        }


        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                Console.WriteLine("FFEEEEEESSTT!!!!!");
                RedirectToAction("Index","Movie");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //var signInResult = await _signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, true, false);

                /*var disco = await DiscoveryClient.GetAsync("http://localhost:5002");
                var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync("fletnix");
                if (tokenResponse.IsError)
                {
                    ModelState.AddModelError("", "Username or password incorrect");
                    Console.WriteLine(tokenResponse.Error);
                }
                else
                {
                    var client = new HttpClient();
                    client.SetBearerToken(tokenResponse.AccessToken);

                    var response = await client.GetAsync("http://localhost:5001/identity");
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(JArray.Parse(content));
                        return RedirectToAction("Index", "Movie");
                    }
                }*/

                /*if (signInResult.Succeeded)
                {
                   // if (string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);

                    return RedirectToAction("Index", "Movie");

                } else
                {
                    ModelState.AddModelError("","Username or password incorrect");
                }*/
            }


            return View();
        }


        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                //var id = await _auth.CreateLogoutContextAsync();

                //await HttpContext.Authentication.SignOutAsync("oidc");
                //Redirect(_config["AuthServer"]+"/connect/endsession?post_logout_redirect_uri=http://localhost:5000");

                //await HttpContext.Authentication.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);

                await HttpContext.Authentication.SignOutAsync("cookie");
                var redirectUri = _config["AuthServerRedirect"];
                return Redirect(_config["AuthServer"]+"/connect/endsession?logoutId=1337&postLogoutRedirectUri="+redirectUri);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}