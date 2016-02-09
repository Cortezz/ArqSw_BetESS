using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sports;

namespace BetESS
{
    public class Bookie : User, Observer
    {
        private List<int> subscribedEvents;


        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Bookie () : base()
        {
            subscribedEvents = new List<int>();
        }


        /// <summary>
        /// Param constructor.
        /// </summary>
        /// <param name="name">Name of the bookie.</param>
        /// <param name="email">E-mail of the bookie.</param>
        /// <param name="pwd">Password of the bookie.</param>
        //Param Constructor
        public Bookie (String name, String email, String pwd) : base(name, email, pwd)
        {
            subscribedEvents = new List<int>();
        }

        
        /// <sumary>
        /// Copy Constructor.
        /// </sumary>    
        /// <param name="b">Bookie to be copied.</param>
        public Bookie (Bookie b) : base((User)b)
        {
            subscribedEvents = b.getSubscribedEvents();
        }


        //Getters
        public List<int> getSubscribedEvents ()
        {
            List<int> ev = new List<int>();
            foreach (int e in subscribedEvents)
                ev.Add(e);
            return ev;
        }


        /// <summary>
        /// Subscribes a bookie to a certain event
        /// </summary>
        /// <param name="e">Event which the bookie will subscribe to.</param>
        public void SubscribeTo (int e)
        {
            subscribedEvents.Add(e);
        }
        
        /// <summary>
        /// Removes an event from the bookie's subcribed events list.
        /// </summary>
        /// <param name="eventID">Event to be removed.</param>
        public void UnsubscribeTo(int eventID)
        {
            subscribedEvents.Remove(eventID);
        }
        
        
        
        //Observer methods
        /// <summary>
        /// Update method from the Observer Interface.
        /// Pushes a notification into the bookie's notification list.
        /// </summary>
        /// <param name="s">Notification to be pushed.</param>
        public void Update(String s)
        {
            this.PushNotification(s);
        }


        /**Equals, ToString and Clone**/
        public override User Clone ()
        {
            return new Bookie(this);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Events on which this bookie is interested: ").Append("\n");
            foreach (int e in subscribedEvents)
                sb.Append("Subscribed to the following events:\n").Append(e.ToString()).Append("\n");
            return sb.ToString();
        }
        public override bool Equals(Object obj)
        {
            if (obj == this) return true;
            if (obj == null || obj.GetType() != this.GetType()) return false;
            Bookie b = (Bookie)obj;
            return (EventsEquals(b.getSubscribedEvents()) && base.Equals(b));
        }

        public bool EventsEquals (List<int> events)
        {
            int i;
            if (subscribedEvents.Count != events.Count) return false;
            for (i = 0; i < events.Count; i++)
                if (events[i] != subscribedEvents[i]) return false;
            return true;
        }
    }
}
