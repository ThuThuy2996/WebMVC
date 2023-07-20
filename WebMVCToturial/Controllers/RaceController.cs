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


        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceResponitory.GetById(id);
            if (race != null)
            {
                var raceEditViewModel = new EditRaceViewModel
                {
                    Title = race.Title,
                    Description = race.Description,
                    URL = race.Image,
                    RaceCategory = race.RaceCategory,
                    AddressId = race.AddressId,

                    Address = new Address
                    {
                        Street = race.Address.Street,
                        City = race.Address.City,
                        State = race.Address.State,
                    }
                };
                return View(raceEditViewModel);
            }
            return View("Race is not found!");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel editRaceView)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", editRaceView);
            }
            var oldRace = await _raceResponitory.GetByIdAsyncNoTracking(id);
            if (oldRace == null) return View("Club is not found!");
            Race raceModified = new Race();
            if (editRaceView.Image != null)
            {
                var uploadResult = await _photoService.AddPhotoAsync(editRaceView.Image);
                if (uploadResult.Error != null)
                {
                    ModelState.AddModelError("", "Upload photo was failed ! Try again !");
                    return View("Edit", editRaceView);
                }

                raceModified = new Race
                {
                    Id = id,
                    Title = editRaceView.Title,
                    Description = editRaceView.Description,
                    Image = uploadResult.Url.ToString(),
                    RaceCategory = editRaceView.RaceCategory,

                    Address = new Address
                    {
                        Street = editRaceView.Address.Street,
                        City = editRaceView.Address.City,
                        State = editRaceView.Address.State,
                    }
                };
            }

            if (!string.IsNullOrEmpty(oldRace.Image))
            {
                await _photoService.DeletePhotoAsync(oldRace.Image);
            }
            var rs = _raceResponitory.Update(raceModified);
            if (!rs)
            {
                ModelState.AddModelError("", "Edit failed ! Try again !");
                return View("Edit", editRaceView);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var race = await _raceResponitory.GetById(id);
            if (race != null)
            {
                return View(race);
            }
            return View("Race is not found");
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var race = await _raceResponitory.GetById(id);
            if (race != null)
            {
                return _raceResponitory.Delete(race) ? RedirectToAction("Index") : View("Error", race);
            }
            return View("Not Found", race);
        }

    }
}
