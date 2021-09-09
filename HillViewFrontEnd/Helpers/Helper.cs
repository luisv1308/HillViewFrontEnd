using Microsoft.AspNetCore.Http;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HillViewFrontEnd.Helpers
{
    public class Helper : IHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Helper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool CheckUserIsLoggedIn()
        {
            if (_httpContextAccessor.HttpContext.Session.Keys.Contains("token"))
            {
                return true;
            }
            else
            {
                return  false;
            }
        }

        public IRestResponse SendToApi(string url, string method, bool session, Dictionary<string, string>? parameters)
        {
            var client = new RestClient(url);
            RestSharp.RestRequest request = null;
            client.Timeout = -1;
            
            if(method == "GET")
            {
                request = new RestRequest(Method.GET);
            } else if (method == "POST") {
                request = new RestRequest(Method.POST);
            }

            request.AddHeader("Content-Type", "application/json");
            if (parameters != null)
            {
                request.AddJsonBody(parameters);
                //request.AddParameter("application/json", parameters, ParameterType.RequestBody);
            }
            if (session)
            {
                request.AddHeader("Authorization", "Bearer " + _httpContextAccessor.HttpContext.Session.GetString("token"));
            }
            request.AddHeader("ApiKey", "c859900b-c682-4fbb-8f1b-58d9da7e3e24");
            request.AddHeader("Accept-Language", "es");
            String[] host = _httpContextAccessor.HttpContext.Request.Host.Value.Split(':');
            request.AddHeader("HostName", host[0]);
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
