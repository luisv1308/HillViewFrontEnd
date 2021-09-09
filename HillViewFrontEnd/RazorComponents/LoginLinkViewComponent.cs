using HillViewFrontEnd.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HillViewFrontEnd.RazorComponents.LoginLinkViewComponent
{
    public class LoginLinkViewComponent : ViewComponent
    {
        public bool IsLoggedIn { get; set; }
        public IHelper _helpers { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginLinkViewComponent(IHttpContextAccessor httpContextAccessor, IHelper helper)
        {
            _httpContextAccessor = httpContextAccessor;
            _helpers = helper;
        }

        public  IViewComponentResult Invoke()
        {
            IsLoggedIn = _helpers.CheckUserIsLoggedIn();

            return View("Default", IsLoggedIn);
        }
    }
}
