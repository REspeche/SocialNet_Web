using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Helpers
{
    public class CultureHelper
    {
        // Include ONLY cultures you are implementing
        private static readonly IList<string> _cultures = new List<string> {
            "en",
            "es"
        };

        public static bool isCultureExist(string name)
        {
            return (_cultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() > 0);
        }

        public static List<string> otherCultures(string name)
        {
            return _cultures.Where(c => !c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).ToList<string>();
        }
    }
}