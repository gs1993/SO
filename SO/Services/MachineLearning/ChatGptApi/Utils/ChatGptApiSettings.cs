namespace ChatGptApi.Utils
{
    public sealed record ChatGptApiSettings
    {
        public string ApiKey { get; set; }
        public string Url { get; set; }
        public string ModelName { get; set; }
        public string Role { get; set; }
    }
}