using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTypes
{
    [Serializable]
    public class AppUser
    {
        public int? UserID { get; set; }
        public string Username { get; set; }
        public bool IsADUser { get; set; }
        public string[] Roles { get; set; }

        public AppUser() { }
        public AppUser(int? userID, string username, bool isADUser, string[] roles)
        {
            this.UserID = userID;
            this.Username = username;
            this.IsADUser = isADUser;
            this.Roles = roles;
        }
    }
}
