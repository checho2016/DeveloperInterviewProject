using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProjectEngine.Implementations
{
    public class StringParser : Interfaces.IStringParser
    {
        public string ProcessCoursesStrings(string coursesString)
        {
            coursesString = Regex.Replace(coursesString, "(([ ]){3,}) ", Environment.NewLine);
            var coursesList = coursesString.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            var courseRegex = new Regex(@".+:");
            var match = courseRegex.Match(coursesString);
            var coursesCatalog = new Dictionary<string, decimal>();

            var isCannonicalCourse = new Regex(@"(?<=:).+\w");

            for(var ii=0; ii < coursesList.Count; ii++)
            {
                if(coursesList[ii] == "" || coursesList[ii] == " ")
                {
                    coursesList.RemoveAt(ii);
                    ii--;                    
                }
            }

            foreach (Match itemMatch in courseRegex.Matches(coursesString))
            {
                if (itemMatch.Value != "" && itemMatch.Value != " ")
                {
                    var x = itemMatch.Value.Replace(":", "");
                    var targetCourseLineDescription = coursesList.First(coursePointer => coursePointer.StartsWith(x) && coursePointer.IndexOf(x) < coursePointer.IndexOf(":"));

                    if (isCannonicalCourse.IsMatch(targetCourseLineDescription))
                        coursesCatalog.Add(itemMatch.Value.Replace(":", ""), -1);
                    else
                        coursesCatalog.Add(itemMatch.Value.Replace(":", ""), 0);
                }
            }

            var callStack = new List<string>();
            
            var result = "";

            var lastItemType = "none";
            foreach(var resultPointer in coursesCatalog.OrderBy(p => p.Value))
            {
                if(resultPointer.Value % 1 == 0)
                {
                    if (lastItemType == "loop")
                    {
                        result = result.Substring(0, result.Length - 2);
                        result += ", ";
                    }
                    result += resultPointer.Key + ", ";
                }
                else
                {
                    result += resultPointer.Key + "- ";
                    lastItemType = "loop";
                }
            }
            result = result.Substring(0, result.Length - 2);
            return result;
        }

        private string recursiveInsight(string courseTarget, List<string> callStack, Dictionary<string, decimal> coursesCatalog, List<string> coursesList)
        {
            if (callStack.Contains(courseTarget))
                return "loop";

            if (coursesCatalog[courseTarget] < 1 && coursesCatalog[courseTarget] >= 0 )
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
