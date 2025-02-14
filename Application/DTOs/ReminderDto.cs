using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    /// <summary>
    /// DTO for task reminder.
    /// </summary>
    public class ReminderDto
    {
        /// <summary>
        /// Task ID
        /// </summary>
        [Required(ErrorMessage = "Task id is required")]
        public int TaskId     { get; set; }

        /// <summary>
        /// Remind message
        /// </summary>
        [Required(ErrorMessage = "Email message is required")]
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        public string Message { get; set; }
    }
}
