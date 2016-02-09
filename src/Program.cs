using System;
using System.Collections.Generic;
using UI;
using Sports;
using Criteria;

namespace BetESS
{
    /// <summary>
    /// Main Application Class. 
    /// </summary>
    public class Program
    {
        private static int eventCounter, betCounter;
        private static System BetESS;
        private static Menu MenuLogin, UserMainMenu, AdminMenu, SportsMenu, BookieMenu,BetHistoryMenu;

        static void Main(string[] args)
        {
            LoadComponents();
            Initialize();

            do
            {
                MenuLogin.ExecuteMenu();
                switch (MenuLogin.getOption())
                {
                    case 1:
                        Register("User");
                        break;
                    case 2:
                        Login("User");
                        break;
                    case 3:
                        Register("Admin");
                        break;
                    case 4:
                        Login("Admin");
                        break;
                    case 5:
                        Register("Bookie");
                        break;
                    case 6:
                        Login("Bookie");
                        break;
                }
            } while (MenuLogin.getOption() != 0);
        }


        /** ------------ LOGIN MENU METHODS ---------------- **/
        /// <summary>
        /// Method which in which user registration takes place. If the e-mail is already on the system, it notifies so and the 
        /// registration won't take place.
        /// </summary>
        private static void Register(string type)
        {
            string name, pwd, email;
            Console.Write("Name: ");
            name = Console.ReadLine();
            Console.Write("E-mail: ");
            email = Console.ReadLine();
            Console.Write("Password: ");
            pwd = Console.ReadLine();

            if (BetESS.ValidateEmail(email))
            {
                switch (type)
                {
                    case "User":
                        BetESS.AddPunter(name, email, pwd);
                        break;
                    case "Bookie":
                        BetESS.AddBookie(name, email, pwd);
                        break;
                    case "Admin":
                        BetESS.AddAdmin(name, email, pwd);
                        break;
                }
                Console.WriteLine("You registration was successful!");
            }
            else Console.WriteLine("That E-mail already exists!");

        }

        /// <summary>
        /// Method which handles the login. It deals with two exceptions:
        /// i) Wrong key-value info (e-mail->password)
        /// ii) Non-existant e-mail
        /// </summary>
        private static void Login(string type)
        {
            string email, pwd;
            Console.Write("E-mail: ");
            email = Console.ReadLine();
            Console.Write("Password: ");
            pwd = Console.ReadLine();

            if (!BetESS.ValidateEmail(email))
                if (BetESS.CheckCredentials(email, pwd))
                {
                    switch (type)
                    {
                        case "User":
                            MainApp(email);
                            break;
                        case "Bookie":
                            BookieApp(email);
                            break;
                        case "Admin":
                            AdminApp();
                            break;
                    }
                    Console.WriteLine("Your login was successful!");
                }
                else Console.WriteLine("Either the e-mail or password are wrong.");
            else Console.WriteLine("That e-mail does not exist.");
        }

        /** -------------- MAIN USER MENU METHODS ------------------------ **/

        ///
        ///<summary>Exectues the Main User Menu. Available options are:
        /// 1 - List of Events
        /// 2 - History of Bets
        /// 3 - Place a Bet
        /// 4 - Available Coins
        /// 5 - Insert Coins
        /// </summary>
        private static void MainApp(string UserEmail)
        {
            do
            {

                if (BetESS.AmountOfNotificationsFrom(UserEmail) > 0)
                {
                    Console.WriteLine("You have new notifications!\n\n");
                    Console.WriteLine(BetESS.NotificationListFrom(UserEmail));
                    BetESS.RemoveNotificationsFrom(UserEmail);
                }
                UserMainMenu.ExecuteMenu();
                switch (UserMainMenu.getOption())
                {
                    case 1:
                        ListOfEvents();
                        break;
                    case 2:
                        HistoryOfBets(UserEmail);
                        break;
                    case 3:
                        PlaceBet(UserEmail);
                        break;
                    case 4:
                        AvailableCoins(UserEmail);
                        break;
                    case 5:
                        InsertCoins(UserEmail);
                        break;
                }
            } while (UserMainMenu.getOption() != 0);
        }


