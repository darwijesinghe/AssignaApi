using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    /// <summary>
    /// DTO for add task note
    /// </summary>
    public class AddNoteDto
    {
        /// <summary>
        /// Task ID
        /// </summary>
        [Required(ErrorMessage = "Task id is required")]
        public int TaskId      { get; set; }

        /// <summary>
        /// Task note
        /// </summary>
        [Required(ErrorMessage = "Task note is required")]
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        public string UserNote { get; set; }
    }
}
