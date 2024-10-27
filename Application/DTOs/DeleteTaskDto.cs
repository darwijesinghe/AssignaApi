using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    /// <summary>
    /// DTO for delete task
    /// </summary>
    public class DeleteTaskDto
    {
        /// <summary>
        /// Task ID
        /// </summary>
        [Required(ErrorMessage = "Task id is required")]
        public int TaskId { get; set; }
    }
}
