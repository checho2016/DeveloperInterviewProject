using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEngine.Interfaces
{
    public interface IStringParser
    {
        void ProcessCoursesStrings(string coursesString, Dictionary<string, decimal> coursesCatalog, ref List<string> coursesList);
        string PrcessCoursesOutput(Dictionary<string, decimal> coursesCatalog);
    }
}
