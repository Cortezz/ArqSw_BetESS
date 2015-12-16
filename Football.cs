using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports
{
    public class Football : Sport
    {
        
        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public Football () : base() { }
        /// <summary>
        /// Param constructor.
        /// </summary>
        /// <param name="name">Name of the sport.</param>
        public Football (string name) : base(name) { }
        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="f">Object to be copied from.</param>
        public Football (Football f) : base(f) { }


        /// <summary>
        /// Adds a NormalEvent (1/X/2) related to Football. 
        /// </summary>
        /// <param name="id">Id of the event.</param>
        /// <param name="e">Instance of an event.</param>
        public override void AddEvent(int id, Event e)
        {
            NormalEvent ne = (NormalEvent)e;
            events.Add(id, ne);
        }






        /**Equals, ToString, Clone **/
        public override Sport Clone()
        {
            return new Football(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            if (obj == null || this.GetType() != obj.GetType()) return true;
            return base.Equals(obj);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Sport: Football\n");
            sb.Append(base.ToString());
            return sb.ToString();

        }



    }
}
