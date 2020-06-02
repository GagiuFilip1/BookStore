using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Common.Requests.Commons;
using BookStore.Core.Models.Entities;
using BookStore.Core.Models.Filters;
using BookStore.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookStore.Presentation.Controllers.Implementations
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> m_logger;
        private readonly IBookService m_bookService;

        public BookController(ILogger<BookController> logger, IBookService bookService)
        {
            m_logger = logger;
            m_bookService = bookService;
        }

        [HttpPost]
        [Route("get")]
        public async Task<ActionResult<Tuple<int, List<Book>>>> GetBooksAsync([FromBody] SearchRequest<Book, BookFilter> request)
        {
            var response = await m_bookService.SearchBooksAsync(request);
            return response.Data;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> RegisterBookAsync([FromBody] Book request)
        {
            var response = await m_bookService.RegisterBookAsync(request);
            return Guid.Parse(response.Message);
        }

        [HttpPut]
        public async Task<ActionResult<Guid>> UpdateBookAsync([FromBody] Book request)
        {
            var response = await m_bookService.UpdateBookAsync(request);
            return Guid.Parse(response.Message);
        }

        [HttpDelete]
        public async Task<ActionResult<Guid>> DeleteBookAsync([FromQuery(Name="id")] string idBody)
        {
            Guid.TryParse(idBody, out var id);
            var response = await m_bookService.DeleteBookAsync(id);
            return Guid.Parse(response.Message);
        }
    }
}