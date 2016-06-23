using System;
using System.Collections.Generic;
using System.Linq;
using DeveloperInterviewProject.Controllers.API;
using ProjectEngine.Interfaces;
using ProjectEngine.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DeveloperInterviewProject.Tests.Controllers
{
    [TestClass]
    public class SampleWebAPIControllerTest
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";

        private IStringParser _stringParserService;
        private ICoursesProcessing _coursesProcessingService;

        [TestInitialize]
        public void SetupTests()
        {
            _stringParserService = new StringParser();
            _coursesProcessingService = new CoursesProcessing();
        }

        [TestMethod]
        public void RequirementsPositiveTest()
        {
            string sampleText = @"Introduction to Paper Airplanes:
Advanced Throwing Techniques: Introduction to Paper Airplanes
History of Cubicle Siege Engines: Rubber Band Catapults 101
Advanced Office Warfare: History of Cubicle Siege Engines 
Rubber Band Catapults 101: 
Paper Jet Engines: Introduction to Paper Airplanes";
            
            var coursesList = new List<string>();
            var coursesCatalog = new Dictionary<string, decimal>();
            var callStack = new List<string>();

            _stringParserService.ProcessCoursesStrings(sampleText, coursesCatalog, ref coursesList);
            Assert.AreEqual(coursesCatalog.Count, coursesList.Count); //parsed relationships equal to courses
            Assert.AreEqual(sampleText.Split('\n').Length, coursesCatalog.Count); //raw relationships equal to courses

            _coursesProcessingService.CoursesCorrelation(callStack, coursesCatalog, coursesList);
            Assert.AreEqual(0, coursesCatalog.Count(test => test.Value < 0)); //not having any course with the corresponding requisite solved            

            var functionResult = _stringParserService.PrcessCoursesOutput(coursesCatalog);            
            Assert.AreEqual("Introduction to Paper Airplanes, Rubber Band Catapults 101, Advanced Throwing Techniques, History of Cubicle Siege Engines, Paper Jet Engines, Advanced Office Warfare", functionResult);
        }

        [TestMethod]
        public void RequirementsCircularReferenceTest()
        {
            string sampleText = @"Intro to Arguing on the Internet: Godwin’s Law
Understanding Circular Logic: Intro to Arguing on the Internet
Godwin’s Law: Understanding Circular Logic";

            var coursesList = new List<string>();
            var coursesCatalog = new Dictionary<string, decimal>();
            var callStack = new List<string>();

            _stringParserService.ProcessCoursesStrings(sampleText, coursesCatalog, ref coursesList);
            Assert.AreEqual(coursesCatalog.Count, coursesList.Count); //parsed relationships equal to courses
            Assert.AreEqual(sampleText.Split('\n').Length, coursesCatalog.Count); //raw relationships equal to courses

            _coursesProcessingService.CoursesCorrelation(callStack, coursesCatalog, coursesList);
            Assert.AreEqual(0, coursesCatalog.Count(test => test.Value < 0)); //not having any course with the corresponding requisite solved

            var functionResult = _stringParserService.PrcessCoursesOutput(coursesCatalog);
            Assert.AreEqual("Notice: A cricular reference was detected in the courses entered. Intro to Arguing on the Internet- Understanding Circular Logic- Godwin’s Law", functionResult);          
        }

        [TestMethod]
        public void WebserviceStressPerformanceTest()
        {
            var caseCorrelationRatio = 1;
            var correlationCount = 0;

            for (var counter = 0; counter < 10000; counter++)
            {
                if (correlationCount == caseCorrelationRatio)
                {
                    RequirementsPositiveTest();
                    correlationCount = 0;
                }                    
                else
                {
                    RequirementsCircularReferenceTest();
                    correlationCount++;
                }                    
            }
        }

        [TestMethod]
        public void RandomCoursesTest()
        {
            var hasCircularReference = false;
            var courseCaseCorrelationRatio = 33; //0 to 100
            var randomCourseOptionSeed = new Random();
            var generateCourseName = new Random();
            var randomCourseOption = randomCourseOptionSeed.Next(100);
            var coursesTestString = "";
            var coursesTestCollection = new List<string>();

            for (var coursesRelationshipIterator = 0; coursesRelationshipIterator < 8; coursesRelationshipIterator++)
            {

                if (randomCourseOption < courseCaseCorrelationRatio || coursesTestCollection.Count == 0) //cannonical course
                {

                    var courseName = new string(Enumerable.Repeat(chars, 15)
                                     .Select(s => s[generateCourseName.Next(s.Length)]).ToArray());
                    courseName = "T" + courseName + "T";

                    coursesTestString += string.Format("{0}: {1}", courseName, Environment.NewLine);
                    coursesTestCollection.Add(courseName);
                }
                if(coursesTestCollection.Count > 0 && randomCourseOption > courseCaseCorrelationRatio && randomCourseOption < 2 * courseCaseCorrelationRatio) //requisite course
                {
                    var generateCourseRequisiteSeed = new Random();
                    var randomRequisite = generateCourseRequisiteSeed.Next(coursesTestCollection.Count());

                    var courseName = new string(Enumerable.Repeat(chars, 15)
                                     .Select(s => s[generateCourseName.Next(s.Length)]).ToArray());
                    courseName = "T" + courseName + "T";

                    coursesTestCollection.Add(courseName);

                    coursesTestString += string.Format("{0}: {1}{2}", courseName, coursesTestCollection[randomRequisite], Environment.NewLine);
                }

                if (randomCourseOption > 2*courseCaseCorrelationRatio && randomCourseOption < 3 * courseCaseCorrelationRatio) //circular requisite course
                {
                    var generateCourseRequisiteSeed = new Random();
                    var randomRequisite = generateCourseRequisiteSeed.Next(2,12);
                    var circularReferenceCollection = new List<string>();

                    hasCircularReference = true;

                    for (var courseCreationIterator = 0; courseCreationIterator < randomRequisite; courseCreationIterator++)
                    {
                        var courseName = new string(Enumerable.Repeat(chars, 15)
                                     .Select(s => s[generateCourseName.Next(s.Length)]).ToArray());
                        courseName = "T" + courseName + "T";

                        circularReferenceCollection.Add(courseName);
                        coursesTestCollection.Add(courseName);
                    }

                    for (var circularReferenceIterator = 0; circularReferenceIterator < randomRequisite; circularReferenceIterator++)
                    {

                        if(circularReferenceIterator == 0)
                            coursesTestString += string.Format("{0}: {1} {2}", circularReferenceCollection[circularReferenceCollection.Count - 1], circularReferenceCollection[circularReferenceIterator], Environment.NewLine);
                        else
                            coursesTestString += string.Format("{0}: {1} {2}", circularReferenceCollection[circularReferenceIterator - 1], circularReferenceCollection[circularReferenceIterator], Environment.NewLine);
                    }                        
                }
            }

            var coursesList = new List<string>();
            var coursesCatalog = new Dictionary<string, decimal>();
            var callStack = new List<string>();

            _stringParserService.ProcessCoursesStrings(coursesTestString, coursesCatalog, ref coursesList);
            Assert.AreEqual(coursesCatalog.Count, coursesList.Count); //parsed relationships equal to courses
            Assert.AreEqual(coursesTestString.Split('\n').Length - 1, coursesCatalog.Count); //raw relationships equal to courses

            _coursesProcessingService.CoursesCorrelation(callStack, coursesCatalog, coursesList);
            Assert.AreEqual(0, coursesCatalog.Count(test => test.Value < 0)); //not having any course with the corresponding requisite solved

            var functionResult = _stringParserService.PrcessCoursesOutput(coursesCatalog);
            Assert.AreEqual(hasCircularReference, functionResult.Contains("Notice: A cricular reference was detected in the courses entered."));
        }

        [TestMethod]
        public void WebserviceFullyRandomStressPerformanceTest()
        {
            for (var counter = 0; counter < 10000; counter++)
            {
                RandomCoursesTest();
            }
        }

    }
}
