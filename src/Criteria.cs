using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sports;

namespace Criteria
{
    public interface Criteria
    {
        Dictionary<int, Bet> meetCriteria(Dictionary<int, Bet> bets);
    }
}
