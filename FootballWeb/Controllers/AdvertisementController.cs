using FootballWeb.Models;
using FootballWeb.Repository;
using FootballWeb.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
namespace FootballWeb.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataContext _context;

        public AdvertisementController(IWebHostEnvironment env, DataContext context)
        {
            _env = env;
            _context = context;
        }

        // Gửi quảng cáo
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AdvertisementViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(fileStream);
                    }
                }

                var ad = new Advertisement
                {
                    Title = model.Title,
                    Description = model.Description,
                    ImagePath = "/images/" + uniqueFileName,
                    ContactEmail = model.ContactEmail
                };

                _context.Advertisements.Add(ad);
                _context.SaveChanges();

                return RedirectToAction("ThankYou");
            }

            return View(model);
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        // Trang quảng cáo đã duyệt
        public IActionResult ApprovedAds()
        {
            var ads = _context.Advertisements.Where(a => a.IsApproved).ToList();
            return View(ads);
        }

        // Admin - duyệt quảng cáo
        public IActionResult Pending()
        {
            var ads = _context.Advertisements.ToList(); // ← bỏ lọc để hiển thị cả đã duyệt và chưa duyệt
            return View(ads);
        }


        public IActionResult Approve(int id)
        {
            var ad = _context.Advertisements.Find(id);
            if (ad != null)
            {
                ad.IsApproved = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Pending");  // Quảng cáo chỉ chuyển sang trạng thái duyệt mà không bị xóa
        }

        public IActionResult Delete(int id)
        {
            var ad = _context.Advertisements.Find(id);
            if (ad != null)
            {
                _context.Advertisements.Remove(ad);
                _context.SaveChanges();
            }
            return RedirectToAction("Pending");
        }
    }
}
