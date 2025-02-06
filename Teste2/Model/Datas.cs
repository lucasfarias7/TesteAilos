﻿using System.Text.Json.Serialization;

namespace Teste2.Model
{
    public class Datas
    {
        [JsonPropertyName("competition")]
        public string? Competition { get; set; }
        [JsonPropertyName("year")]
        public int? Year { get; set; }
        [JsonPropertyName("round")]
        public string? Round { get; set; }
        [JsonPropertyName("team1")]
        public string? Team1 { get; set; }
        [JsonPropertyName("team2")]
        public string? Team2 { get; set; }
        [JsonPropertyName("team1goals")]
        public string? Team1Goals { get; set; }
        [JsonPropertyName("team2goals")]
        public string? Team2Goals { get; set; }
    }
}
