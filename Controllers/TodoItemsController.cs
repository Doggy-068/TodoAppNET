using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Produces("application/json")]
	public class TodoItemsController : ControllerBase
	{
		private readonly TodoContext _context;

		public TodoItemsController(TodoContext context)
		{
			_context = context;
		}
		
		/// <summary>
		/// Find all TodoItems.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
		{
			if (_context.TodoItems == null)
			{
				return NotFound();
			}
			return await _context.TodoItems.ToListAsync();
		}

		/// <summary>
		/// Create a TodoItem.
		/// </summary>
		/// <param name="todoItem"></param>
		/// <returns>A newly created TodoItem</returns>
		/// <remarks>
		///	Sample request:
		///	
		///		POST /api/TodoItems
		///		{
		///			"id": 1,
		///			"name": "Item #1",
		///			"isComplete": true
		///		}
		/// </remarks>
		[HttpPost]
		public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
		{
			if (_context.TodoItems == null)
			{
				return Problem("Entity set 'TodoContext.TodoItems'  is null.");
			}
			_context.TodoItems.Add(todoItem);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
		}

		/// <summary>
		/// Find a specific TodoItem.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
		{
			if (_context.TodoItems == null)
			{
				return NotFound();
			}
			var todoItem = await _context.TodoItems.FindAsync(id);

			if (todoItem == null)
			{
				return NotFound();
			}

			return todoItem;
		}

		/// <summary>
		/// Update a specific TodoItem.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="todoItem"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
		{
			if (id != todoItem.Id)
			{
				return BadRequest();
			}

			_context.Entry(todoItem).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TodoItemExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		/// <summary>
		/// Delete a specific TodoItem.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTodoItem(long id)
		{
			if (_context.TodoItems == null)
			{
				return NotFound();
			}
			var todoItem = await _context.TodoItems.FindAsync(id);
			if (todoItem == null)
			{
				return NotFound();
			}

			_context.TodoItems.Remove(todoItem);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool TodoItemExists(long id)
		{
			return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
