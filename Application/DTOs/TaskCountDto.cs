namespace Application.DTOs
{
    /// <summary>
    /// DTO for task count.
    /// </summary>
    public class TaskCountDto
    {
        /// <summary>
        /// All task count.
        /// </summary>
        public int AllTask        { get; set; }

        /// <summary>
        /// Pending task count.
        /// </summary>
        public int Pending        { get; set; }

        /// <summary>
        /// Complete task count.
        /// </summary>
        public int Complete       { get; set; }

        /// <summary>
        /// High priority task count.
        /// </summary>
        public int HighPriority   { get; set; }

        /// <summary>
        /// Medium priority task count.
        /// </summary>
        public int MediumPriority { get; set; }

        /// <summary>
        /// Low priority task count.
        /// </summary>
        public int LowPriority    { get; set; }
    }
}
