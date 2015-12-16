using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports
{
    public class NormalEvent : Event
    {
        double odd1;
        double odd2;
        double drawOdd;


        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public NormalEvent() : base()
        {
            odd1 = 0;
            odd2 = 0;
            drawOdd = 0;
        }

        /// <summary>
        /// Param Constructor.
        /// </summary>
        /// <param name="id">Id of the event.</param>
        /// <param name="descr">Event description.</param>
        /// <param name="bookie">Bookie which created the event.</param>
        /// <param name="odd1">Odds for home win.</param>
        /// <param name="odd2">Odds for away win.</param>
        /// <param name="drawOdd">Odds for a draw.</param>
        public NormalEvent (int id, String descr, string bookie, double odd1, double odd2, double drawOdd) : base(id,descr,bookie)
        {
            this.odd1 = odd1;
            this.odd2 = odd2;
            this.drawOdd = drawOdd;
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        /// <param name="ne">NormalEvent to be copied from.</param>
        public NormalEvent (NormalEvent ne) : base(ne)
        {
            this.odd1 = ne.getOdd1();
            this.odd2 = ne.getOdd2();
            this.drawOdd = ne.getDrawOdd();
        }


        /**Getters and Setters **/
        public double getOdd1 () { return this.odd1; }
        public double getOdd2 () { return this.odd2; }
        public double getDrawOdd () { return this.drawOdd; }

        public void setOdd1 (double o1) { this.odd1 = o1; }
        public void setOdd2 (double o2) { this.odd2 = o2; }
        public void setOddDraw(double od) { this.drawOdd = od; }


        /** Equals, Clone, ToString**/

        public override Event Clone ()
        {
            return new NormalEvent(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            if (obj == null || obj.GetType() != this.GetType()) return false;
            NormalEvent ne = (NormalEvent)obj;
            return (odd1 == ne.getOdd1() && odd2 == ne.getOdd2() && drawOdd == ne.getDrawOdd() && base.Equals(ne));
        }

        public override string ToString ()
        {
            StringBuilder sb = new StringBuilder("Normal Event (1|X|2)\n");
            sb.Append(base.ToString());
            sb.Append("1 - ").Append(this.odd1).Append("\n");
            sb.Append("X - ").Append(this.drawOdd).Append("\n");
            sb.Append("2 - ").Append(this.odd2).Append("\n");
            return sb.ToString();
        }


        /// <summary>
        /// Returns a List with all of the odds of a Normal Event (1|X|2).
        /// Specific implementation of DisplayOdds (Abstract method of Event) for a Normal Event.
        /// The configuration used is as follows:
        /// [0] - Home Win Odd
        /// [1] - Draw Odd
        /// [2] - Away Win
        /// </summary>
        /// <returns>A list of a tuple for every odd. In this case, it'll return a list of three tuples.</returns>
        public override List<Tuple<string, double>> DisplayOdds ()
        {
            List<Tuple<string, double>> result = new List<Tuple<string, double>>();
            Tuple<string, double> homeWin = new Tuple<string, double>("1 - ", odd1);
            Tuple<string, double> draw = new Tuple<string, double>("X - ", drawOdd);
            Tuple<string, double> awayWin = new Tuple<string, double>("2 - ", odd2);
            result.Add(homeWin);
            result.Add(draw);
            result.Add(awayWin);
            return result;
        }

        /// <summary>
        /// Returns the odd for a specific outcome of a NormalEvent (1/X/2).
        /// </summary>
        /// <param name="opt">Outcome (Note: Draw is represented by 0).</param>
        /// <returns>Respective odd.</returns>
        public override double GetSpecificOdd(int opt)
        {
            switch (opt)
            {
                case 1:
                    return odd1;
                case 2:
                    return odd2;
                case 0:
                    return drawOdd;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// When called upon in this subclass (NormalEvent) the list will contain the new odds for Home win, draw and away win, respectively.
        /// </summary>
        /// <param name="list">List which contains new odds.</param>
        public override void ChangeOdds(List<double> list)
        {

            odd1 = list[0];
            odd2 = list[2];
            drawOdd = list[1];
            NotifyObservers(ObservableEvents.OddChange);
        }
    }
}
