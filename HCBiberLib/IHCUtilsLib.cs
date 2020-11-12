using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCBiberLib
{
    interface IHCLastError
    {
        string _LastErrorMsg { set; get; }

        string _LastErrorWrite { set;  }

        string LastErrorMsg { get; }
    }

    public abstract class AHCLastError : IHCLastError
    {
        public string _LastErrorMsg { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string _LastErrorWrite { set => throw new NotImplementedException(); }

        public string LastErrorMsg => throw new NotImplementedException();
    }
}
