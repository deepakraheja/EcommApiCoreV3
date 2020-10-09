using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using EcommApiCoreV3.Services;

namespace EcommApiCoreV3.Controllers.Common
{
    public abstract class BaseController<T> : Controller where T : BaseController<T>
    {
        private ILogger<T> _logger;
        private UserService _userService;
        protected ILogger<T> Logger => _logger ?? (_logger = HttpContext?.RequestServices.GetService<ILogger<T>>());
        protected UserService UserService => _userService ?? (_userService = HttpContext?.RequestServices.GetService<UserService>());
    }
}
