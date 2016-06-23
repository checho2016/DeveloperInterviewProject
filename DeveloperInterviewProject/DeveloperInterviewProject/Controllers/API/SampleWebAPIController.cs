using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjectEngine.Interfaces;

namespace DeveloperInterviewProject.Controllers.API
{
    public class SampleWebAPIController : ApiController
    {
        private readonly IStringParser _stringParserService;
        private readonly ICoursesProcessing _coursesProcessingService;

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";

        public SampleWebAPIController(IStringParser stringParserService, ICoursesProcessing coursesProcessingService)
        {
            _stringParserService = stringParserService;
            _coursesProcessingService = coursesProcessingService;
        }

        [HttpPost]
        [System.Web.Http.Route("API/SampleAPI/AnalyzeCourses")]
        public IHttpActionResult AnalyzeCourses([FromBody] string message)
        {
            var coursesList = new List<string>();
            var coursesCatalog = new Dictionary<string, decimal>();
            var callStack = new List<string>();

            _stringParserService.ProcessCoursesStrings(message, coursesCatalog, ref coursesList);
            _coursesProcessingService.CoursesCorrelation(callStack, coursesCatalog, coursesList);
            var result = _stringParserService.PrcessCoursesOutput(coursesCatalog);

            return Ok(result);
        }
    }
}
