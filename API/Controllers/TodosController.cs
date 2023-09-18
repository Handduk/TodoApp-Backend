using API.Dtos;
using AutoMapper;
using Entity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;

        public TodosController(ITodoRepository todoRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoDto>>> GetAllTodos()
            {
                var todos = await _todoRepository.GetTodo();
                var todoDtos = _mapper.Map<List<TodoDto>>(todos);
            return Ok(todoDtos);
            }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<TodoDto>> GetTodo(int id)
        {
            var todo = await _todoRepository.GetTodo(id);
            if (todo == null)
            {
                return NotFound();
            }
            var todoDto = _mapper.Map<TodoDto>(todo);
            return todoDto;

        }

        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            var createdTodo = await _todoRepository.AddTodo(todo);
            return CreatedAtAction("GetTodo", new { id = createdTodo.Id }, createdTodo);
        }

    }
}
