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
        //TODO: check if id already exist
        [HttpPost]
        public async Task<ActionResult<List<Todos>>> AddTodoPost(Todos todo)
        { 
            posts.Add(todo);
            return Ok(posts);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Todos>>> DeleteTodoPost(int id)
        {
            var todo = posts.Find(x => x.Id == id);
            if (todo is null)
            {
                return NotFound("no post with that id was found");
            }
            posts.Remove(todo);
            return Ok(posts);
        }
    }
}
