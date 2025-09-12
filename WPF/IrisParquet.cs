using System.Text.Json.Serialization;

namespace WPFExample
{
    internal class IrisParquet
    {
        [JsonPropertyName("sepal.length")]
        public float SepalLength { get; set; }

        [JsonPropertyName("sepal.width")]
        public float SepalWidth { get; set; }

        [JsonPropertyName("petal.length")]
        public float PetalLength { get; set; }

        [JsonPropertyName("petal.width")]
        public float PetalWidth { get; set; }

        [JsonPropertyName("variety")]
        public string Variety { get; set; } = "";
    }
}
