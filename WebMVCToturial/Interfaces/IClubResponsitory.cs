using WebMVCToturial.Models;

namespace WebMVCToturial.Interfaces
{
    public interface IClubResponsitory 
    {
        Task<IEnumerable<Club>> GetAll();
        Task<IEnumerable<Club>> GetClubsByCity(string city);
        Task<Club> GetClubsById(int id);
        Task<Club?> GetByIdAsyncNoTracking(int id);
        bool Add(Club club);

        bool Update(Club club);

        bool Delete(Club club);

        bool Save();
    }
}
