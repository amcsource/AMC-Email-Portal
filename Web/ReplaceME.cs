using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL_AMCPE
{
    class ReplaceME
    {


        public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.IndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }
    }
}
