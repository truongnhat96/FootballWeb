namespace FootballWeb.Models
{
    public class Stadium
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TeamName { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public int FoundedYear { get; set; }
        public string ImageUrl { get; set; } // logo đội
        public string Description { get; set; } // mô tả
        public string TeamLogoUrl { get; set; }  // logo đội bóng
        public string MapEmbedUrl { get; set; }  // nếu bạn muốn nhúng Google Maps
    }
}