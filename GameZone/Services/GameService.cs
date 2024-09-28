using GameZone.Models;
using GameZone.Settings;
using GameZone.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public GameService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagePath}";
        }
        public async Task Create(CreateGameFormViewModel model)
        {
            //create cover in sever
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(model.Cover.FileName)}";

            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await model.Cover.CopyToAsync(stream);

            Game game = new Game()
            {
                Name = model.Name,
                Description = model.Description,
                CaregoryId = model.CaregoryId,
                Cover = coverName.ToString(),
                GameDevices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
            };

            await _context.Games.AddAsync(game);
            _context.SaveChanges();
        }
        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.GameDevices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .ToList();
        }

        public Game? GetById(int id)
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.GameDevices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .SingleOrDefault(s => s.Id == id);
        }

        public async Task<Game?> Update(EditFormViewModel model)
        {
            var game = _context.Games
                .Include(g => g.GameDevices)
                .SingleOrDefault(g => g.Id == model.Id);
            if (game == null)
                return null;

            var delCover = game.Cover;
            game.Name = model.Name;
            game.Description = model.Description;
            game.CaregoryId = model.CaregoryId;
            game.GameDevices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

            if (model.Cover is not null)
            {
                //create cover in sever
                var coverName = $"{Guid.NewGuid()}{Path.GetExtension(model.Cover.FileName)}";

                var path = Path.Combine(_imagesPath, coverName);

                using var stream = File.Create(path);
                await model.Cover.CopyToAsync(stream);

                game.Cover = coverName;
            }
            var effRows = _context.SaveChanges();

            if (effRows > 0)
            {
                if (model.Cover is not null)
                {
                    var oldCover = Path.Combine(_imagesPath, delCover);
                    File.Delete(oldCover);

                }
                return game;

            }
            else
            {
                var oldCover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(oldCover);

                return null;
            }
        }

        public bool Delete(int id)
        {
            var isDeleted = false;
            var game = _context.Games.Find(id);
            if (game is null)
                return isDeleted;

            _context.Games.Remove(game);
            var effRows = _context.SaveChanges();
            if (effRows > 0)
            {
                isDeleted = true;

                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
            }

            return isDeleted;
        }

    }
}
