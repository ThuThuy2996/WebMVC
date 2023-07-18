using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVCToturial.Data;
using WebMVCToturial.Interfaces;
using WebMVCToturial.Models;

namespace WebMVCToturial.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubResponsitory _clubResponsitory;
        public ClubController(IClubResponsitory clubResponsitory)
        {
            _clubResponsitory = clubResponsitory;
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
        public async Task<IActionResult> Create(Club club)
        {
            if(ModelState.IsValid)
            {
                _clubResponsitory.Add(club);
                return RedirectToAction("Index");
            }
            return View(club);
        }
    }
}
