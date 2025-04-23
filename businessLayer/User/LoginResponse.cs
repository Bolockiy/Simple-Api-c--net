namespace toDoList.User
{
    public class LoginResponse
    {
        public string? UserName { get; set; }
        public string? AccessToken { get; set; } = null;
        public int ExpiresIn { get; set; }
        public bool isAdmin { get; set; }
    }
}
