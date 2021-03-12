using AutoMapper;
using FilmsCatalog.Entities;
using FilmsCatalog.Interfaces;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IDatabaseRepository<Film> _filmRepository;
        private readonly ILogger<FilmController> _logger;
        private readonly IMapper _mapper;

        public CatalogController(
            IDatabaseRepository<Film> filmRepository,
            ILogger<FilmController> logger,
            IMapper mapper)
        {
            _filmRepository = filmRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Получает список всех фильмов.
        /// </summary>
        /// <returns>Отправляет список моделей в представление.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var filmDatas = await _filmRepository.ListAllAsync();
            var models = _mapper.Map<IEnumerable<FilmViewModel>>(filmDatas);

            return View(models);
        }
    }
}
