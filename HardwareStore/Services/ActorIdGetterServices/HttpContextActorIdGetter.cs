using System.Security.Claims;

namespace HardwareStore.Services.ActorIdGetterServices
{
    
    public class HttpContextActorIdGetter:IActorIdGetter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextActorIdGetter(IHttpContextAccessor httpContextAccessor) { 
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetActorId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
