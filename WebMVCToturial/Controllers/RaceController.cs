using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVCToturial.Data;
using WebMVCToturial.Interfaces;
using WebMVCToturial.Models;

namespace WebMVCToturial.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceResponsitory _raceResponitory;
        public RaceController(IRaceResponsitory raceResponitory)
        {
            _raceResponitory = raceResponitory;
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
        public async Task<IActionResult> Create(Race race)
        {
            if (ModelState.IsValid)
            {
                _raceResponitory.Add(race);
                return RedirectToAction("Index");
            }
            return View(race);
        }
    }
}
