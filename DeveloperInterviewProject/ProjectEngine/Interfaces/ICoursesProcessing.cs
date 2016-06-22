using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEngine.Interfaces
{
    public interface ICoursesProcessing
    {
        void CoursesCorrelation(List<string> callStack, Dictionary<string, decimal> coursesCatalog, List<string> coursesList);
    }
}
