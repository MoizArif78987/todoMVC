using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using todo_app.Models;

namespace todo_app.Controllers
{
    public class AddTodoController : Controller
    {
        private readonly ITodoService _context;

        public AddTodoController(ITodoService context)
        {
            _context = context;
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string Title, string Description, bool IsCompleted)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Todo todoItem = new Todo
            {
                Title = Title,
                Description = Description,
                IsCompleted = IsCompleted,
                UserId = userId
            };

            await _context.AddTodoAsync(todoItem);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todos = await _context.GetAllTodosAsync(userId);
            return View(todos);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            await _context.DeleteTodoAsync(Id);
            return RedirectToAction("Index");
        }
    }
}
