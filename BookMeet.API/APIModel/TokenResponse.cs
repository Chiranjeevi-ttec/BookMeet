namespace BookMeet.API.APIModel
{
    public class TokenResponse
    {
        public static string response { get; internal set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
    }
}
