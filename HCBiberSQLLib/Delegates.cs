using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCBiberSQLLib
{
    public delegate void _ErrorEvent(string ErrorMessage);

    public delegate void _ErrorEventPacked(HCBiberSQLErrorMsgPacked ErrorMessage);
}
