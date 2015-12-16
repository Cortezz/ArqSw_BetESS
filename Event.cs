using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetESS;

namespace Sports
{ 
    public enum ObservableEvents { OddChange, EndOfEvent };

    public abstract class Event : Observable
    {
        private string description;
        private int eventID;
        private int outcome;
        private List<int> bets;
        private List<string> subscribedBookies;
        private List<Observer> observers;
        private string bookie;
        private float gains;
        private float losses;


        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public Event()
        {
            this.description = bookie = "";
            this.gains = this.losses = 0;
            this.eventID = 0;
            this.outcome = -1;
            this.bets = new List<int>();
            this.subscribedBookies = new List<string>();
            this.observers = new List<Observer>();
        }

        /// <summary>
        /// Param constructor. The outcome is -1 by default, which means the event is open.
        /// </summary>
        /// <param name="id">Id of the event.</param>
        /// <param name="description">Event description.</param>
        /// <param name="bookie">Name of the bookie which created the event.</param>
        public Event(int id,string description,string bookie)
        {
            this.description = description;
            this.gains = this.losses = 0;
            this.eventID = id;
            this.bookie = bookie;
            this.outcome = -1;
            this.bets = new List<int>();
            this.subscribedBookies = new List<string>();
            this.observers = new List<Observer>();
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        /// <param name="e">Event to be copied.</param>
        public Event (Event e)
        {
            this.description = e.getDescription();
            this.eventID = e.getEventID();
            this.outcome = e.getOutcome();
            this.bets = e.getBets();
            this.bookie = e.getBookie();
            this.subscribedBookies = e.getSubscribedBookies();
            this.observers = e.getObservers();
            this.gains = e.getGains();
            this.losses = e.getLosses();
        }


        /**Getters and Setters**/
        public string getDescription () { return this.description; }
        public int getEventID () { return this.eventID; }
        public int getOutcome() { return this.outcome; }
        public string getBookie () { return this.bookie; }
        public float getGains() { return this.gains; }
        public float getLosses() { return this.losses; }
        public List<int> getBets ()
        {
            List<int> retList = new List<int>();
            foreach (int i in bets)
                retList.Add(i);
            return retList;
        }
        public List<string> getSubscribedBookies()
        {
            List<string> l = new List<string>();
            foreach (string s in subscribedBookies)
                l.Add(s);
            return l;
        }
        public List<Observer> getObservers()
        {
            List<Observer> l = new List<Observer>();
            foreach (Observer ob in observers)
                l.Add(ob);
            return l;
        }


        public void setBookie (string b) { this.bookie = b; }
        public void setDescription (string desc) { this.description = desc; }
        public void setEventID (int id) { this.eventID = id; }
        public void setGains (float gains) { this.gains = gains; }
        public void setLosses (float losses) { this.losses = losses; }


       
        /// <summary>
        /// Closes an event by changing its outcome (from -1 to some other value).
        /// </summary>
        /// <param name="outcome">Outcome of the event.</param>
        public void CloseEvent (int outcome)
        {
            this.outcome = outcome;
            NotifyObservers(ObservableEvents.EndOfEvent);
        }

        /// <summary>
        /// Subscribes a bookie to this instance's event.
        /// </summary>
        /// <param name="subBookieEmail">Email of the bookie.</param>
        public void AddSubscribedBookie (string subBookieEmail)
        {
            subscribedBookies.Add(subBookieEmail);
        }

        /// <summary>
        /// Adds a bet to this event.
        /// </summary>
        /// <param name="bID">ID of the bet.</param>
        public void AddBet (int bID)
        {
            bets.Add(bID);
        }



        //Observable Methods

        /// <summary>
        /// Adds an observer to the observers list.
        /// </summary>
        /// <param name="o">Observer to be added.</param>
        public void Subscribe (Observer o)
        {
            observers.Add(o);
        }
        
        /// <summary>
        /// Removes an observer from the observers list
        /// </summary>
        /// <param name="o">Observer to be removed.</param>
        public void Unsubscribe (Observer o)
        {
            observers.Remove(o);
        }

        /// <summary>
        /// Notifies either the end of change observers or the odd change observers, depending on the Enum value passed as argument.
        /// </summary>
        /// <param name="obsEvs">Enumeration related to the event which triggered the notification.
        /// Can be OddChange or EndOfEvent</param>
        public void NotifyObservers (ObservableEvents obsEvs)
        {
            StringBuilder sb = new StringBuilder();
            switch (obsEvs)
            {
                case ObservableEvents.EndOfEvent:
                    sb.Append("An event you've subscribed to has ended!\n").Append(this.ToString());
                    sb.Append("Gains: ").Append(this.gains).Append(" coins.\n");
                    sb.Append("Losses: ").Append(this.losses).Append(" coins.\n");
                    break;
                case ObservableEvents.OddChange:
                    sb.Append("The odds of an event you've subscribed to have changed!\n").Append(this.ToString());
                    break;
                default:
                    Console.WriteLine("THE FUCK IS GOING ON!");
                    break;
            }
            foreach (Observer o in observers)
                o.Update(sb.ToString());
            
        }
        

        /** Clone, Equals, ToString and HashCode */

        public abstract Event Clone();




        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            if (obj == null || obj.GetType() != this.GetType()) return false;
            Event e = (Event)obj;
            return (description.Equals(e.getDescription()) && this.eventID == e.getEventID() && this.outcome == e.getOutcome() 
                && EqualBets(e.getBets()) && gains==e.getGains() && losses==e.getLosses()
                && this.bookie.Equals(e.getBookie()) && EqualsSubscribedBookies(e.getSubscribedBookies()));
        }
        //Auxiliar method to check if two lists of bets are the same.
        public bool EqualBets (List<int> l)
        {
            int size, i;
            if (bets.Count != l.Count) return false;
            size = l.Count;
            for (i = 0; i < size; i++)
                if (!l[i].Equals(bets[i])) return false;
            return true;
        }

        //Auxiliar method to check if two lists of subscribed bookies are the same.
        public bool EqualsSubscribedBookies (List<string> l)
        {
            int size, i;
            if (subscribedBookies.Count != l.Count) return false;
            size = l.Count;
            for (i = 0; i < size; i++)
                if (!l[i].Equals(subscribedBookies[i])) return false;
            return true;
        }
        



        public override string ToString ()
        {
            StringBuilder s = new StringBuilder();
            s.Append("Event ID: ").Append(this.eventID).Append("\n");
            s.Append("Description: ").Append(this.description).Append("\n");
            if (outcome != -1) s.Append("Outcome: ").Append(outcome).Append("\n");
            return s.ToString();
        }

        public override int GetHashCode()
        {
            return eventID;
        }

        /// <summary>
        /// Method which represents every available outcome and its odd into a SortedDictionary.
        /// </summary>
        /// <returns>Sorted Dictionary whose key is the outcome and the value is the odd.</returns>
        public abstract List<Tuple<string, double>> DisplayOdds();

        /// <summary>
        /// Method which gives an odd to a certain outcome.
        /// </summary>
        /// <param name="opt">Outcome chosen.</param>
        /// <returns>Odd of a certain specific outcome.</returns>
        public abstract double GetSpecificOdd(int opt);

        /// <summary>
        /// Changes the odds of an event.
        /// Receives a list of doubles so it can be used by all sorts of events (subclasses, that is). 
        /// Despite the fact there's only NormalEvent right now, this is a measure to achieve good levels of extensibility.
        /// </summary>
        /// <param name="list">List of doubles which represents the new odds.</param>
        public abstract void ChangeOdds(List<double> list);
    }
    
}
