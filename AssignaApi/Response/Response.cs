namespace AssignaApi.Response
{
    // response return class
    public class Result
    {
        public bool success { get; set; }
        public string message { get; set; } = null!;
        public int id { get; set; }
    }

    public class AuthResult
    {
        public string uesr_name { get; set; } = null!;
        public string role { get; set; } = null!;
        public bool success { get; set; }
        public string message { get; set; } = null!;
        public string token { get; set; } = null!;
        public string refresh_token { get; set; } = null!;
        public string? reset_token { get; set; }
    }

    public class Result<T> where T : class
    {
        public bool success { get; set; }
        public string message { get; set; } = null!;
        public int id { get; set; }
        public T data { get; set; } = null!;
    }
}