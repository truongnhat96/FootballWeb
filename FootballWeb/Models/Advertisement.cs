using System.ComponentModel.DataAnnotations;
namespace FootballWeb.Models

{
    public class Advertisement
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public bool IsApproved { get; set; } = false;

        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }
    }
}
