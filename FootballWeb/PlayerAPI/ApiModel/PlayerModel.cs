namespace FootballWeb.PlayerAPI.ApiModel
{
    public class PlayerModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int ShirtNo { get; set; } 
        public string Nationality { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; } = null;
        public string Team { get; set; } = string.Empty;
    }
}
