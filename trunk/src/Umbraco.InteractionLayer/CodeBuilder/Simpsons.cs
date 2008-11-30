using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.cms.businesslogic.web;

namespace Umbraco.InteractionLayer.CodeBuilder
{
    internal class Abraham
    {
    }
    internal class Homer
    {
        public Marge Where(Func<Bart, bool> p) { throw new NotImplementedException(); }
        public Abraham Father { get; set; }
    }
    internal class Lisa
    {
        public Maggie Sister { get; set; }
    }
    internal class Bart
    {
        public bool HasSkateboard { get; set; }
    }
    internal class Maggie
    {
        public Lisa Sister { get; set; }
    }
    internal class Marge
    {
        public Homer Select(Func<Lisa, Maggie> p) { throw new NotImplementedException(); }
    }
    internal class Tester
    {
        public Tester()
        {
            var res = (from x in new Homer()
                       where x.HasSkateboard
                       select x.Sister).Father;
        }
    }
}
