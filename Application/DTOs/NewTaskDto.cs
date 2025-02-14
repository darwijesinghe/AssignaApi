using System;
using System.ComponentModel.DataAnnotations;
using static Application.Helpers.FilterAttributes;

namespace Application.DTOs
{
    /// <summary>
    /// DTO for new task.
    /// </summary>
    public class NewTaskDto
    {
        /// <summary>
        /// Task title
        /// </summary>
        [MaxLength(50, ErrorMessage = "Max length is 50 characters")]
        [Required(ErrorMessage = "Task title is required")]
        public string TaskTitle  { get; set; }

        /// <summary>
        /// Task category
        /// </summary>
        [Required(ErrorMessage = "Task category is required")]
        public int TaskCategory  { get; set; }

        /// <summary>
        /// Task deadline
        /// </summary>
        [Required(ErrorMessage = "Task due date is required")]
        [FutureDate(ErrorMessage = "Task due date should be future date")]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Task priority
        /// </summary>
        [Required(ErrorMessage = "Task priority is required")]
        public string Priority   { get; set; }

        /// <summary>
        /// Task assignee
        /// </summary>
        [Required(ErrorMessage = "Task assignee is required")]
        public int Member        { get; set; }

        /// <summary>
        /// Task note
        /// </summary>
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        [Required(ErrorMessage = "Task note is required")]
        public string TaskNote   { get; set; }
    }
}
