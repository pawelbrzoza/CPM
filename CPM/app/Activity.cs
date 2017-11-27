using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPM
{
    public class Activity
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public int Begin { get; set; }
        public int End { get; set; }

        public Event First { get; private set; }
        public Event Next { get; private set; }
 
        public Activity()
        {
            First = new Event();
            Next = new Event();
        }
    }
}