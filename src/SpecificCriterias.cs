using System;
using System.Collections.Generic;
using Sports;

namespace Criteria
{
    /// <summary>
    /// Criteria to match bets that are still open.
    /// </summary>
    public class CriteriaOpenBet : Criteria
    {
        public Dictionary<int,Bet> meetCriteria (Dictionary<int,Bet> bets)
        {
            Dictionary<int, Bet> result = new Dictionary<int, Bet>();
            foreach (KeyValuePair<int, Bet> b in bets)
                if (!b.Value.getClosedStatus())
                    result.Add(b.Key, b.Value);
            return result;
        }
    }

    /// <summary>
    /// Criteria to match bets that are closed.
    /// </summary>
    public class CriteriaClosedBet : Criteria
    {
        public Dictionary<int, Bet> meetCriteria(Dictionary<int, Bet> bets)
        {
            Dictionary<int, Bet> result = new Dictionary<int, Bet>();
            foreach (KeyValuePair<int, Bet> b in bets)
                if (b.Value.getClosedStatus())
                    result.Add(b.Key, b.Value);
            return result;
        }
    }

    /// <summary>
    /// Criteria to match bets with more than a certain amount of coins.
    /// </summary>
    public class CriteriaMoreThanAmountOfCoins : Criteria
    {

        private float coins;

        public CriteriaMoreThanAmountOfCoins (float c)
        {
            this.coins = c;
        }

        public Dictionary<int, Bet> meetCriteria(Dictionary<int, Bet> bets)
        {
            Dictionary<int, Bet> result = new Dictionary<int, Bet>();
            foreach (KeyValuePair<int, Bet> b in bets)
                if (b.Value.getCoins()>this.coins)
                    result.Add(b.Key, b.Value);
            return result;
        }
    }

    /// <summary>
    /// Criteria to match bets with less than a certain amount of coins.
    /// </summary>
    public class CriteriaLessThanAmountOfCoins : Criteria
    {

        private float coins;

        public CriteriaLessThanAmountOfCoins(float c)
        {
            this.coins = c;
        }

        public Dictionary<int, Bet> meetCriteria(Dictionary<int, Bet> bets)
        {
            Dictionary<int, Bet> result = new Dictionary<int, Bet>();
            foreach (KeyValuePair<int, Bet> b in bets)
                if (b.Value.getCoins() < this.coins)
                    result.Add(b.Key, b.Value);
            return result;
        }
    }

    /// <summary>
    /// Receives to criteria and applies the AND logical expression to them.
    /// </summary>
    public class CriteriaAnd : Criteria
    {

        private Criteria criteria;
        private Criteria otherCriteria;

        public CriteriaAnd(Criteria criteria, Criteria otherCriteria)
        {
            this.criteria = criteria;
            this.otherCriteria = otherCriteria;
        }

        public Dictionary<int,Bet> meetCriteria(Dictionary<int,Bet> bets)
        {

            Dictionary<int,Bet> firstCriteriaBets = criteria.meetCriteria(bets);
            return otherCriteria.meetCriteria(firstCriteriaBets);
        }
    }

    /// <summary>
    /// Applies the OR logical expressions to two criteria.
    /// </summary>
    public class CriteriaOr : Criteria
    {
        private Criteria criteria;
        private Criteria otherCriteria;

        public CriteriaOr(Criteria criteria, Criteria otherCriteria)
        {
            this.criteria = criteria;
            this.otherCriteria = otherCriteria;
        }

        public Dictionary<int,Bet> meetCriteria(Dictionary<int,Bet> bets)
        {
            Dictionary<int, Bet> firstCriteriaBets = criteria.meetCriteria(bets);
            Dictionary<int, Bet> otherCriteriaBets = otherCriteria.meetCriteria(bets);

            foreach (KeyValuePair<int,Bet> b in otherCriteriaBets)
            {
                if (!firstCriteriaBets.ContainsKey(b.Key))
                    firstCriteriaBets.Add(b.Key,b.Value);
            }
            return firstCriteriaBets;
        }


    }
}
