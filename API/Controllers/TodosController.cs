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
            try
            {
                var todos = await _todoRepository.GetTodos();
                var todoDtos = _mapper.Map<List<TodoDto>>(todos);
                _logger.LogInformation("Todos was loaded successfully");
                return Ok(todoDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to load todos");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<TodoDto>> GetTodo(int id)
        {
            try
            {
                var todo = await _todoRepository.GetTodo(id);
                if (todo is null)
                {
                    _logger.LogWarning("Todo with id {id} was not found", id);
                    return NotFound();
                }
                _logger.LogInformation("Todo with id {id} was found", id);  
                var todoDto = _mapper.Map<TodoDto>(todo);
                return Ok(todoDto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to load todo with id {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            try
            {
                var createdTodo = await _todoRepository.AddTodo(todo);
                _logger.LogInformation("Todo with id {id} was created", todo.Id);
                return CreatedAtAction("GetTodo", new { id = createdTodo.Id }, createdTodo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to post todo");
                return StatusCode(500, "Internal Server Error");
            }

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
                _logger.LogError(ex, "Error while trying to delete todo with id {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<TodoDto>> PutTodo(int id, Todo todo)
        {
            var selectedTodo = await _todoRepository.UpdateTodo(id, todo);
            if (selectedTodo is null)
            {
                _logger.LogWarning("Todo with id {id} was not updated", id);
                return BadRequest();
            }
            var todoDto = _mapper.Map<TodoDto>(selectedTodo);
            _logger.LogInformation("Todo with id {id} was updated", id);
            return Ok(todoDto);
        }

    }
}
