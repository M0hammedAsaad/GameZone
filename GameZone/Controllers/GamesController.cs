using GameZone.Services;
using GameZone.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly IDevicesService _devicesService;
        private readonly ICategoriesService _categoriesService;
        private readonly IGameService _gameServices;

        public GamesController(ICategoriesService categoriesService, IDevicesService devicesService, IGameService gameServices)
        {
            _categoriesService = categoriesService;
            _devicesService = devicesService;
            _gameServices = gameServices;
        }

        public IActionResult Index()
        {

            var games = _gameServices.GetAll();
            return View(games);
        }


        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel modelView = new()
            {
                Categories = _categoriesService.GetSelectList(),

                Devices = _devicesService.GetSelectList()


            };

            return View(modelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {

                model.Categories = _categoriesService.GetSelectList();

                model.Devices = _devicesService.GetSelectList();

                return View(model);
            }

            //save Game in database  //save cover to server

            await _gameServices.Create(model);

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var game = _gameServices.GetById(id);
            if (game == null)
                return NotFound();
            return View(game);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = _gameServices.GetById(id);
            if (game == null)
                return NotFound();

            EditFormViewModel model = new EditFormViewModel()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CaregoryId = game.CaregoryId,
                SelectedDevices = game.GameDevices.Select(d => d.DeviceId).ToList(),
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList(),
                currentCover = game.Cover,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditFormViewModel model)
        {
            if (!ModelState.IsValid)
            {

                model.Categories = _categoriesService.GetSelectList();

                model.Devices = _devicesService.GetSelectList();

                return View(model);
            }

            //save Game in database  //save cover to server
            var game = await _gameServices.Update(model);
            if (game == null) return BadRequest();


            return RedirectToAction(nameof(Index));

        }

        //[HttpDelete]  
        public IActionResult Delete(int id)
        {

            var isDeleted = _gameServices.Delete(id);
            return isDeleted ? RedirectToAction(nameof(Index)) : BadRequest();
        }
    }
}
