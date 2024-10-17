using Microsoft.EntityFrameworkCore;
using todo_app.Models;

public interface ITodoService
{
    Task<IEnumerable<Todo>> GetAllTodosAsync(string userId);
    Task<Todo> GetTodoByIdAsync(int id);
    Task AddTodoAsync(Todo todo);
    Task DeleteTodoAsync(int id);
    Task CompleteAsync();
    Task Dispose();
}

public class TodoService : ITodoService
{
    private readonly TodoContext _context;

    public TodoService(TodoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Todo>> GetAllTodosAsync(string userId)
    {
        return await _context.Todos
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<Todo> GetTodoByIdAsync(int id)
    {
        return await _context.Todos.FindAsync(id);
    }

    public async Task AddTodoAsync(Todo todo)
    {
        await _context.Todos.AddAsync(todo);
        await CompleteAsync();
    }

    public async Task DeleteTodoAsync(int id)
    {
        var todo = await GetTodoByIdAsync(id);
        if (todo != null)
        {
            _context.Todos.Remove(todo);
            await CompleteAsync();
        }
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task Dispose()
    {
        _context.Dispose();
    }
}