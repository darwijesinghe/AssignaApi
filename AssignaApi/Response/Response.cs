namespace AssignaApi.Response
{
    // response return class
#pragma warning disable IDE1006 // Naming Styles
    public class Result
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int id { get; set; }
    }

    public class AuthResult
    {
        public string user_name { get; set; }
        public string role { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public string refresh_token { get; set; }
        public string reset_token { get; set; }
    }

    public class Result<T> where T : class
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int id { get; set; }
        public T data { get; set; }
    }

    // google user info response class
    public class GoogleResponse
    {
        public string sub { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string picture { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        public string locale { get; set; }
        public string error { get; set; }
    }

#pragma warning restore IDE1006 // Naming Styles

}