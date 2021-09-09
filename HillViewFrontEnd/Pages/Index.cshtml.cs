using HillViewFrontEnd.Helpers;
using HillViewFrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HillViewFrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public User UserLoggin { get; set; }
        [BindProperty]
        public string Message { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public IHelper _helper { get; set; }
        public bool IsLoggedIn { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IHelper helper)
        {
            _logger = logger;
            _helper = helper;
        }

        public void OnGet()
        {
            Message = "";
            IsLoggedIn = _helper.CheckUserIsLoggedIn();
        }

        public void OnPostRegister()
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("firstName", FirstName);
            body.Add("lastName", LastName);
            body.Add("email", Email);
            body.Add("username", Username);
            body.Add("password", Password);
            IRestResponse response = _helper.SendToApi("http://localhost:4000/users/register", "POST", false, body);
            if (response.IsSuccessful)
            {
                string jsonString = response.Content;
                Message message = JsonConvert.DeserializeObject<Message>(jsonString);

                Message = message.message;

                IsLoggedIn = _helper.CheckUserIsLoggedIn();
            }
            else
            {
                string jsonString = response.Content;
                Message message = JsonConvert.DeserializeObject<Message>(jsonString);

                Message = message.message;
            }
        }
        
        public void OnPostLogin()
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("username", Username);
            body.Add("password", Password);
            
            IRestResponse response = _helper.SendToApi("http://localhost:4000/users/authenticate", "POST", false, body);
            if (response.IsSuccessful)
            {
                string stringJWT = response.Content;
                JWT jwt = JsonConvert.DeserializeObject<JWT>(stringJWT);

                HttpContext.Session.SetString("token", jwt.JwtToken);

                Message = "User logged in successfully!";

                IsLoggedIn = _helper.CheckUserIsLoggedIn();
            }
            else
            {
                Message = "Something wrong";
            }
            
        }

        public JsonResult OnPostLoginFacebook(String accessToken)
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("AccessToken", accessToken);

            IRestResponse response = _helper.SendToApi("http://localhost:4000/users/authenticateFacebook", "POST", false, body);
            if (response.IsSuccessful)
            {
                string stringJWT = response.Content;
                JWT jwt = JsonConvert.DeserializeObject<JWT>(stringJWT);

                HttpContext.Session.SetString("token", jwt.JwtToken);

                Message = "User logged in successfully!";

                IsLoggedIn = _helper.CheckUserIsLoggedIn();
            }
            else
            {
                Message = "Something wrong";
            }

            return new JsonResult(accessToken);
        }

        public JsonResult OnPostLoginGoogle(String accessToken)
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("AccessToken", accessToken);

            IRestResponse response = _helper.SendToApi("http://localhost:4000/users/authenticateGoogle", "POST", false, body);
            if (response.IsSuccessful)
            {
                string stringJWT = response.Content;
                JWT jwt = JsonConvert.DeserializeObject<JWT>(stringJWT);

                HttpContext.Session.SetString("token", jwt.JwtToken);

                Message = "User logged in successfully!";

                IsLoggedIn = _helper.CheckUserIsLoggedIn();
            }
            else
            {
                Message = "Something wrong";
            }

            return new JsonResult(accessToken);
        }

        public void OnPostLogout()
        {
            HttpContext.Session.Remove("token");
            Message = "User logged out successfully!";
            //return new RedirectToPageResult("Index");
        }

        public void OnPostShowData()
        {
            string baseUrl = "http://localhost:4000";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",HttpContext.Session.GetString("token"));

            HttpResponseMessage response = client.GetAsync("/users").Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<User> data = JsonConvert.DeserializeObject<List<User>>(stringData);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Message = "Unauthorized!";
            }
            else
            {
                string strTable = "<table border='1' cellpadding='10'>";
                foreach (User emp in data)
                {
                    strTable += "<tr>";
                    strTable += "<td>";
                    strTable += emp.ID;
                    strTable += "</td>";
                    strTable += "<td>";
                    strTable += emp.FirstName;
                    strTable += "</td>";
                    strTable += "<td>";
                    strTable += emp.LastName;
                    strTable += "</td>";
                    strTable += "</tr>";

                }
                strTable += "</table>";

                Message = strTable;
            }

            //return new RedirectToPageResult("Index");
        }
    }
}
