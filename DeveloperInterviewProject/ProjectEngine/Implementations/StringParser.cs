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
        public void ProcessCoursesStrings(string coursesString, Dictionary<string, decimal> coursesCatalog, ref List<string> coursesList)
        {
            coursesString = Regex.Replace(coursesString, "(([ ]){3,}) ", Environment.NewLine);
            coursesString = Regex.Replace(coursesString, "\n", Environment.NewLine);
            coursesList = coursesString.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            var courseRegex = new Regex(@".+:");
            var match = courseRegex.Match(coursesString);

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
        }

        public string PrcessCoursesOutput(Dictionary<string, decimal> coursesCatalog)
        {
            var result = "";

            var lastItemType = "none";
            foreach(var resultPointer in coursesCatalog.OrderBy(p => p.Value))
            {
                if (resultPointer.Value % 1 == 0)
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

            if (result.Contains("-"))
                result = "Notice: A cricular reference was detected in the courses entered. " + result;

            return result;
        }
    }
}
