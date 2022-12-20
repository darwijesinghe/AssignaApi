using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        // relationships
        [JsonIgnore]
        public List<Task>? task { get; set; }
    }

    // priority levels
    public static class PriLevels
    {
        public const string high = "High Priority";
        public const string medium = "Medium Priority";
        public const string low = "Low Priority";
    }

    // data transferring model -> DTO
    public class PriorityDto
    {
        public int pri_id { get; set; }
        public string pri_name { get; set; } = null!;
    }
}