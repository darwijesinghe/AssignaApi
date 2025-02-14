namespace Application.Response
{
    /// <summary>
    /// Response class for the methods.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Indicates operation is done or failed
        /// </summary>
        public bool Success    { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string? Message { get; set; }

        // Extra fields --------------------
        public int Id          { get; set; }
    }

    /// <summary>
    /// Generic response class
    /// </summary>
    public class Result<T> where T : class
    {
        /// <summary>
        /// Indicates operation is done or failed
        /// </summary>
        public bool Success    { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string? Message { get; set; }

        // Extra fields --------------------
        public int Id          { get; set; }
        public T? Data         { get; set; }
    }
}
