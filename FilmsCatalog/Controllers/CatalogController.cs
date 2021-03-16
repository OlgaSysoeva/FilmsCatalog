using AutoMapper;
using FilmsCatalog.Entities;
using FilmsCatalog.Interfaces;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IDatabaseRepository<Film> _filmRepository;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<CatalogController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly FilesConfigModel _filesCongigModel;

        public CatalogController(
            IDatabaseRepository<Film> filmRepository,
            IMapper mapper,
            SignInManager<User> signInManager,
            ILogger<CatalogController> logger,
            UserManager<User> userManager,
            IOptions<FilesConfigModel> congig)
        {
            _filmRepository = filmRepository;
            _mapper = mapper;
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _filesCongigModel = congig.Value;
    }

        /// <summary>
        /// Получает странрицу для просмотра каталога.
        /// </summary>
        /// <returns>Отправляет представление.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /// <summary>
        /// Получает каталог фильмов для заданной страницы.
        /// </summary>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="pageNumber">Номер текущей страницы.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFilms(int pageSize, int pageNumber)
        {
            var filmsCount = await _filmRepository.CountAsync();
            var currentUser = _userManager.GetUserId(User);

            if (filmsCount == 0)
            {
                var catalog = new CatalogViewModel
                {
                    TotalNumber = 0,
                    CurrentUser = currentUser,
                    Films = Enumerable.Empty<FilmViewModel>()
                };

                return new JsonResult(catalog, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }

            var startIndex = (pageNumber - 1) * pageSize;

            var filmDatas = await _filmRepository.GetFilmsPageAsync<string>(
                x => x.Name, startIndex, pageSize);

            var models = Enumerable.Empty<FilmViewModel>();
            try
            {
                models = _mapper.Map<IEnumerable<FilmViewModel>>(filmDatas);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error of mapper: Film to FilmViewModel: {ex}", ex);   
            }

            foreach (var film in models.Where(x => x.PosterPath == null))
                film.PosterPath = _filesCongigModel.DefaultFilePath;

            var catalogViewModel = new CatalogViewModel
            {
                TotalNumber = filmsCount,
                CurrentUser = currentUser,
                Films = models
            };

            return new JsonResult(catalogViewModel, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
