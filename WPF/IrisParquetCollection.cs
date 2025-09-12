using System.Text.Json.Serialization;

namespace WPFExample
{
    internal class IrisParquetCollection
    {
        [JsonPropertyName("data")]
        public List<IrisParquet> Data { get; set; } = new List<IrisParquet>();
    }
}
