using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Classes
{
    /// <summary>
    /// Domain class for tasks priorities
    /// </summary>
    public class Priority
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PriorityId      { get; set; }

        /// <summary>
        /// Priority name
        /// </summary>
        public string PriorityName { get; set; }

        /// <summary>
        /// Inserted date
        /// </summary>
        public DateTime InsertDate { get; set; }

    }
}