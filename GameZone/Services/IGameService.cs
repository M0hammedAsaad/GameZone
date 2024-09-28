using GameZone.Models;
using GameZone.ViewModels;

namespace GameZone.Services
{
    public interface IGameService
    {
        Task Create(CreateGameFormViewModel model);
        Task<Game?> Update(EditFormViewModel model);

        IEnumerable<Game> GetAll();

        Game? GetById(int id);

        bool Delete(int id);
    }
}
