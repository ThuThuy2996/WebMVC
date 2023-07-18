using Microsoft.EntityFrameworkCore;
using WebMVCToturial.Data;
using WebMVCToturial.Interfaces;
using WebMVCToturial.Models;

namespace WebMVCToturial.Responsitory
{
    public class RaceResponsitory : IRaceResponsitory
    {
        private readonly ApplicationDbContext _context;
        public RaceResponsitory(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Race race)
        {
            _context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context?.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _context.Race.ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetClubsByCity(string city)
        {
            return await _context.Race.Include(o => o.Address).Where(p => p.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Race> GetById(int id)
        {
            return await _context.Race.Include(o => o.Address).FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}
