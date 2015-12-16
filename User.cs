using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetESS
{
    public abstract class User
    {
        private String name;
        private String email;
        private String pwd;
        private List<string> notifications;



        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public User()
        {
            this.name = "";
            this.email = "";
            this.pwd = "";
            this.notifications = new List<string>();
        }

        /// <summary>
        /// Param Constructor.
        /// </summary>
        /// <param name="name">Name of the user.</param>
        /// <param name="email">Email of the user.</param>
        /// <param name="pwd">Password of the user.</param>
        public User(String name, String email, String pwd)
        {
            this.name = name;
            this.email = email;
            this.pwd = pwd;
            this.notifications = new List<string>();

        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        /// <param name="p">user to be copied from.</param>
        public User (User p)
        {
            this.name = p.getName();
            this.email = p.getEmail();
            this.pwd = p.getPassword();
            this.notifications = p.getNotifications();
        }


        /*Gettters and Setters*/
        public String getName() { return this.name; }
        public String getEmail() { return this.email; }
        public String getPassword() { return this.pwd; }
        public List<string> getNotifications()
        {
            List<string> not = new List<string>();
            foreach (string s in notifications)
                not.Add(s);
            return not;
        }

        public void setName(String name) { this.name = name; }
        public void setEmail(String email) { this.email = email; }
        public void setPassword(String password) { this.pwd = password; }


                                        /** -------------- NOTIFICATIONS -------------------- **/
        /// <summary>
        /// Adds a notification into the notification list.
        /// <param name="s">Notification to be added.</param>
        /// </summary>
        public void PushNotification(String s)
        {
            notifications.Add(s);
        }

        /// <summary>
        /// Returns the number of notifications from a user.
        /// </summary>
        /// <returns>Number of notifications a user has.</returns>
        public int AmountOfNotification()
        {
            return notifications.Count;
        }

        /// <summary>
        /// Removes all notifications from a user.
        /// </summary>
        public void RemoveNotifications()
        {
            notifications = new List<string>();
        }

        /// <summary>
        /// Returns all notifications in formatted string.
        /// </summary>
        /// <returns>A string which represents all notifications.</returns>
        public string NotificationList()
        {
            StringBuilder sb = new StringBuilder("\t\tList of notifications\n-----------------\n");
            foreach (string s in notifications)
                sb.Append(s).Append("-----------------\n");
            return sb.ToString();
        }


        /*Clone, toString and equals*/
        public abstract User Clone();

        public override String ToString ()
        {
            return "Name: " + this.name + "\nE-mail: " + this.email + "\nPassword: " + this.pwd;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || this.GetType() != obj.GetType()) return false;
            User p = (User)obj;
            return (this.name.Equals(p.getName()) && this.email.Equals(p.getEmail()) && this.pwd.Equals(p.getPassword()) 
                && NotificationsEquals(p.getNotifications()));
        }

        public bool NotificationsEquals(List<string> l)
        {
            int i;
            if (notifications.Count != l.Count) return false;
            for (i = 0; i < l.Count; i++)
                if (!notifications[i].Equals(l[i])) return false;
            return true;
        }

    }
}
