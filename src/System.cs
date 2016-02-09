using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sports;


namespace BetESS
{
    /// <summary>
    /// Class that acts as the facade.
    /// </summary>
    public class System
    {
        private SortedDictionary<string, User> users;
        private SortedDictionary<string, Sport> sports;
        private Dictionary<int, Bet> openBets;
        private Dictionary<int, Bet> closedBets;


        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public System()
        {
            this.users = new SortedDictionary<string, User>();
            this.sports = new SortedDictionary<string, Sport>();
            this.openBets = new Dictionary<int, Bet>();
            this.closedBets = new Dictionary<int, Bet>();
        }


                                                           /** --------- USER ------------ **/


        /// <summary>
        /// Checks whether a certain e-mail is already in the system or not.
        /// </summary>
        /// <param name="email">E-mail of the user.</param>
        /// <returns>True if the e-mail's not in the system, false otherwise.</returns>
        public bool ValidateEmail(string email)
        {
            return !users.ContainsKey(email);
        }

        /// <summary>
        /// Returns a string with all users in it.
        /// </summary>
        /// <returns>String which represents the users list.</returns>
        public string ListOfUsers()
        {
            StringBuilder sb = new StringBuilder("List of Users\n");
            foreach (User u in users.Values)
                sb.Append(u.ToString());
            return sb.ToString();

        }

        /// <summary>
        /// Checks if some user's credentials are correct.
        /// </summary>
        /// <param name="email">User's e-mail.</param>
        /// <param name="pwd">User's password.</param>
        /// <returns>True if his e-mail->password match is correct, false otherwise.</returns>
        public bool CheckCredentials(string email, string pwd)
        {
            User u = users[email];
            return (email.Equals(u.getEmail()) && pwd.Equals(u.getPassword()));
        }

                                            /** -------------------- NOTIFICATIONS ------------- **/
        
        /// <summary>
        /// Returns the amount of notifications of a certain user, given his e-mail.
        /// </summary>
        /// <param name="email">User's e-mail.</param>
        /// <returns>Number of notifications a user has.</returns>      
        public int AmountOfNotificationsFrom (string email)
        {
            return users[email].AmountOfNotification();
        }

        /// <summary>
        /// Gets all the notifications from a certain user, specified by his e-mail.
        /// </summary>
        /// <param name="email">User's e-mail.</param>
        /// <returns>String which represents the notifications.</returns>
        public string NotificationListFrom (string email)
        {
            return users[email].NotificationList();
        }

        /// <summary>
        /// Removes the notifications from a certain user, specified by his e-mail.
        /// </summary>
        /// <param name="email">E-mail of the user.</param>
        public void RemoveNotificationsFrom (string email)
        {
            users[email].RemoveNotifications();
        }


                                            /** ------------------- PUNTERS --------------------- **/


        /// <summary>
        /// Adds a punter to the system.
        /// </summary>
        /// <param name="name">Punter's name.</param>
        /// <param name="email">Punter's e-mail.</param>
        /// <param name="pwd">Punter's password.</param>
        public void AddPunter(string name, string email, string pwd)
        {
            Punter p = new Punter(name, email, pwd, 0);
            users.Add(email, p);
        }
        

        /// <summary>
        /// Returns the bet history in string representation from a punter, given his e-mail.
        /// </summary>
        /// <param name="email">E-mail of the punter.</param>
        /// <returns>String representation of the bets made by a certain punter.</returns>
        public string BetHistoryFrom (string email)
        {
            Dictionary<string, Dictionary<int,int>> bets = ((Punter)users[email]).BetHistory();
            StringBuilder sb = new StringBuilder();

            //Open Bets
            Dictionary<int, int> oBets = bets["Open"];
            sb.Append("\t\tOpen Bets \n");
            foreach (int openBetID in oBets.Keys)
                sb.Append(openBets[openBetID].ToString()).Append("\n----\n");
            //Closed Bets
            Dictionary<int, int> cBets = bets["Closed"];
            sb.Append("\t\tClosed Bets\n");
            foreach (int closedBetID in cBets.Keys)
                sb.Append(closedBets[closedBetID].ToString()).Append("\n----\n");

            return sb.ToString();
        }

