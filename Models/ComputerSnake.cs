namespace HelloWorld.Models{

    public class ComputerSnake
    {
      public int computer_id { get; set; }
      public string motherboard { get; set; } = "";
      public int? cpu_cores { get; set; } = 0;
      public bool has_wifi { get; set; }
      public bool has_lte { get; set; }
      public DateTime? release_date { get; set;}
      public decimal price { get; set; }
      public string video_card { get; set; } = "";

    }
}

// primary key added as a field on the model, for use as key in entity framework
// only string are nullable fields by default
// EF does not like mapping a null value back into a model