using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVCToturial.Data;
using WebMVCToturial.Interfaces;
using WebMVCToturial.Models;
using WebMVCToturial.ViewModels;

namespace WebMVCToturial.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubResponsitory _clubResponsitory;
        private readonly IPhotoService _photoService;
        public ClubController(IClubResponsitory clubResponsitory, IPhotoService photoService)
        {
            _clubResponsitory = clubResponsitory;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubResponsitory.GetAll();
            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubResponsitory.GetClubsById(id);
            return View(club);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubViewModel)
        {
            if (ModelState.IsValid)
            {
                var uploadResult = await _photoService.AddPhotoAsync(clubViewModel.Image);
                if (uploadResult.Error == null)
                {
                    var club = new Club
                    {
                        Title = clubViewModel.Title,
                        Description = clubViewModel.Description,
                        Image = uploadResult.Url.ToString(),
                        ClubCategory = clubViewModel.ClubCategory,

                        Address = new Address
                        {
                            Street = clubViewModel.Address.Street,
                            City = clubViewModel.Address.City,
                            State = clubViewModel.Address.State,
                        }
                    };
                    _clubResponsitory.Add(club);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Photo upload failed ! Please try again!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed ! Please try again!");

            }
            return View(clubViewModel);
        }
    }
}
