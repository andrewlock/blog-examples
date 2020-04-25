using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PreventingOverPosting
{
    public class AppUserService
    {
        private static readonly ConcurrentDictionary<int, AppUser> _users = new ConcurrentDictionary<int, AppUser>(new Dictionary<int, AppUser>
        {
            {1, new AppUser { Name = "Andrew", IsAdmin = true } },
            {2, new AppUser { Name = "David", IsAdmin = false } },
            {3, new AppUser { Name = "Luke", IsAdmin = false} },
        });

        public AppUser Get(int id)
        {
            if (_users.TryGetValue(id, out var user))
            {
                return user;
            }
            return null;
        }

        public void Upsert(int id, AppUser user)
        {
            _users.AddOrUpdate(id, _ => user, (_, __) => user);
        }
    }
}
