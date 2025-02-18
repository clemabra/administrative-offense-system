namespace src.Models.Wz
{
    public class WzEntity
    {
        public int Id { get; set; }
        public int RowVersion { get; set; }
        public string Fallnummer { get; set; } = string.Empty;
        public string Weiserzeichen { get; set; } = string.Empty;
    }
}

