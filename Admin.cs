using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetESS
{
    public class Admin : User
    {
        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public Admin (): base() { }

        /// <summary>
        /// Param Constructor.
        /// </summary>
        /// <param name="name">Name of the admin.</param>
        /// <param name="email">Email of the admin.</param>
        /// <param name="password">Password of the admin.</param>
        public Admin (string name, string email, string password) : base(name,email,password) {}

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        /// <param name="a">Admin to be copied.</param>
        public Admin (Admin a) : base(a) { }


        /**ToString, Equals and Clone**/
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Admin:\n");
            sb.Append(base.ToString());
            return sb.ToString();
        }

        public override bool Equals(Object obj)
        {
            if (this == obj) return true;
            if (obj == null || this.GetType() != obj.GetType()) return false;
            Admin a = (Admin)obj;
            return (base.Equals((User)a));
        }

        public override User Clone()
        {
            return new Admin(this);
        }


    }
}
