using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HillViewFrontEnd.Helpers;
using HillViewFrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RestSharp;

namespace HillViewFrontEnd.Pages
{
    public class UserModel : PageModel
    {
        public List<User> users { get; set; }
        public String message { get; set; }
        public IHelper _helper { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserModel(IHttpContextAccessor httpContextAccessor, IHelper helper)
        {
            _httpContextAccessor = httpContextAccessor;
            _helper = helper;
        }
        public void OnGet()
        {
            message = "";
            IRestResponse response = _helper.SendToApi("http://localhost:4000/users", "GET", true, null);
            if (response.IsSuccessful)
            {
                users = JsonConvert.DeserializeObject<List<User>>(response.Content);
            }
            else
            {
                message = "Something wrong";
            }

            //Console.WriteLine(response.Content);
        }
    }
}
