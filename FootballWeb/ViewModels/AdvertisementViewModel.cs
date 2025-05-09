using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace FootballWeb.ViewModels
{
    public class AdvertisementViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }
    }
}
