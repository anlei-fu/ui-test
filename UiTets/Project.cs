using System;
using System.Collections.Generic;

namespace UiTest
{
    public class Project
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastModifyTime { get; set; }

        public List<Command> Commands { get; set; }

        public List<Project> SubProjects { get; set; }
    }
}
