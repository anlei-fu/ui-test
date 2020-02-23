using System.Collections.Generic;

namespace UiTets
{
    public class Project
    {
        public string Folder { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Test> Tests { get; set; } = new List<Test>();
    }
}
