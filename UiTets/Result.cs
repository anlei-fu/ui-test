namespace UiTest
{
    public class Result
    {
        public Result(string snapshot, string description)
        {
            Snapshot = snapshot;
            Description = description;
        }

        public string Snapshot { get; }
        public string Description { get;}
    }
}
