using System;
using System.Collections.Generic;
using System.Text;
namespace Sports
{
    public class Bet
    {
        private int betID { get; set; }
        private string punter { get; set; }
        private int option { get; set; }
        private double odd { get; set; }
        private float coins { get; set; }
        private string description { get; set; }
        private bool closed { get; set; }
        private bool won { get; set; }


        /// <summary>
        /// Empty Constructor.
        /// </summary>
        /// <param name="id">Id of the bet.</param>
        public Bet (int id)
        {
            betID = id;
            punter = description = "";
            option = 0;
            coins = 0;
            coins = 0;
            closed = won = false;
        }

        /// <summary>
        /// Param Constructor.
        /// </summary>
        /// <param name="bID">Bet ID.</param>
        /// <param name="desc">Bet description.</param>
        /// <param name="name">Name of the better.</param>
        /// <param name="opt">Option chosen by the better.</param>
        /// <param name="o">Odds.</param>
        /// <param name="c">Coins bet.</param>
        public Bet (int bID, string desc, string name, int opt, double o, float c)
        {
            betID = bID;
            description = desc;
            punter = name;
            option = opt;
            odd = o;
            coins = c;
            closed = won = false;
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        /// <param name="b">Bet to be copied.</param>
        public Bet (Bet b)
        {
            betID = b.betID;
            description = b.description;
            punter = b.punter;
            option = b.option;
            odd = b.odd;
            coins = b.coins;
            closed = b.closed;
            won = b.won;
        }

        /*Getters*/
        public int getBetID () { return this.betID; }
        public string getDescription () { return this.description; }
        public string getBetter() { return this.punter; }
        public int getOption () { return this.option; }
        public double getOdd () { return this.odd; }
        public float getCoins () { return this.coins; }
        public bool getClosedStatus () { return this.closed; }
        public bool getWonStatus () { return this.won; }
        

        /// <summary>
        /// Closes a bet, defining whether or not the bet was won.
        /// </summary>
        /// <param name="wonBet">True if bet was won, false otherwise.</param>
        public void CloseBet (bool wonBet)
        {
            closed = true;
            won = wonBet;
        }

       
        /**Equals, ToString, clone**/
        public Bet Clone()
        {
            return new Bet(this);
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("Bet ID - ").Append(betID).Append("\n");
            sb.Append(description).Append("\n");
            sb.Append("Option chosen - ").Append(option).Append("\n");
            sb.Append("Odds for that option: ").Append(odd).Append("\n");
            sb.Append("Coins: ").Append(coins).Append("\n");
            
            if (!closed) sb.Append("Possible gain: " + (coins * odd)).Append("\n");
            else
            {
                if (won) {
                    sb.Append("Won bet!\n");
                    sb.Append("Coins won: " + (coins * odd)).Append("\n");
                }
                else sb.Append("Lost bet.\n");
            }
            return sb.ToString();
        }

        public override bool Equals(Object obj)
        {
            if (this == obj) return true;
            if (obj == null || obj.GetType() != this.GetType()) return false;
            Bet b = (Bet)obj;
            return (betID == b.betID && punter.Equals(b.punter) && description.Equals(b.description) 
                && option == b.option && odd == b.odd && coins == b.coins && closed==b.closed && won==b.won);
        }
    }
}
