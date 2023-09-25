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
        private readonly ILogger<TodosController> _logger;

        public TodosController(ITodoRepository todoRepository, 
            IMapper mapper, 
            ILogger<TodosController> logger)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
            _logger = logger;
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
            if (todo is null)
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

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<TodoDto>> DeleteTodo(int id)
        {
            try
            {
                var selectedTodo = await _todoRepository.RemoveTodo(id);
                if (selectedTodo == null)
                {
                    _logger.LogWarning("Todo with id {id} was not found", id);
                    return NotFound();
                }
                _logger.LogInformation("Todo with id {id} was deleted", id);
                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BingBong Error while trying to delete todo with id {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<TodoDto>> UpdateTodoStatus(int id)
        {
            var selectedTodo = await _todoRepository.StatusUpdate(id);
            if (selectedTodo is null)
            {
                return NotFound();
            }
            var todoDto = _mapper.Map<TodoDto>(selectedTodo);
            return todoDto;
        }

        [HttpPut]
        public async Task<ActionResult<TodoDto>> UpdateTodoTitle(int id, Todo todo)
        {
            var selectedTodo = await _todoRepository.TitleUpdate(id, todo);
            if (selectedTodo is null)
            {
                return NotFound();
            }
            var todoDto = _mapper.Map<TodoDto>(selectedTodo);
            return todoDto;
        }

    }
}
