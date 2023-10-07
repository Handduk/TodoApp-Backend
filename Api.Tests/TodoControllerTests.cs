using Moq;
using Infrastructure.Repositories;
using Api.Controllers;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Entity;
using Microsoft.AspNetCore.Mvc;
using API.Dtos;

namespace Api.Tests
{
    public class TodoControllerTests
    {
        private readonly TodosController _controller;
        private readonly Mock<ITodoRepository> _mockRepo = new Mock<ITodoRepository>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>(); 
        private readonly Mock<ILogger<TodosController>> _mockLogger = new Mock<ILogger<TodosController>>();

        public TodoControllerTests()
        {
            _controller = new TodosController(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetTodo_ShouldReturnOkwithTodos()
        {
            //Arrange:
            var todos = new List<Todo>
            {
                new Todo { Id = 1, Title = "Test1", Completed = true },
                new Todo { Id = 2, Title = "Test2", Completed = false }
            };

            var todoDtos = new List<TodoDto>
            {
                new TodoDto { Id = 1, Title = "Test1", Completed = true },
                new TodoDto { Id = 2, Title = "Test2", Completed = false }
            };

            _mockRepo.Setup(repo => repo.GetTodos()).ReturnsAsync(todos);
            _mockMapper.Setup(mapper => mapper.Map<List<TodoDto>>(todos)).Returns(todoDtos);

            //Act:
            var result = await _controller.GetAllTodos();

            //Assert:

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTodos = Assert.IsAssignableFrom<List<TodoDto>>(okObjectResult.Value);

            Assert.Equal(todoDtos, returnedTodos);

            _mockRepo.Verify(repo => repo.GetTodos(), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<List<TodoDto>>(todos), Times.Once);
        }

        [Fact]
        public async Task GetTodoWithId_ShouldReturnOk()
        {
            //Arrange:
            int id = 1;
            var todo = new Todo
            { 
                Id = 1, 
                Title = "Test", 
                Completed = true 
            };

            var todoDto = new TodoDto { Id = 1, Title = "Test", Completed = true };
            _mockRepo.Setup(repo => repo.GetTodo(id)).ReturnsAsync(todo);
            _mockMapper.Setup(mapper => mapper.Map<TodoDto>(todo)).Returns(todoDto);

            //Act:
            var result = await _controller.GetTodo(id);

            //Assert:

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var todoDtos = Assert.IsType<TodoDto>(okResult.Value);
            Assert.Equal(id, todoDto.Id);
            Assert.Equal("Test", todoDto.Title);

            _mockRepo.Verify(repo => repo.GetTodo(id), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<TodoDto>(todo), Times.Once);
        }

        [Fact]
        public async Task PostTodo_ReturnsCreatedAtAction()
        {
            //Arrange:
            var todo = new Todo
            {
                Id = 1,
                Title = "Test1",
                Completed = true
            };

            _mockRepo.Setup(repo => repo.AddTodo(It.IsAny<Todo>()))
                .ReturnsAsync(todo);

            //Act:
            var result = await _controller.PostTodo(todo);

            //Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdTodo = Assert.IsType<Todo>(createdAtActionResult.Value);

            Assert.Equal("GetTodo", createdAtActionResult.ActionName);
            Assert.Equal(todo.Id, createdTodo.Id);
            Assert.Equal(201, createdAtActionResult.StatusCode);

            _mockRepo.Verify(repo => repo.AddTodo(todo), Times.Once);
        }

        [Fact]

        public async Task DeleteTodo_ShouldReturnNoContent()
        {
            //Arrange:
            int id = 1;
            var todo = new Todo 
            {
                Id = 1, 
                Title = "Test", 
                Completed = true 
            };

            _mockRepo.Setup(repo => repo.RemoveTodo(id)).ReturnsAsync(todo);

            //Act:
            var result = await _controller.DeleteTodo(id);

            //Assert:
            Assert.IsType<NoContentResult>(result.Result);

            _mockRepo.Verify(repo => repo.RemoveTodo(id), Times.Once);
        }

        [Fact]

        public async Task PutTodo_ShouldReturnOk()
        {
            //Arrange:
            int id = 1;
            var updatedTodo = new Todo { Id = 1, Title = "TestUpdate", Completed = true };

            _mockRepo.Setup(repo => repo.UpdateTodo(id, updatedTodo)).ReturnsAsync(updatedTodo);

            //Act:
            var result = await _controller.PutTodo(id, updatedTodo);

            //Assert:
            Assert.IsType<OkObjectResult>(result.Result);

            _mockRepo.Verify(repo => repo.UpdateTodo(id, updatedTodo), Times.Once);
        }
    }
}