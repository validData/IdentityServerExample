namespace AuthServer.Model
{
    public class PermissionResponse
    {
        public string UserId { get; set; }
        public string ClientId { get; set; }
        public Permission[] Permissions { get; set; }
    }
}