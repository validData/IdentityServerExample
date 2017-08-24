using System;

namespace AuthServer.Model
{
    public class AppFeature
    {
        public AppFeature(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; set; }
    }
}