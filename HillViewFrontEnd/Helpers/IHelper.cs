using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HillViewFrontEnd.Helpers
{
    public interface IHelper
    {
        bool CheckUserIsLoggedIn();

        IRestResponse SendToApi(string apiUrl, string method, bool session, Dictionary<string, string>? parameters);
    }
}
