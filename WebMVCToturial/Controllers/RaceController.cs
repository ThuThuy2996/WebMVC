using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVCToturial.Data;
using WebMVCToturial.Interfaces;
using WebMVCToturial.Models;
using WebMVCToturial.ViewModels;

namespace WebMVCToturial.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceResponsitory _raceResponitory;
        private readonly IPhotoService _photoService;
        public RaceController(IRaceResponsitory raceResponitory, IPhotoService photoService)
        {
            _raceResponitory = raceResponitory;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceResponitory.GetAll();
            return View(races);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceResponitory.GetById(id);
            return View(race);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceViewModel)
        {
            if (ModelState.IsValid)
            {
                var uploadResult = await _photoService.AddPhotoAsync(raceViewModel.Image);
                if (uploadResult.Error == null)
                {
                    var race = new Race
                    {
                        Title = raceViewModel.Title,
                        Description = raceViewModel.Description,
                        Image = uploadResult.Url.ToString(),
                        RaceCategory = raceViewModel.RaceCategory,

                        Address = new Address
                        {
                            Street = raceViewModel.Address.Street,
                            City = raceViewModel.Address.City,
                            State = raceViewModel.Address.State,
                        }
                    };
                    _raceResponitory.Add(race);
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
            return View(raceViewModel);

        }
    }
}