        /// <summary>
        /// Prints the list of an user's events.
        /// </summary>
        private static void ListOfEvents()
        {
            Console.WriteLine(BetESS.DisplayEvents(BetESS.GetAllEvents()));
        }


        /// <summary>
        /// Lets the user decide in which way he wants his bet history to be showed. The options are as follows:
        /// 1 - All bets
        /// 2 - Filter.
        /// </summary>
        private static void HistoryOfBets(string UserEmail)
        {
            
            do
            {
                BetHistoryMenu.ExecuteMenu();
                switch (BetHistoryMenu.getOption())
                {
                    case 1:
                        Console.WriteLine(BetESS.BetHistoryFrom(UserEmail));
                        break;
                    case 2:
                        FilterBetHistory(UserEmail);
                        break;
                }
            } while (BetHistoryMenu.getOption() != 0);
        }

        /// <summary>
        /// Browses through the bet history with the following filters:
        /// 1 - Open bets
        /// 2 - Closed bets
        /// 3 - More coins than a certain amount
        /// 4 - Less coins than a certain amount
        ///</summary>
        /// <param name="UserEmail"></param>
        private static void FilterBetHistory (string UserEmail)
        {
            int option;
            Dictionary<int,Bet> bets = BetESS.GetBetsFrom(UserEmail);
            Dictionary<int, Bet> result;
            Criteria.Criteria c, c1;
            Console.WriteLine("##############\n      EXPRESSION      \n##############");
            c = Expression();
            Console.WriteLine("##############\n      END OF EXPRESSION      \n##############");
            do
            {
                option = LogicalMenu();
                switch (option)
                {
                    case 1:
                        Console.WriteLine("##############\n      EXPRESSION      \n##############");
                        c1 = Expression();
                        c = new CriteriaAnd(c, c1);
                        Console.WriteLine("##############\n      END OF EXPRESSION      \n##############");
                        break;
                    case 2:
                        Console.WriteLine("##############\n      EXPRESSION      \n##############");
                        c1 = Expression();
                        c = new CriteriaOr(c, c1);
                        Console.WriteLine("##############\n      END OF EXPRESSION      \n##############");
                        break;
                    default:
                        break;
                }

            } while (option != 0);

            Console.WriteLine("###############");
            result = c.meetCriteria(bets);
            foreach (Bet b in result.Values)
                Console.WriteLine(b.ToString());
        }


        /// <summary>
        /// This method will handle the bets registration. 
        /// </summary>
        private static void PlaceBet(string UserEmail)
        {
            string sID, sCoins, sOption;
            int id, opt;
            float coins;
            double odd;

            SortedDictionary<string, Dictionary<int, Event>> sports = BetESS.GetAllEvents();
            Console.WriteLine(BetESS.DisplayEvents(sports));
            Dictionary<int, Event> events = BetESS.MapOfAllEvents(sports);


            //Event
            Console.Write("#############################\nWhich event do you want to bet on: ");
            sID = Console.ReadLine();
            int.TryParse(sID, out id);
            Event e = events[id];

            //Option
            Console.WriteLine("------------");
            Console.WriteLine(e.ToString() + "------------");
            Console.Write("What do you want to bet on (NOTE: Draw is represented by 0):");
            sOption = Console.ReadLine();
            int.TryParse(sOption, out opt);

            //Coins
            do
            {
                Console.Write("Amount of coins: ");
                sCoins = Console.ReadLine();
                float.TryParse(sCoins, out coins);
                if (coins > BetESS.GetBetESSCoinsFrom(UserEmail)) Console.WriteLine("Insufficient credit (" + BetESS.GetBetESSCoinsFrom(UserEmail) + ")");
            } while (coins > BetESS.GetBetESSCoinsFrom(UserEmail));


            //Adding bet to the system and user's history, respectively.
            odd = e.GetSpecificOdd(opt);

            BetESS.AddBet(e, betCounter, UserEmail, opt, odd, coins);
            BetESS.AddOpenBetTo(betCounter, UserEmail);
            betCounter++;
            BetESS.DebitCoinsFrom(UserEmail, coins);
            Console.WriteLine("The bet was successfully created!");
        }


