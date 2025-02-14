using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    /// <summary>
    /// DTO for mark task as done.
    /// </summary>
    public class MarkDoneDto
    {
        /// <summary>
        /// Task ID
        /// </summary>
        [Required(ErrorMessage = "Task id is required")]
        public int TaskId { get; set; }
    }
}
