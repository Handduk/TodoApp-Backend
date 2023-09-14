using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Models;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {

        private static List<Todos> posts = new List<Todos>
            {
                new Todos {
                    Id = 1,
                    Title = "Vakna",
                    IsActive = true
                },
                new Todos {
                    Id = 2,
                    Title = "Duscha",
                    IsActive = false
                }
            };

        [HttpGet]
        public async Task<ActionResult<List<Todos>>> GetAllTodoPosts()
        {
            return Ok(posts);
        }

        [HttpPost]
        public async Task<ActionResult<List<Todos>>> AddTodoPost(Todos todo)
        {
            posts.Add(todo);
            return Ok(posts);
        }
    }
}
