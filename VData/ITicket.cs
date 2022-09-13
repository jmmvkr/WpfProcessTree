using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VData
{
    public interface ITicket<ID_Type>
    {
        ID_Type Id { get; }
    }
}
