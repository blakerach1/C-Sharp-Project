using System.Text.Json.Serialization;

namespace HelloWorld.Models{

    public class Computer
    {
      [JsonPropertyName("computer_id")]
      public int ComputerId { get; set; }
      [JsonPropertyName("motherboard")]
      public string Motherboard { get; set; } = "";
      [JsonPropertyName("cpu_cores")]
      public int? CPUCores { get; set; } = 0;
      [JsonPropertyName("has_wifi")]
      public bool HasWifi { get; set; }
      [JsonPropertyName("has_lte")]
      public bool HasLTE { get; set; }
      [JsonPropertyName("release_date")]
      public DateTime? ReleaseDate { get; set;}
      [JsonPropertyName("price")]
      public decimal Price { get; set; }
      [JsonPropertyName("video_card")]
      public string VideoCard { get; set; } = "";

    }
}

// primary key added as a field on the model, for use as key in entity framework
// only string are nullable fields by default
// EF does not like mapping a null value back into a model