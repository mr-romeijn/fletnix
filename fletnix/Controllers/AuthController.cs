using System;
using System.Net.Http;
using System.Threading.Tasks;
using fletnix.Models.Auth;
using fletnix.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace fletnix.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;

        public AuthController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index","Movie");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, true, false);

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

                if (signInResult.Succeeded)
                {
                   // if (string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);

                    return RedirectToAction("Index", "Movie");

                } else
                {
                    ModelState.AddModelError("","Username or password incorrect");
                }
            }


            return View();
        }

        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
               await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}