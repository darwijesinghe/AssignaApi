namespace Infrastructure.Configurations
{
    /// <summary>
    /// DB option class.
    /// </summary>
    public class DatabaseOptions
    {
        /// <summary>
        /// DB timeout
        /// </summary>
        public int CommandTimeout        { get; set; }

        /// <summary>
        /// Connection string
        /// </summary>
        public string ConnectionString   { get; set; }

        // Extra -------------------------------------
        public bool EnableDetailedErrors { get; set; }
        public int MaxRetryCount         { get; set; }
    }
}