        /// <summary>
        /// Shows how many coins a user has, given his e-mail.
        /// </summary>
        /// <param name="UserEmail">E-mail of the user.</param>
        private static void AvailableCoins(string UserEmail)
        {
            Console.WriteLine("The amount of coins in your account is: " + BetESS.GetBetESSCoinsFrom(UserEmail));
        }


        /// <summary>
        /// Inserts a specified amount of coins in the user's account.
        /// </summary>
        private static void InsertCoins(string UserEmail)
        {
            string s;
            float coins;

            Console.Write("How many coins do you wish to credit: ");
            s = Console.ReadLine();
            float.TryParse(s, out coins);
            BetESS.CreditCoinsTo(UserEmail, coins);
            Console.WriteLine("The amount of coins in your account is: " + BetESS.GetBetESSCoinsFrom(UserEmail));

        }



        /** ---------------- MAIN ADMIN MENU METHODS ----------------------------- **/

        ///
        ///<summary>Executes the main admin menu. Available options are:
        /// 1 - Determine Outcome of an Event
        /// </summary>
        private static void AdminApp()
        {
            do
            {
                AdminMenu.ExecuteMenu();
                switch (AdminMenu.getOption())
                {
                    case 1:
                        DetermineOutcome();
                        break;
                }
            } while (AdminMenu.getOption() != 0);
        }


       

        /// <summary>
        /// Finishes an event (by registering its outcome), closes every bet associated with it
       ///  and sends a notification to everyone who bet or subscribed to that particular event.
        /// </summary>
        private static void DetermineOutcome()
        {
            string sID, sOut;
            int outcome, eventID;

            SortedDictionary<string, Dictionary<int, Event>> sports = BetESS.GetAllEvents();
            Console.WriteLine(BetESS.DisplayEvents(sports));
            Dictionary<int, Event> events = BetESS.MapOfAllEvents(sports);


            //Event
            Console.Write("Which event do you want to determine the outcome of: ");
            sID = Console.ReadLine();
            int.TryParse(sID, out eventID);
            Event e = events[eventID];
            Console.WriteLine(e.ToString());

            //Outcome
            Console.Write("What's the outcome of that event:");
            sOut = Console.ReadLine();
            int.TryParse(sOut, out outcome);


            BetESS.CloseEvent(e, outcome);
            //Notify Bookies
            //BetESS.NotifyBookiesAboutEndOfEvent(e);
        }

        /*** --------------------- BOOKIE MENU METHODS --------------------------- **/

        /// <summary>
        /// Executes the bookie menu. Available options are:
        /// 1 - Insert an Event
        /// 2 - Change Odds of an Event
        /// 3 - Subscribe to an Event
        /// 4 - List of Subscribed Events
        /// </summary>
        private static void BookieApp(string BookieEmail)
        {
            do
            {
                if (BetESS.AmountOfNotificationsFrom(BookieEmail) > 0)
                {
                    Console.WriteLine("You have new notifications!\n");
                    Console.WriteLine(BetESS.NotificationListFrom(BookieEmail));
                    BetESS.RemoveNotificationsFrom(BookieEmail);
                }
                BookieMenu.ExecuteMenu();
                switch (BookieMenu.getOption())
                {
                    case 1:
                        ShowSportsMenu(BookieEmail);
                        break;
                    case 2:
                        ChangeOdds(BookieEmail);
                        break;
                    case 3:
                        SubscribeToEvent(BookieEmail);
                        break;
                    case 4:
                        ListOfSubscribedEvents(BookieEmail);
                        break;
                }
            } while (BookieMenu.getOption() != 0);

        }

        /// <summary>
        /// Method which will display every available sport. The admin will only be able to place events related to these sports.
        /// </summary>
        private static void ShowSportsMenu(string BookieEmail)
        {
            do
            {
                SportsMenu.ExecuteMenu();
                switch (SportsMenu.getOption())
                {
                    case 1:
                        InsertNormalEvent("Football",BookieEmail);
                        break;
                }
            } while (SportsMenu.getOption() != 0);

        }


