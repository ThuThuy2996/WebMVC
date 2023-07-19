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

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubResponsitory.GetClubsById(id);
            if (club != null)
            {
                var clubEditViewModel = new EditClubViewModel
                {
                    Title = club.Title,
                    Description = club.Description,
                    URL = club.Image,
                    ClubCategory = club.ClubCategory,
                    AddressId = club.AddressId,

                    Address = new Address
                    {
                        Street = club.Address.Street,
                        City = club.Address.City,
                        State = club.Address.State,
                    }
                };
                return View(clubEditViewModel);
            }
            return View("Club is not found!");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel editClubViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", editClubViewModel);
            }
            var oldClub = await _clubResponsitory.GetByIdAsyncNoTracking(id);
            if (oldClub == null) return View("Club is not found!");
            Club clupModified = new Club();
            if (editClubViewModel.Image != null)
            {
                var uploadResult = await _photoService.AddPhotoAsync(editClubViewModel.Image);
                if (uploadResult.Error != null)
                {
                    ModelState.AddModelError("", "Upload photo was failed ! Try again !");
                    return View("Edit", editClubViewModel);
                }

                clupModified = new Club
                {
                    Id = id,
                    Title = editClubViewModel.Title,
                    Description = editClubViewModel.Description,
                    Image = uploadResult.Url.ToString(),
                    ClubCategory = editClubViewModel.ClubCategory,

                    Address = new Address
                    {
                        Street = editClubViewModel.Address.Street,
                        City = editClubViewModel.Address.City,
                        State = editClubViewModel.Address.State,
                    }
                };      
            }

            if (!string.IsNullOrEmpty(oldClub.Image))
            {
                await _photoService.DeletePhotoAsync(oldClub.Image);
            }
            var rs = _clubResponsitory.Update(clupModified);
            if (!rs)
            {
                ModelState.AddModelError("", "Edit failed ! Try again !");
                return View("Edit", editClubViewModel);
            }
            return RedirectToAction("Index");
        }
    }
}
