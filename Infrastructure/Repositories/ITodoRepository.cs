using Entity;

namespace Infrastructure.Repositories
{
    public interface ITodoRepository
    {
        Task<Todo> AddTodo(Todo todo);
        Task <Todo> GetTodo(int id);
        Task <List<Todo>> GetTodo();
        Task<Todo> RemoveTodo(int id);
        Task <Todo> StatusUpdate(int id);
    }
}