        /// <summary>
        /// Inserts a normal event (possible outcomes are victory, draw and loss).
        /// </summary>
        /// <param name="sportname">Name of the sport</param>
        /// <param name="BookieEmail">E-mail of the bookie.</param>
        private static void InsertNormalEvent(string sportname, string BookieEmail)
        {
            string descr, o1, o2, d;
            double odd1, odd2, draw;
            Console.Write("Description: ");
            descr = Console.ReadLine();
            Console.Write("Odds:\n1: ");
            o1 = Console.ReadLine();
            double.TryParse(o1, out odd1);
            Console.Write("2: ");
            o2 = Console.ReadLine();
            double.TryParse(o2, out odd2);
            Console.Write("Draw: ");
            d = Console.ReadLine();
            double.TryParse(d, out draw);

            NormalEvent ne = new NormalEvent(eventCounter, descr, BookieEmail, odd1, odd2, draw);
            BetESS.AddEvent(sportname, eventCounter, ne,BookieEmail);
            BetESS.SubscribeBookieToEvent(ne, BookieEmail);
            BetESS.AddSubscribedEventTo(ne.getEventID(), BookieEmail);
            Console.WriteLine("Event was added successfully!");
            eventCounter++;
        }

        /// <summary>
        /// Changes the odds of a certain event and notifies every bookie who subscribed to it.
        /// </summary>
        private static void ChangeOdds(string BookieEmail)
        {
            string sID, sOdd;
            int eventID;
            double odd;

            //List of all eventsID
            List<int> subscribedEvents = BetESS.GetSubscribedEventsFrom(BookieEmail);
            //<eventID, Event> with ALL Events
            Dictionary<int, Event> events = BetESS.MapOfAllEvents();

            //printing subscribed events
            Console.WriteLine("\t\tList of Events");
            foreach (int ev in subscribedEvents)
                Console.WriteLine(events[ev].ToString()+"-------");

            //Event
            Console.Write("Which event do you want to change the odds of: ");
            sID = Console.ReadLine();
            int.TryParse(sID, out eventID);
            Console.WriteLine("-------");
            Event e = events[eventID];
            Console.WriteLine(e.ToString()+"\tNew Odds:"); 

            //Changing Odds
            List<double> newOdds = new List<double>();
            List<Tuple<string,double>> oldOdds = BetESS.DisplayOddsFrom(e);
            foreach (Tuple<string,double> t in oldOdds)
            {
                Console.Write(t.Item1 + ": ");
                sOdd = Console.ReadLine();
                double.TryParse(sOdd, out odd);
                newOdds.Add(odd);
            }
            Console.WriteLine("##############################");

            //Changing odds
            BetESS.ChangeOddsTo(e,newOdds);

        }

        /// <summary>
        /// Method which handles the subscription of an event by a bookie.
        /// In other words, it subscribes the bookie to a certain event and adds that same event into the bookie's subscribed events.
        /// </summary>
        private static void SubscribeToEvent(string BookieEmail)
        {
            string sID;
            int eventID;


            Dictionary<int, Event> events = BetESS.MapOfAllEvents();

            //Remove subscribed events
            List<int> subscribedEvents = BetESS.GetSubscribedEventsFrom(BookieEmail);
            foreach (int key in subscribedEvents)
                if (events.ContainsKey(key)) events.Remove(key);

            Console.WriteLine("\t\tList of Events");
            foreach (Event ev in events.Values)
                Console.WriteLine(ev.ToString()+"------");

            //Event
            if (events.Count > 0)
            {
                Console.Write("Which event do want to subscribe to: ");
                sID = Console.ReadLine();
                int.TryParse(sID, out eventID);
                Event e = events[eventID];

                BetESS.SubscribeBookieToEvent(e, BookieEmail);
                BetESS.AddSubscribedEventTo(e.getEventID(), BookieEmail);
                Console.WriteLine("Event subscription was successful!");
            }
            else Console.WriteLine("There are no subscribable events.");
        }

