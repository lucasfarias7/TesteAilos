using System.Text.Json.Serialization;

namespace Teste2.Model
{
    public class ApiResponse
    {
        [JsonPropertyName("page")]
        public int? Page { get; set; }
        [JsonPropertyName("per_page")]
        public int? PerPage { get; set; }
        [JsonPropertyName("total")]
        public int? Total { get; set; }
        [JsonPropertyName("total_pages")]
        public int? TotalPages { get; set; }
        [JsonPropertyName("data")]
        public Datas[] Data { get; set; }
    }
}
