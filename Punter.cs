using System;
using System.Collections.Generic;
using System.Text;
using Sports;

namespace BetESS
{
    public class Punter : User, Observer
    {
        private float BetESSCoins;
        private Dictionary<int,int> openBets;
        private Dictionary<int,int> closedBets;



        /// <summary>
        /// Empty Constructor. 
        /// </summary>
        public Punter () : base()
        {
            this.BetESSCoins = 0;
            openBets = new Dictionary<int,int>();
            closedBets = new Dictionary<int,int>();
        }

        /// <summary>
        /// Param Constructor. 
        /// </summary>
        /// <param name="name">Name of the punter.</param>
        /// <param name="email">Email of the punter.</param>
        /// <param name="pwd">Password of the punter.</param>
        /// <param name="coins">Coins the punter has.</param>
        public Punter (String name, String email, String pwd, float coins) : base(name,email,pwd) 
        {
            this.BetESSCoins = coins;
            openBets = new Dictionary<int, int>();
            closedBets = new Dictionary<int, int>();
        }

        //Copy Constructor
        public Punter (Punter u) : base((User)u)
        {
            this.BetESSCoins = u.getBetESSCoins();
            this.openBets = u.getOpenBets();
            this.closedBets = u.getClosedBets();
        }




        /*Getters and Setters*/
        public float getBetESSCoins() { return this.BetESSCoins; }
        public Dictionary<int, int> getOpenBets ()
        {
            Dictionary<int, int> l = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> i in openBets)
                l.Add(i.Key, i.Value);
            return l;
        }
        public Dictionary<int, int> getClosedBets()
        {
            Dictionary<int, int> l = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> i in closedBets)
                l.Add(i.Key, i.Value);
            return l;
        }
        
        public void setBetESSCoins (float coins) { this.BetESSCoins = coins; }





                                   /** ---------------- COINS -------------------------- **/
                      
        /// <summary>
        /// Credit coins into the punter's account.
        /// </summary>
        /// <param name="coins">Amount of coins to be credited.</param>
        public void CreditCoins (float coins)
        {
            this.BetESSCoins += coins;
        }

        /// <summary>
        /// Debit coins from the punter's account.
        /// </summary>
        /// <param name="coins">Amount of coints debited from the account.</param>
        public void DebitCoins (float coins)
        {
            this.BetESSCoins -= coins;
        }



                                    /*** ------------- BETS -----------------------***/
        /// <summary>
        /// Adds a bet to the punter's open bet list.
        /// </summary>
        /// <param name="betID">ID of the bet.</param>
        public void AddOpenBet (int betID)
        {
            openBets.Add(betID,betID);
        }

        /// <summary>
        /// Removes a bet from the punter's open bet list and inserts the same bet into the closed bet list.
        /// </summary>
        /// <param name="betID">ID of the bet.</param>
        public void CloseOpenBet(int betID)
        {
            int id = openBets[betID];
            openBets.Remove(betID);
            closedBets.Add(betID, betID);
            
        }

        /// <summary>
        /// Display the history of all bets taken by the punter (both the open and closed ones).
        /// </summary>
        /// <returns>A structure which contains every bet, separated by its open status. The structure is as follows:
        /// "Closed"|"Open" -> Dictionary (betId,betID).
        /// A dictionary with keys and values being the same is used due to C#'s implemtantion of an HashSet not enabling to remove items.</returns>
        public Dictionary<string,Dictionary<int,int>> BetHistory ()
        {
            Dictionary<int, int> oBets = new Dictionary<int, int>();
            Dictionary<int, int> cBets = new Dictionary<int, int>();
            Dictionary<string, Dictionary<int, int>> allBets = new Dictionary<string, Dictionary<int, int>>();
            foreach (KeyValuePair<int, int> b in openBets)
                oBets.Add(b.Key, b.Value);
            allBets.Add("Open", oBets);
            foreach (KeyValuePair<int, int> b in closedBets)
                cBets.Add(b.Key, b.Value);
            allBets.Add("Closed", cBets);

            return allBets;
        }



        //Observer methods
        public void Update (String s)
        {
            this.PushNotification(s);
        }            


        /*ToString, clone and Equals*/
        public override User Clone ()
        {
            return new Punter(this);
        }


        public override String ToString() 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append("BetESSCoins: " + this.BetESSCoins);
            return sb.ToString();
        }

        public override bool Equals(Object obj)
        {
            if (obj == this) return true;
            if (obj == null || this.GetType() != obj.GetType()) return false;
            Punter u = (Punter)obj;
            return (base.Equals(obj) && this.BetESSCoins == u.getBetESSCoins() 
                && OpenBetsListEquals(u.getOpenBets()) && ClosedBetsListEquals(u.getClosedBets()));
        }

        public bool OpenBetsListEquals(Dictionary<int,int> d)
        {
            if (this.openBets.Count != d.Count) return false;
            foreach (int key in openBets.Keys)
                if (!d.ContainsKey(key)) return false;
            return true;
        }

        public bool ClosedBetsListEquals(Dictionary<int, int> d)
        {
            if (this.closedBets.Count != d.Count) return false;
            foreach (int key in closedBets.Keys)
                if (!d.ContainsKey(key)) return false;
            return true;
        }
    }
}