        /// <summary>
        /// Prints the list of all subscribed events from a certain bookie.
        /// </summary>
        private static void ListOfSubscribedEvents(string BookieEmail)
        {
            Dictionary<int, Event> events = BetESS.MapOfAllEvents();
            List<int> subscribedEvents = BetESS.GetSubscribedEventsFrom(BookieEmail);

            Console.WriteLine("\t\tSubscribed Events:");
            foreach (int id in subscribedEvents)
            {
                Console.WriteLine(events[id].ToString()+"------");
            }
        }
        /*** ----------------------- Criteria Auxiliary Methods ------------------------------***/

        private static int LogicalMenu()
        {
            int option;
            string s;

            Console.Write("1 - And\n2 - Or\n0 - Leave\nOption: ");
            s = Console.ReadLine();
            int.TryParse(s, out option);
            return option;
        }

        private static Criteria.Criteria Expression()
        {
            int option;
            Criteria.Criteria c, c1;
            c = CriteriaMenu();
            do
            {
                option = LogicalMenu();
                switch (option)
                {
                    case 1:
                        c1 = CriteriaMenu();
                        c = new CriteriaAnd(c, c1);
                        break;
                    case 2:
                        c1 = CriteriaMenu();
                        c = new CriteriaOr(c, c1);
                        break;
                    default:
                        break;
                }

            } while (option != 0);

            return c;
        }

        private static Criteria.Criteria CriteriaMenu()
        {
            int option, x;
            string s;
            Console.Write("1 - Open Bets\n2 - Closed Bets\n3 - Bets with more than a certain amount of coins"+
                "\n4 - Bets with less than a certain amount of coins\n0 - Leave\nOption: ");
            s = Console.ReadLine();
            int.TryParse(s, out option);
            Criteria.Criteria c;

            switch (option)
            {
                case 1:
                    c = new CriteriaOpenBet();
                    break;
                case 2:
                    c = new CriteriaClosedBet();
                    break;
                case 3:
                    Console.Write("Amount: ");
                    s = Console.ReadLine();
                    int.TryParse(s, out x);
                    c = new CriteriaMoreThanAmountOfCoins(x);
                    break;
                case 4:
                    Console.Write("Amount: ");
                    s = Console.ReadLine();
                    int.TryParse(s, out x);
                    c = new CriteriaLessThanAmountOfCoins(x);
                    break;
                default:
                    c = null;
                    break;
            }

            return c;

        }

        /*** --------------------- Initialization Methods --------------------------- **/

        /// <summary>
        /// Both the main menu and the methods are created here.
        /// </summary>
        private static void LoadComponents()
        {
            Console.WriteLine("##############################\n\n    Welcome to BetESS v4.0! \n");
            BetESS = new System();
            LoadMenus();

        }

        /// <summary>
        /// Sets the event counter (responsible to tag the events) to 1. Inserts the available sports into the its list.
        /// </summary>
        private static void Initialize()
        {
            eventCounter = 1;
            betCounter = 1;
            Football f = new Football();
            BetESS.AddSport("Football", f);

        }


        /// <summary>
        /// Creates the several static menus used throughout the interface.
        /// </summary>
        private static void LoadMenus()
        {
            string[] Login = { "Register", "Login", "Admin Register", "Admin Login", "Bookie Register", "Bookie Login" };
            string[] MainMenu = { "List of events", "History of bets", "Place a bet", "Coins available", "Insert coins" };
            string[] AdmMenu = { "Determine outcome of event" };
            string[] sports = { "Football" };
            string[] bookieMenu = { "Insert Event", "Change Odds of an Event", "Subscribe to an event", "List of subscribed events" };
            string[] betHistory = { "All Bets", "Filter" };

            MenuLogin = new Menu(Login);
            UserMainMenu = new Menu(MainMenu);
            AdminMenu = new Menu(AdmMenu);
            SportsMenu = new Menu(sports);
            BookieMenu = new Menu(bookieMenu);
            BetHistoryMenu = new Menu(betHistory);
        }
    }

}
