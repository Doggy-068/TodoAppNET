using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Utils;

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
		[Authorize(policy: Permission.User)]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TodoItemReturnDTO>>> GetTodoItems()
		{
			if (_context.TodoItems == null)
			{
				return NotFound();
			}
			return await _context.TodoItems.Select(x => ItemToReturnDTO(x)).ToListAsync();
		}

		/// <summary>
		/// Create a TodoItem.
		/// </summary>
		/// <param name="todoItemCreateDTO"></param>
		/// <returns>A newly created TodoItem</returns>
		/// <remarks>
		///	Sample request:
		///	
		///		POST /api/TodoItems
		///		{
		///			"name": "Item #1",
		///			"isComplete": true
		///		}
		/// </remarks>
		/// <response code="201">Returns the newly created item.</response>
		[Authorize(policy: Permission.Root)]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<TodoItemReturnDTO>> PostTodoItem(TodoItemCreateDTO todoItemCreateDTO)
		{
			if (_context.TodoItems == null)
			{
				return Problem("Entity set 'TodoContext.TodoItems'  is null.");
			}
			TodoItem todoItem = new()
			{
				Id = 0,
				Name = todoItemCreateDTO.Name,
				IsComplete = todoItemCreateDTO.IsComplete
			};
			_context.TodoItems.Add(todoItem);
			await _context.SaveChangesAsync();
			return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
		}

		/// <summary>
		/// Find a specific TodoItem.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Authorize(policy: Permission.User)]
		[HttpGet("{id}")]
		public async Task<ActionResult<TodoItemReturnDTO>> GetTodoItem(long id)
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
			return ItemToReturnDTO(todoItem);
		}

		/// <summary>
		/// Update a specific TodoItem.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="todoItemCreateDTO"></param>
		/// <returns></returns>
		[Authorize(policy: Permission.Admin)]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTodoItem(long id, TodoItemCreateDTO todoItemCreateDTO)
		{
			TodoItem todoItem = new()
			{
				Id = id,
				Name = todoItemCreateDTO.Name,
				IsComplete = todoItemCreateDTO.IsComplete
			};
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
		/// <response code="204">Delete item successfully.</response>
		[Authorize(policy: Permission.Root)]
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
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

		private static TodoItemReturnDTO ItemToReturnDTO(TodoItem todoItem) =>
			new()
			{
				Id = todoItem.Id,
				Name = todoItem.Name,
				IsComplete = todoItem.IsComplete
			};
	}
}