        /// <summary>
        /// Returns all bets from a specific punter, given his e-mail.
        /// </summary>
        /// <param name="email">E-mail of the punter.</param>
        /// <returns>A dictionary whose key is the bet ID and value is the bet itself. These bets are all the bets a certain punter 
        /// has made.</returns>
        public Dictionary<int,Bet> GetBetsFrom (string email)
        {
            Dictionary<int, Bet> bets = new Dictionary<int, Bet>();

            Dictionary<int, int> oBets = ((Punter)users[email]).getOpenBets();
            Dictionary<int,int> cBets = ((Punter)users[email]).getClosedBets();

            foreach (int key in oBets.Keys)
                bets.Add(key, openBets[key]);
            foreach (int key in cBets.Keys)
                bets.Add(key, closedBets[key]);

            return bets;

        }

        /// <summary>
        /// Returns how many coins a punter has, given his e-mail.
        /// </summary>
        /// <param name="email">E-mail of the punter.</param>
        /// <returns>Amount of coins the specified punter has.</returns>
        public float GetBetESSCoinsFrom (string email)
        {
            return ((Punter)users[email]).getBetESSCoins();
        }

        /// <summary>
        /// Adds a bet to a certain punter, given his e-mail.
        /// </summary>
        /// <param name="betID">ID of the bet.</param>
        /// <param name="email">Email of the punter.</param>
        public void AddOpenBetTo (int betID, string email)
        {
            ((Punter)users[email]).AddOpenBet(betID);
        }

        /// <summary>
        /// Debits coins from a certian punter, given his e-mail.
        /// </summary>
        /// <param name="email">Email of the punter whose coins are to be debited.</param>
        /// <param name="coins">Coins to be debited.</param>
        public void DebitCoinsFrom (string email, float coins)
        {
            ((Punter)users[email]).DebitCoins(coins);
        }

        /// <summary>
        /// Credits coins from a certain punter, given his e-mail.
        /// </summary>
        /// <param name="email">Email of the punter whose coins are to be credited.</param>
        /// <param name="coins">Coins to be credited.</param>
        public void CreditCoinsTo (string email, float coins)
        {
            ((Punter)users[email]).CreditCoins(coins);
        }

                                /** ------------------------ BOOKIES ---------------------------------- **/

        /// <summary>
        /// Adds a bookie to the system.
        /// </summary>
        /// <param name="name">Name of the bookie.</param>
        /// <param name="email">Email of the bookie.</param>
        /// <param name="pwd">Password of the bookie.</param>
        public void AddBookie(string name, string email, string pwd)
        {
            Bookie b = new Bookie(name, email, pwd);
            users.Add(email, b);
        }

        /// <summary>
        /// Adds a bookie to a certain event.
        /// </summary>
        /// <param name="e">Event.</param>
        /// <param name="bookieEmail">Email of a bookie.</param>
        public void SubscribeBookieToEvent (Event e, string bookieEmail)
        {
            e.AddSubscribedBookie(bookieEmail);
            Bookie b = ((Bookie)users[bookieEmail]);
            e.Subscribe(b);
        } 
        
        /// <summary>
        /// Adds a subscrived event to a bookie.
        /// </summary>
        /// <param name="eventID">ID of the event to be subscribed.</param>
        /// <param name="email">Email of the bookie.</param>
        public void AddSubscribedEventTo (int eventID, string email)
        {
            ((Bookie)users[email]).SubscribeTo(eventID);
            
        }
        /// <summary>
        /// Returns all subscribed event IDs from a certain bookie, given his e-mail.
        /// </summary>
        /// <param name="bookieEmail">E-mail of the bookie.</param>
        /// <returns>List with all event IDs.</returns>
        public List<int> GetSubscribedEventsFrom (string bookieEmail)
        {
            return ((Bookie)users[bookieEmail]).getSubscribedEvents();
        }


