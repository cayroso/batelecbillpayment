
using Data.App.Models.Documents;
using Data.App.Models.FileUploads;
using Data.App.Models.Teams;
using Data.App.Models.Users.UserTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Users
{
    public class User
    {
        public string UserId { get; set; }
        public string ImageId { get; set; }
        public virtual FileUpload Image { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FirstLastName => $"{FirstName} {LastName}";
        public string FirstMiddleLast
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(MiddleName))
                    return $"{FirstName} {MiddleName} {LastName}";
                else
                    return $"{FirstName} {LastName}";
            }
        }
        [NotMapped]
        public string Initials => $"{FirstName[0]}{LastName[0]}".ToUpper();
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
        public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();

        //public virtual ICollection<DocumentAccessHistory> DocumentAccessHistories { get; set; } = new List<DocumentAccessHistory>();
    }

    public static class UserExtension
    {

        public static void ThrowIfNull(this User me)
        {
            if (me == null)
                throw new ApplicationException("User not found.");
        }

        public static void ThrowIfNullOrAlreadyUpdated(this User me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("User already updated by another user.");

            me.ConcurrencyToken = newToken;
        }
    }
}
