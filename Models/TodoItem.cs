using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
	public class TodoItem
	{
		[Required]
		public required long Id { get; set; }
		[Required]
		public required string Name { get; set; }
		[Required]
		public required bool IsComplete { get; set; }
	}
}
