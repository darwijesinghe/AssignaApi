using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AssignaApi.Models
{
    // data table
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tsk_id { get; set; }
        [MaxLength(50)]
        public string tsk_title { get; set; } = null!;
        public DateTime deadline { get; set; }
        [MaxLength(250)]
        public string tsk_note { get; set; } = null!;
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
        public Category category { get; set; } = null!;

        public int user_id { get; set; }
        [JsonIgnore]
        [ForeignKey("user_id")]
        public Users users { get; set; } = null!;
    }

    // data transferring model -> DTO
    public class TaskDto
    {
        public int tsk_id { get; set; }
        public string tsk_title { get; set; } = null!;
        public DateTime deadline { get; set; }
        public string tsk_note { get; set; } = null!;
        public bool pending { get; set; }
        public bool complete { get; set; }
        public bool pri_high { get; set; }
        public bool pri_medium { get; set; }
        public bool pri_low { get; set; }
        public string user_note { get; set; } = null!;
        public string? user_name { get; set; }
        public string? first_name { get; set; }
        public string? user_mail { get; set; }
        public string? cat_name { get; set; }
        public int cat_id { get; set; }
        public int user_id { get; set; }

    }

    // add a new task
    public class NewTask
    {
        [MaxLength(50, ErrorMessage = "Max length is 50 characters")]
        [Required(ErrorMessage = "Task title is required")]
        public string tsk_title { get; set; } = null!;
        [Required(ErrorMessage = "Task category is required")]
        public int tsk_category { get; set; }
        [Required(ErrorMessage = "Task due date is required")]
        [RegularExpression(@"^\d\d\d\d-\d\d-\d\d$", ErrorMessage = "Date format is not valid, valid format is yyyy-MM-dd")]
        public DateTime deadline { get; set; }
        [Required(ErrorMessage = "Task priority is required")]
        public string priority { get; set; } = null!;
        [Required(ErrorMessage = "Task assignee is required")]
        public int member { get; set; }
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        [Required(ErrorMessage = "Task note is required")]
        public string tsk_note { get; set; } = null!;

    }

    // add a new task
    public class EditTask
    {
        [Required(ErrorMessage = "Task id is required")]
        public int tsk_id { get; set; }
        [MaxLength(50, ErrorMessage = "Max length is 50 characters")]
        [Required(ErrorMessage = "Task title is required")]
        public string tsk_title { get; set; } = null!;
        [Required(ErrorMessage = "Task category is required")]
        public int tsk_category { get; set; }
        [Required(ErrorMessage = "Task due date is required")]
        [RegularExpression(@"^\d\d\d\d-\d\d-\d\d$", ErrorMessage = "Date format is not valid, valid format is yyyy-MM-dd")]
        public DateTime deadline { get; set; }
        [Required(ErrorMessage = "Task priority is required")]
        public string priority { get; set; } = null!;
        [Required(ErrorMessage = "Task assignee is required")]
        public int member { get; set; }
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        [Required(ErrorMessage = "Task note is required")]
        public string tsk_note { get; set; } = null!;

    }

    // task remind
    public class Reminder
    {
        [Required(ErrorMessage = "Task id is required")]
        public int tsk_id { get; set; }
        [Required(ErrorMessage = "Email message is required")]
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        public string message { get; set; } = null!;
    }

    // add note to task
    public class AddNote
    {
        [Required(ErrorMessage = "Task id is required")]
        public int tsk_id { get; set; }
        [Required(ErrorMessage = "Task note is required")]
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        public string user_note { get; set; } = null!;
    }

    // delete a task
    public class DeleteTask
    {
        [Required(ErrorMessage = "Task id is required")]
        public int tsk_id { get; set; }
    }

    // mark as done
    public class MarkDone
    {
        [Required(ErrorMessage = "Task id is required")]
        public int tsk_id { get; set; }
    }
}