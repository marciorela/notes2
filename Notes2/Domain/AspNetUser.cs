using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Notes2.Domain
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        //public int Id => Convert.ToInt32(ClaimsPrincipal.Current.FindFirst("Id").Value); //       ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes. FindFirst( "Id").Value);
        //public int Id => Convert.ToInt32(((ClaimsIdentity)_accessor.HttpContext.User.Identity).FindFirst("Id").Value);
        public int Id => int.Parse(_accessor.HttpContext.User.FindFirstValue("id"));
        //public int Id => Convert.ToInt32(GetClaimsIdentity().Where(c => c.Type == "Id").FirstOrDefault());

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        /// <summary>  
        /// Get the cookie  
        /// </summary>  
        /// <param name="key">Key </param>  
        /// <returns>string value</returns> 
        public string GetCookie(string key)
        {
            //return Request.Cookies[Key];
            return _accessor.HttpContext.Request.Cookies[key];
        }

        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            _accessor.HttpContext.Response.Cookies.Append(key, value, option);
        }

        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void RemoveCookie(string key)
        {
            _accessor.HttpContext.Response.Cookies.Delete(key);
        }
    }
}
