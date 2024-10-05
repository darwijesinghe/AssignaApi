using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static AssignaApi.Helpers.Helper;

namespace AssignaApi.Models
{
#pragma warning disable IDE1006 // Naming Styles
    // data table
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tsk_id { get; set; }
        [MaxLength(50)]
        public string tsk_title { get; set; }
        public DateTime deadline { get; set; }
        [MaxLength(250)]
        public string tsk_note { get; set; }
        public bool pending { get; set; }
        public bool complete { get; set; }
        public bool pri_high { get; set; }
        public bool pri_medium { get; set; }
        public bool pri_low { get; set; }
        [MaxLength(250)]
        public string user_note { get; set; } = string.Empty;
        public DateTime insertdate { get; set; }

        // relationships
        public int cat_id { get; set; }
        [JsonIgnore]
        [ForeignKey("cat_id")]
        public Category category { get; set; }

        public int user_id { get; set; }
        [JsonIgnore]
        [ForeignKey("user_id")]
        public Users users { get; set; }

    }

#pragma warning restore IDE1006 // Naming Styles

    // data transferring model -> DTO
    public class TaskDto
    {
        public int TskId { get; set; }
        public string TskTitle { get; set; }
        public DateTime Deadline { get; set; }
        public string TskNote { get; set; }
        public bool Pending { get; set; }
        public bool Complete { get; set; }
        public bool PriHigh { get; set; }
        public bool PriMedium { get; set; }
        public bool PriLow { get; set; }
        public string UserNote { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? UserMail { get; set; }
        public string? CatName { get; set; }
        public int CatId { get; set; }
        public int UserId { get; set; }

    }

    // add a new task
    public class NewTask
    {
        [MaxLength(50, ErrorMessage = "Max length is 50 characters")]
        [Required(ErrorMessage = "Task title is required")]
        public string TskTitle { get; set; }
        [Required(ErrorMessage = "Task category is required")]
        public int TskCategory { get; set; }
        [Required(ErrorMessage = "Task due date is required")]
        [FutureDate(ErrorMessage ="Task due date should be future date")]
        public DateTime Deadline { get; set; }
        [Required(ErrorMessage = "Task priority is required")]
        public string Priority { get; set; }
        [Required(ErrorMessage = "Task assignee is required")]
        public int Member { get; set; }
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        [Required(ErrorMessage = "Task note is required")]
        public string TskNote { get; set; }

    }

    // edit a task
    public class EditTask
    {
        [Required(ErrorMessage = "Task id is required")]
        public int TskId { get; set; }
        [MaxLength(50, ErrorMessage = "Max length is 50 characters")]
        [Required(ErrorMessage = "Task title is required")]
        public string TskTitle { get; set; }
        [Required(ErrorMessage = "Task category is required")]
        public int TskCategory { get; set; }
        [Required(ErrorMessage = "Task due date is required")]
        [FutureDate(ErrorMessage = "Task due date should be future date")]
        public DateTime Deadline { get; set; }
        [Required(ErrorMessage = "Task priority is required")]
        public string Priority { get; set; }
        [Required(ErrorMessage = "Task assignee is required")]
        public int Member { get; set; }
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        [Required(ErrorMessage = "Task note is required")]
        public string TskNote { get; set; }

    }

    // task remind
    public class Reminder
    {
        [Required(ErrorMessage = "Task id is required")]
        public int TskId { get; set; }
        [Required(ErrorMessage = "Email message is required")]
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        public string Message { get; set; }
    }

    // add note to task
    public class AddNote
    {
        [Required(ErrorMessage = "Task id is required")]
        public int TskId { get; set; }
        [Required(ErrorMessage = "Task note is required")]
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        public string UserNote { get; set; }
    }

    // delete a task
    public class DeleteTask
    {
        [Required(ErrorMessage = "Task id is required")]
        public int TskId { get; set; }
    }

    // mark as done
    public class MarkDone
    {
        [Required(ErrorMessage = "Task id is required")]
        public int TskId { get; set; }
    }
}