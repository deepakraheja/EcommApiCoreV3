using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace EcommApiCoreV3.Services
{
    public class UserService
    {
       

        private IHttpContextAccessor _httpContextAccessor;
        private HttpContext _httpcontext { get { return _httpContextAccessor.HttpContext; } }

        public int LoggedInUser { get { return CurrentUser(); } }
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private int CurrentUser()
        {
            string userId = "0";

            if (_httpcontext != null)
            {
                if (_httpcontext.User != null)
                {
                    var identity = _httpcontext.User.Identity;  // Figure out the user's identity

                    if (identity != null && identity.IsAuthenticated)
                    {
                        userId = _httpcontext.User.Claims.First(c => c.Type == "UserID").Value;

                    }
                }
            }
            return Convert.ToInt32(userId);
        }
    }
}
