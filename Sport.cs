using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sports
{
    public abstract class Sport
    {
        private string name;
        protected internal SortedDictionary<int, Event> events;



        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public Sport ()
        {
            this.name = "";
            events = new SortedDictionary<int, Event>();
        }

        /// <summary>
        /// Param Constructor.
        /// </summary>
        /// <param name="name">Name of the sport.</param>
        public Sport (string name)
        {
            this.name = name;
            events = new SortedDictionary<int, Event>();
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        /// <param name="s">Sport to be copied from.</param>
        public Sport (Sport s)
        {
            this.name = s.getName();
            this.events = s.getEvents();
        }


        /*Getters and Setters */
        public string getName () { return this.name; }
        public SortedDictionary<int,Event> getEvents ()
        {
            SortedDictionary<int, Event> newDictionary = new SortedDictionary<int, Event>();
            foreach (KeyValuePair<int,Event> de in events)
                newDictionary.Add(de.Key, de.Value);

            return newDictionary;
        }

        public void setName (string name) { this.name = name; }
        public void setEvents (SortedDictionary<int,Event> ev)
        {
            SortedDictionary<int, Event> aux = new SortedDictionary<int, Event>();
            foreach (KeyValuePair<int, Event> kvp in ev)
                aux.Add(kvp.Key, kvp.Value);
            this.events = aux;
        }



        /** Equals, ToString, Clone **/
        public abstract Sport Clone();
        

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            if (obj == null || this.GetType() != obj.GetType()) return false;
            Sport s = (Sport)obj;
            return (name.Equals(s.getName()) && EventsEquals(s.getEvents()));
        }

        public bool EventsEquals(SortedDictionary<int,Event> ev )
        {
            if (ev.Count != events.Count) return false;
            foreach (KeyValuePair<int, Event> kvp in ev)
                if (!events.ContainsKey(kvp.Key))
                    return false;
                else if (!events[kvp.Key].Equals(kvp.Value))
                    return false;
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name: ").Append(this.name).Append("\n");
            sb.Append("Events: ").Append(events.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// Adds an event into its list. 
        /// This method is represented as an abstract one because each Sport Subclass may be associated with a different kind of event.
        /// This means it is the role of those specific sports to insert whatever events they're associated with.
        /// </summary>
        /// <param name="id">Id of the event.</param>
        /// <param name="e">Event ot be added.</param>
        public abstract void AddEvent(int id, Event e);

        /// <summary>
        /// Removes an event.
        /// </summary>
        /// <param name="id">ID of the event.</param>
        public void RemoveEvent (int id)
        {
            events.Remove(id);
        }

    }
}
