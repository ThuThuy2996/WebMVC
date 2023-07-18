using Microsoft.EntityFrameworkCore;
using WebMVCToturial.Data;
using WebMVCToturial.Interfaces;
using WebMVCToturial.Models;

namespace WebMVCToturial.Responsitory
{
    public class ClubResponsitory : IClubResponsitory
    {
        private readonly ApplicationDbContext _context;
        public ClubResponsitory(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Club club)
        {
            _context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context?.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<IEnumerable<Club>> GetClubsByCity(string city)
        {
            return await _context.Clubs.Include(o => o.Address).Where(p => p.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Club> GetClubsById(int id)
        {
            return await _context.Clubs.Include(o => o.Address).FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}
