/*
 * OPAS - Account Management Controller
 * Copyright (C) 2025 Ims2425Bruh
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using src.Models.Authentication;

namespace src.Controllers
{
    public class AccountController(AuthenticationService authService) : Controller
    {
        private readonly AuthenticationService _authService = authService;

        // register action
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _authService.RegisterUserAsync(model);
            if (response.IsSuccessful)
                return RedirectToAction("Login");
            
            ModelState.AddModelError(string.Empty, "Registration failed: " + response.Content);
            return View(model);
        }

        /// login action
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _authService.LoginUserAsync(model);

            if (response.IsSuccessful)
            {
                var responseContent = response.Content;

                if (string.IsNullOrEmpty(responseContent))
                {
                    ModelState.AddModelError(string.Empty, "Response content is null or empty.");
                    return View();
                }

                try
                {
                    if (string.IsNullOrEmpty(responseContent))
                    {
                        ModelState.AddModelError(string.Empty, "Response content is null or empty.");
                        return RedirectToAction("Login");
                    }
                    if (string.IsNullOrEmpty(responseContent))
                    {
                        ModelState.AddModelError(string.Empty, "Response content is null or empty.");
                        return RedirectToAction("Login");
                    }
                    var jsonObj = JObject.Parse(responseContent);

                    if (jsonObj["Token"] != null)
                    {
                        var token = jsonObj["Token"]?.ToString();

                        // store only the jwt token in the session
                        if (!string.IsNullOrEmpty(token))
                        {
                            HttpContext.Session.SetString("JWT", token);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Token is null or empty.");
                            return View();
                        }

                        // store username in session
                        HttpContext.Session.SetString("Username", model.UserName);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Token not found in the response.");
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error parsing response: " + ex.Message);
                    ModelState.AddModelError(string.Empty, "Error parsing response: " + ex.Message);
                    return View();
                }
            }

            ModelState.AddModelError(string.Empty, "Login failed: " + response.Content);
            return View();
        }

        // logout action
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWT");
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }

        // this endpoint will receive the sso token from another app,
        // validate it with the Auth Server,
        // and log in the user by storing the access token
        [HttpGet("validate-sso")]
        public async Task<IActionResult> ValidateSSO([FromQuery] string ssoToken)
        {
            if (string.IsNullOrEmpty(ssoToken))
                return RedirectToAction("Login");

            // validate sso token by calling the Auth Server
            var response = await _authService.ValidateSSOTokenAsync(ssoToken);
            if (response.IsSuccessful)
            {
                var responseContent = response.Content;

                try
                {
                    if (string.IsNullOrEmpty(responseContent))
                    {
                        System.Console.WriteLine("Response content is null or empty.");
                        return RedirectToAction("Login");
                    }

                    var jsonObj = JObject.Parse(responseContent);
                    var accessToken = jsonObj["Token"]?.ToString();
                    var username = jsonObj["UserDetails"]?["Username"]?.ToString();

                    if (!string.IsNullOrEmpty(accessToken))
                        // store only the jwt token in the session
                        HttpContext.Session.SetString("JWT", accessToken);
                    if (!string.IsNullOrEmpty(username))
                        // store username in session
                        HttpContext.Session.SetString("Username", username);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error parsing response: " + ex.Message);
                    return RedirectToAction("Login");
                }
            }

            return RedirectToAction("Login");
        }
    }
}