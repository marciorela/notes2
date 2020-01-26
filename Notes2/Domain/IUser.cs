using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Notes2.Domain
{
    public interface IUser
    {
        string Name { get; }
        int Id { get; }

        bool IsAuthenticated();

        IEnumerable<Claim> GetClaimsIdentity();
        string GetCookie(string key);
        void SetCookie(string key, string value, int? expireTime);
    }
}
