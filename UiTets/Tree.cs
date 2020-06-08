using System.Collections.Generic;

namespace UiTest
{
    public class Tree
    {
        public List<Tree> Children { get; set; } = new List<Tree>();

        public string Text { get; set; }

        public string Key { get; set; }

        public object Data { get; set; }
    }
}
