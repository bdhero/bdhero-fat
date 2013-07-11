using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsanPlugin
{
    public class VIsan : Isan
    {
        public Isan Parent;

        protected VIsan(string n1, string n2, string n3, string n4, string n5, string n6) : base(n1, n2, n3, n4, n5, n6)
        {
        }

        public new static VIsan TryParse(string number)
        {
            if (!IsIsan(number))
                return null;

            var n = Parse(number);

            return new VIsan(n[0], n[1], n[2], n[3], n[4], n[5]);
        }
    }
}
