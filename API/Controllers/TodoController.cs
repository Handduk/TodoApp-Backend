using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {

        private static List<Todo> posts = new List<Todo>
            {
                new Todo {
                    Id = 1,
                    Title = "Vakna",
                    IsActive = true
                },
                new Todo {
                    Id = 2,
                    Title = "Duscha",
                    IsActive = false
                }
            };

        [HttpGet]
        public async Task<ActionResult<List<Todo>>> GetAllTodoPosts()
        {
            return Ok(posts);
        }
        //TODO: check if id already exist
        [HttpPost]
        public async Task<ActionResult<List<Todo>>> AddTodoPost(Todo todo)
        { 
            posts.Add(todo);
            return Ok(posts);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Todo>>> DeleteTodoPost(int id)
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
