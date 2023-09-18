using Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoContext _context;

    public TodoRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task<Todo> AddTodo(Todo todo)
    {
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return todo;
    }

    public async Task<Todo> GetTodo(int id)
    {
        return await _context.Todos.FindAsync(id);
    }

    public Task<List<Todo>> GetTodo()
    {
        return _context.Todos
            .OrderBy(x => x.Title)
            .ToListAsync();
    }

    public async Task<List<Todo>> RemoveTodo(int id)
    {
        var selectedTodo = await _context.Todos.FindAsync(id);
        _context.Todos.Remove(selectedTodo);

        await _context.SaveChangesAsync();
        return _context.Todos
            .OrderBy(x => x.Id)
            .ToList();

    }
}