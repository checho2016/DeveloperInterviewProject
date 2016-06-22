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

        public SampleWebAPIController(IStringParser stringParserService, ICoursesProcessing coursesProcessingService)
        {
            _stringParserService = stringParserService;
            _coursesProcessingService = coursesProcessingService;
        }

        [HttpPost]
        [System.Web.Http.Route("API/SampleAPI/AnalyzeCourses")]
        public IHttpActionResult AnalyzeCourses([FromBody] string message)
        {
            var result = _stringParserService.ProcessCoursesStrings(message);

            if (result.Contains("-"))
                result = "Notice: A cricular reference was detected in the courses entered. " + result;
            return Ok(result);
        }
    }
}
