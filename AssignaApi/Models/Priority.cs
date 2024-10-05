using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssignaApi.Models
{
#pragma warning disable IDE1006 // Naming Styles
    // data table
    public class Priority
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pri_id { get; set; }
        public string pri_name { get; set; }
        public DateTime insertdate { get; set; }

    }

#pragma warning restore IDE1006 // Naming Styles

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
        public int PriId { get; set; }
        public string PriName { get; set; }
    }
}