                                /*** ------------------------ Admin ------------------------------- ***/

        /// <summary>
        /// Adds an admin into the user's list.
        /// </summary>
        /// <param name="name">Name of the admin.</param>
        /// <param name="email">Email of the admin.</param>
        /// <param name="pwd">Password of the admin.</param>
        public void AddAdmin (string name, string email, string pwd)
        {
            Admin a = new Admin(name, email, pwd);
            users.Add(email,a);
        }



                                /***  ----------------------- Sports ------------------------------ **/

        

        /// <summary>
        /// Adds a sport into the system.
        /// </summary>
        /// <param name="name">Name of the sport.</param>
        /// <param name="s">Sport to be added.</param>
        public void AddSport(string name, Sport s)
        {
            sports.Add(name, s);
        }



                            /*** ----------------------------- EVENTS ------------------------------------- **/


        /// <summary>
        /// Adds an event to the system.
        /// </summary>
        /// <param name="name">Name of the sport.</param>
        /// <param name="id">Id of the event.</param>
        /// <param name="e">The event's instance (object-wise).</param>
        /// <param name="bookieEmail">E-mail of the bookie who created the event.</param>
        public void AddEvent(string name, int id, Event e, string bookieEmail)
        {
            sports[name].AddEvent(id, e);
            
        }
        

        /// <summary>
        /// The events are mapped according to the sport they represent. This method returns a single collection with all events in it.
        /// </summary>
        /// <returns>A sorted dictionary with the following structure: Key - SportName, Value - Dictionary(EventID, Event).</returns>
        public SortedDictionary<string, Dictionary<int, Event>> GetAllEvents()
        {
            SortedDictionary<string, Dictionary<int, Event>> events = new SortedDictionary<string, Dictionary<int, Event>>();
            foreach (KeyValuePair<string, Sport> sp in sports)
            {
                Dictionary<int, Event> ev = new Dictionary<int, Event>();
                events.Add(sp.Key, ev);
                SortedDictionary<int, Event> aux = sp.Value.getEvents();
                foreach (KeyValuePair<int, Event> kvp in aux)
                    events[sp.Key].Add(kvp.Value.getEventID(), kvp.Value);
            }
            return events;
        }

