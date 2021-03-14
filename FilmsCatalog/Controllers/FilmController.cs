using AutoMapper;
using FilmsCatalog.Entities;
using FilmsCatalog.Interfaces;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    [Authorize]
    public class FilmController : Controller
    {
        private readonly IDatabaseRepository<Film> _filmRepository;
        private readonly ILogger<FilmController> _logger;
        private readonly IMapper _mapper;

        public FilmController(
            IDatabaseRepository<Film> filmRepository,
            ILogger<FilmController> logger,
            IMapper mapper)
        {
            _filmRepository = filmRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Получает фильм по id и отправляет клиенту.
        /// </summary>
        /// <param name="idFilm">Идентификатор фильма.</param>
        /// <returns>Отправляет модель в представление.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(int idFilm)
        {
            var filmData = await _filmRepository.GetByIdAsync(idFilm);
            var model = _mapper.Map<FilmViewModel>(filmData);

            return View(model);
        }

        /// <summary>
        /// Получает пустую модель фильма.
        /// </summary>
        /// <returns>Отправляет модель в представление.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View(new FilmViewModel { });
        }

        /// <summary>
        /// Добавляет фильм в базу.
        /// </summary>
        /// <param name="model">Модель данных фильма.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(FilmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = _mapper.Map<Film>(model);
            await _filmRepository.AddAsync(data);

            _logger.LogDebug("Add film: ", @model);

            return RedirectToAction("Index", "Catalog");
        }

        /// <summary>
        /// Получает модель данных по id.
        /// </summary>
        /// <param name="idFilm">Идентификатор фильма.</param>
        /// <returns>Отправляет модель данных в представление.</returns>
        [HttpGet]
        public async Task<IActionResult> Update(int idFilm)
        {
            var filmData = await _filmRepository.GetByIdAsync(idFilm);
            var model = _mapper.Map<FilmViewModel>(filmData);

            return View(model);
        }

        /// <summary>
        /// Обновляет данные в базе.
        /// </summary>
        /// <param name="model">Модель данных.</param>
        /// <returns>Переход на страницу списка.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(FilmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var filmData = await _filmRepository.GetByIdAsync(model.Id);
            filmData.IssueYear = model.IssueYear;
            filmData.Name = model.Name;
            filmData.Poster = model.Poster;
            filmData.Producer = model.Producer;

            await _filmRepository.UpdateAsync(filmData);

            _logger.LogDebug("Update film: ", @filmData);

            return RedirectToAction("Index", "Catalog");
        }
    }
}
