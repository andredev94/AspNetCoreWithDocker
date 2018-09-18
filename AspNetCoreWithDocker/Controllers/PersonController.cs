using AspNetCoreWithDocker.Business.Rules.People;
using AspNetCoreWithDocker.DataTransformers.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using Tapioca.HATEOAS;

namespace AspNetCoreWithDocker.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private IPersonBusiness _personService;

        public PersonController(IPersonBusiness personService)
        {
            _personService = personService;
        }

        // GET api/values
        [HttpGet]
        [Authorize("Bearer")]
        [SwaggerOperation(
            Summary = "List all people",
            Description = "Open, not requires any permission",
            OperationId = "ListAllPeople",
            Tags = new[] { "Person" }
        )]
        [SwaggerResponse(200, "all the people were recovered", typeof(List<PersonVO>))]
        [SwaggerResponse(400, "The request was incorrect")]
        [SwaggerResponse(500, "Oops! Some internal error has occurred")]
        [SwaggerResponse(401, "Oops! Some error has occurred with your authentication")]
        [SwaggerResponse(404, "Oops! we don't find what you need, sorry!")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_personService.FindAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(int id)
        {
            var person = _personService.FindById(id);

            if (person == null)
            {
                return NoContent();
            }

            return Ok(person);
        }

        [HttpGet("find-by-name")]
        [Authorize("Bearer")]
        [SwaggerOperation(
            Summary = "Find a person by your first name",
            Description = "Requires authentication",
            OperationId = "FindByName",
            Tags = new[] { "Person" }
        )]
        [SwaggerResponse(200, "A person were find", typeof(List<PersonVO>))]
        [SwaggerResponse(400, "The request was incorrect")]
        [SwaggerResponse(500, "Oops! Some internal error has occurred")]
        [SwaggerResponse(401, "Oops! Some error has occurred with your authentication")]
        [SwaggerResponse(404, "Oops! we don't find what you need, sorry!")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get([FromQuery] string firstName)
        {
            var person = _personService.FindByName(firstName);

            if (person == null)
            {
                return NoContent();
            }

            return Ok(person);
        }

        [HttpGet("find-with-paged-search/{sortDirection}/{pageSize}/{page}")]
        [Authorize("Bearer")]
        [SwaggerOperation(
            Summary = "Find person with pagination search, by your first name",
            Description = "Requires authentication",
            OperationId = "FindByNamePaginated",
            Tags = new[] { "Person" }
        )]
        [SwaggerResponse(200, "A person were find", typeof(List<PersonVO>))]
        [SwaggerResponse(400, "The request was incorrect")]
        [SwaggerResponse(500, "Oops! Some internal error has occurred")]
        [SwaggerResponse(401, "Oops! Some error has occurred with your authentication")]
        [SwaggerResponse(404, "Oops! we don't find what you need, sorry!")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get([FromQuery] string firstName, string sortDirection, int pageSize, int page)
        {
            return new OkObjectResult(_personService.PagedSearch(firstName, sortDirection, pageSize, page));
        }

        // POST api/values
        [HttpPost]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] PersonVO person)
        {
            if (person == null)
            {
                return BadRequest();
            }

            return new ObjectResult(_personService.Create(person));
        }

        // PUT api/values/5
        [HttpPut]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] PersonVO person)
        {
            if (person == null)
            {
                return BadRequest();
            }

            return new ObjectResult(_personService.Update(person));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete(int id)
        {
            _personService.Delete(id);

            return NoContent();
        }
    }
}
