using WebMVCToturial.Models;

namespace WebMVCToturial.Interfaces
{
    public interface IRaceResponsitory
    {
        Task<IEnumerable<Race>> GetAll();
        Task<IEnumerable<Race>> GetClubsByCity(string city);
        Task<Race?> GetByIdAsyncNoTracking(int id);
        Task<Race> GetById(int id);
        bool Add(Race race);

        bool Update(Race race);

        bool Delete(Race race);

        bool Save();
    }
}
