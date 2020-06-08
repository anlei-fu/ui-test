using Newtonsoft.Json;

namespace UiTest
{
    public  class Point
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        public System.Drawing.Point Cast()
        {
            return new System.Drawing.Point(X, Y);
        }
    }
}
