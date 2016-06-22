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

        public SampleWebAPIController(IStringParser stringParserService)
        {
            _stringParserService = stringParserService;
        }

        [HttpPost]
        [System.Web.Http.Route("API/SampleAPI/AnalyzeCourses")]
        public IHttpActionResult AnalyzeCourses([FromBody] string message)
        {
            var result = _stringParserService.ProcessCoursesStrings(message);

            return Ok(result);
        }
    }
}
