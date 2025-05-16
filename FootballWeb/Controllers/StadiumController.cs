using FootballWeb.Models;
using Microsoft.AspNetCore.Mvc;

public class StadiumController : Controller
{
    private static List<Stadium> stadiums = new List<Stadium>
    {
        new Stadium
        {
            Id = 1,
            Name = "Emirates Stadium",
            TeamName = "Arsenal",
            Location = "London",
            Capacity = 60260,
            FoundedYear = 2006,
            ImageUrl = "/images/emirates.jpg",
            TeamLogoUrl = "/images/arsenal-logo.png",
            Description = "Emirates là sân nhà của Arsenal từ năm 2006. Sân có kiến trúc hiện đại, sức chứa hơn 60,000 chỗ ngồi.",
            MapEmbedUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d19801.91668463271!2d-0.122228!3d51.554888!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x48761b450a6e7c59%3A0x281d0ee0d2dcac36!2sEmirates+Stadium!5e0!3m2!1sen!2suk!4v1616784113882!5m2!1sen!2suk\r\n"
        },
        new Stadium
        {
            Id = 2,
            Name = "Old Trafford",
            TeamName = "Manchester United",
            Location = "Manchester",
            Capacity = 74879,
            FoundedYear = 1910,
            ImageUrl = "/images/oldtrafford.jpg",
            TeamLogoUrl = "/images/mu-logo.png",
            Description = "Old Trafford là sân đấu nổi tiếng của Manchester United, được mệnh danh là 'Nhà hát của những giấc mơ'.",
            MapEmbedUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2373.6234137606337!2d-2.2922920841791923!3d53.46305898000296!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x487bae116dba92a3%3A0x3700f67f3edfc3b3!2sOld+Trafford!5e0!3m2!1sen!2suk!4v1616784340352!5m2!1sen!2suk\r\n"
        },
        // Thêm các sân khác...
    };

    public IActionResult Index()
    {
        return View("Index", stadiums);
    }

    public IActionResult Details(int id)
    {
        var stadium = stadiums.FirstOrDefault(s => s.Id == id);
        if (stadium == null) return NotFound();
        return View("stadiumdetails", stadium);
    }
}
