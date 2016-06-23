using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectEngine.Implementations
{
    public class CoursesProcessing : Interfaces.ICoursesProcessing
    {
        public void CoursesCorrelation(List<string> callStack, Dictionary<string, decimal> coursesCatalog, List<string> coursesList)
        {
            while (coursesCatalog.Any(coursePointer => coursePointer.Value < 0))
            {
                callStack.Clear();
                var courseTarget = coursesCatalog.First(coursePointer => coursePointer.Value < 0).Key;
                while (courseTarget != "loop" && courseTarget != "base")
                {
                    courseTarget = recursiveInsight(courseTarget, callStack, coursesCatalog, coursesList);
                }

                if (courseTarget == "loop")
                {
                    foreach (var coursePointer in callStack)
                        coursesCatalog[coursePointer] = 0.1M;
                }
                if (courseTarget == "base")
                {
                    var courseLevelSetup = callStack.Count;

                    foreach (var coursePointer in callStack)
                    {
                        coursesCatalog[coursePointer] = courseLevelSetup;
                        courseLevelSetup--;
                    }
                }
            }
        }

        private string recursiveInsight(string courseTarget, List<string> callStack, Dictionary<string, decimal> coursesCatalog, List<string> coursesList)
        {
            if (callStack.Contains(courseTarget))
                return "loop";

            if (coursesCatalog[courseTarget] < 1 && coursesCatalog[courseTarget] >= 0)
                return "base";
            else
            {
                callStack.Add(courseTarget);
                var result = coursesList.First(coursePointer => coursePointer.StartsWith(courseTarget) && coursePointer.IndexOf(courseTarget) < coursePointer.IndexOf(":"));
                result = result.Replace(courseTarget, "");
                result = result.Replace(":", "");
                result = Regex.Replace(result, @"(?:((^\s\s*)|(\s*\s$)))", "");
                return result;
            }
        }

    }
}
