namespace AuthServer.Model
{
    public class Permission
    {
        public Permission(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}