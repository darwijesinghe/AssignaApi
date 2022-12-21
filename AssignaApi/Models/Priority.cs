using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssignaApi.Models
{
    // data table
    public class Priority
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pri_id { get; set; }
        public string pri_name { get; set; } = null!;
        public DateTime insertdate { get; set; }

    }

    // priority levels
    public static class PriLevels
    {
        public const string high = "High";
        public const string medium = "Medium";
        public const string low = "Low";
    }

    // data transferring model -> DTO
    public class PriorityDto
    {
        public int pri_id { get; set; }
        public string pri_name { get; set; } = null!;
    }
}