using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
	public class TodoItem
	{
		public required long Id { get; set; }
		public required string Name { get; set; }
		public required bool IsComplete { get; set; }
	}

	public class TodoItemReturnDTO
	{
		[Required]
		public required long Id { get; set; }
		[Required]
		public required string Name { get; set; }
		[Required]
		public required bool IsComplete { get; set; }
	}

	public class TodoItemCreateDTO
	{
		[Required]
		public required string Name { get; set; }
		[Required]
		public required bool IsComplete { get; set; }
	}
}
