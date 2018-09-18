using AspNetCoreWithDocker.Business.Rules.Book;
using AspNetCoreWithDocker.DataTransformers.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWithDocker.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBooksBusiness _booksService;

        public BooksController(IBooksBusiness booksService)
        {
            _booksService = booksService;
        }

        // GET api/values
        [HttpGet]
        public IActionResult FindAll()
        {
            var result = _booksService.FindAll();
            if (result != null && result.Count > 0) return Ok(result);

            return NoContent();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult FindById(int id)
        {
            var result = _booksService.FindById(id);
            if (result != null) return Ok(result);

            return NoContent();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] BooksVO book)
        {
            if (book != null)
            {
                var result = _booksService.Create(book);
                if (result != null) return Created("table BooksVO", book);
            }

            return BadRequest();
        }

        // PUT api/values/5
        [HttpPut]
        public IActionResult Put([FromBody] BooksVO book)
        {
            if (book != null)
            {
                var result = _booksService.Update(book);
                if (result != null) return Ok(book);
            }

            return BadRequest();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _booksService.Delete(id);
            return Ok();
        }
    }
}
