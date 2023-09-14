using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Models;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {

        private static List<Todos> tickets = new List<Todos>
            {
                new Models.Todos {
                    Id = 1,
                    Title = "Vakna",
                    IsActive = true
                },
                new Models.Todos {
                    Id = 2,
                    Title = "Duscha",
                    IsActive = false
                }
            };

        [HttpGet]
        public async Task<ActionResult<List<Todos>>> GetAllTodoTickets()
        {
            return Ok(tickets);
        }
    }
}