        /// <summary>
        /// Given the collection assembled by the method @getAllEvents, prints a list of every event.
        /// </summary>
        /// <param name="ev">The result of @getAllEvents.</param>
        /// <returns>String which represents a list with every event.</returns>
        public string DisplayEvents(SortedDictionary<string, Dictionary<int, Event>> ev)
        {
            StringBuilder sb = new StringBuilder("List of events\n\n");
            foreach (KeyValuePair<string, Dictionary<int, Event>> kvp in ev)
            {
                sb.Append("\t\t").Append(kvp.Key).Append("\n");
                foreach (Event e in kvp.Value.Values)
                {
                    sb.Append(e.ToString());
                    sb.Append("---\n");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns the odds of all outcomes from a certain event.
        /// </summary>
        /// <param name="e">Event from which the odds are to be gotten.</param>
        /// <returns>A List of tuples(representation of the outcome(e.g. 1, X, 2, etc), and its odd).</returns>
        public List<Tuple<string, double>> DisplayOddsFrom(Event e)
        {
            return e.DisplayOdds();
        }

        /// <summary>
        /// Changes the odds of an event.
        /// </summary>
        /// <param name="e">Events whose odds are to be changed.</param>
        /// <param name="l">New odds.</param>
        public void ChangeOddsTo(Event e, List<double> l)
        {
            //Change the odds of the event itself
            e.ChangeOdds(l);
        }

        /// <summary>
        /// Returns a map with every existing event, mapped according to its ID.
        /// </summary>
        /// <param name="sports">All events divided into sports.</param>
        /// <returns>A Dictionary whose key is the event ID and the value the event itself.</returns>
        public Dictionary<int, Event> MapOfAllEvents(SortedDictionary<string, Dictionary<int, Event>> sports)
        {

            Dictionary<int, Event> events = new Dictionary<int, Event>();
            foreach (KeyValuePair<string, Dictionary<int, Event>> kvp in sports)
            {
                Dictionary<int, Event> aux = kvp.Value;
                foreach (KeyValuePair<int, Event> ev in aux)
                    events.Add(ev.Key, ev.Value);
            }
            return events;
        }

        /// <summary>
        /// Returns a map with every existing event, mapped according to its ID.
        /// </summary>
        /// <returns>A Dictionary whose key is the event ID and the value the event itself.</returns>
        public Dictionary<int, Event> MapOfAllEvents ()
        {
            SortedDictionary<string, Dictionary<int, Event>> sports = GetAllEvents();
            Dictionary<int, Event> events = new Dictionary<int, Event>();
            foreach (KeyValuePair<string, Dictionary<int, Event>> kvp in sports)
            {
                Dictionary<int, Event> aux = kvp.Value;
                foreach (KeyValuePair<int, Event> ev in aux)
                    events.Add(ev.Key, ev.Value);
            }
            return events;

        }



        /// <summary>
        /// Auxiliar method called by @CloseBet in order to remove an event from the system.
        /// </summary>
        /// <param name="e">Event to be removed.</param>
        private void RemoveEvent(Event e)
        {
            foreach (KeyValuePair<string, Sport> sport in sports)
            {
                SortedDictionary<int, Event> evtSports = sport.Value.getEvents();
                if (evtSports.ContainsKey(e.getEventID()))
                    sport.Value.RemoveEvent(e.getEventID());
            }
        }


        /** ------------------- BETS ----------------- **/
        /// <summary>
        /// Adds a bet: 
        /// 1 - Into the punter's open bet list.
        /// 2 - Into the event's bet list.
        /// </summary>
        /// <param name="e">Event related to the bet.</param>
        /// <param name="betID">ID of the bet.</param>
        /// <param name="better">E-mail of the better.</param>
        /// <param name="option">Option chosen by the punter regarding this bet.</param>
        /// <param name="odds">Odds for that particular choice.</param>
        /// <param name="coins">Number of coins bet.</param>
        public void AddBet(Event e, int betID, string better, int option, double odds, float coins)
        {
            Bet b = new Bet(betID, e.getDescription(), better, option, odds, coins);
            openBets.Add(betID, b);
            e.AddBet(betID);
        }

        /// <summary>
        /// Closes the bets of a specified event, notifies the punters which bet on that particular event and credit/debits into their account
        /// based on the outcome.
        /// </summary>
        /// <param name="e">Event to be closed.</param>
        /// <param name="outcome">Outcome of the event.</param>
        public void CloseEvent(Event e, int outcome)
        {
            StringBuilder sb;
            Bet b;
            float losses, gains;
            float value;
            losses = gains = 0;

            List<int> punterBets = e.getBets();
            
            
            foreach (int i in punterBets)
            {
                sb = new StringBuilder();
                b = openBets[i];
                Punter u = (Punter)users[b.getBetter()];
                /* Set the Closed and Won Boolean values */
                if (b.getOption() == outcome)
                {
                    b.CloseBet(true);
                    value = b.getCoins() * (float)b.getOdd();
                    u.CreditCoins(value);
                    gains += value;
                }
                else
                {
                    b.CloseBet(false);
                    losses += b.getCoins();
                }
                //PUNTER: Changes bet from open to closed
                u.CloseOpenBet(i);
                //System: Changes bet from open to closed
                openBets.Remove(i);
                closedBets.Add(i, b);
                //Notifications
                sb.Append(b.ToString());
                u.PushNotification(sb.ToString());


            }
            e.setGains(gains);
            e.setLosses(losses);
            e.CloseEvent(outcome);
            RemoveEvent(e);
            
        }

        





    }
}
