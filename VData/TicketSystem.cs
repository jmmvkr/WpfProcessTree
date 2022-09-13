using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VData
{
    public class TicketSystem<ID_Type, Ticket> where Ticket : ITicket<ID_Type>
    {
        public Dictionary<ID_Type, Ticket> map = new Dictionary<ID_Type, Ticket>();
        public Func<TicketSystem<ID_Type, Ticket>, Ticket> makeNextTicket;
        int nCurrentRoll = 0;

        public Ticket next()
        {
            lock (this)
            {
                var t = makeNextTicket(this);
                var kNext = t.Id;
                map[kNext] = t;
                return t;
            }
        }

        public void destory(Ticket t)
        {
            lock (this)
            {
                map.Remove(t.Id);
            }
        }

        public int roll(int nRollMax)
        {
            int nNext = (1 + nCurrentRoll);
            if (nNext > nRollMax)
            {
                nNext = 1;
            }
            nCurrentRoll = nNext;
            return nNext;
        }

        public int maxId(IEnumerable<int> keys)
        {
            int nMaxId = -1;
            foreach (var n in keys)
            {
                if (n > nMaxId)
                {
                    nMaxId = n;
                }
            }
            return nMaxId;
        }

    } // end - class TicketSystem
}
