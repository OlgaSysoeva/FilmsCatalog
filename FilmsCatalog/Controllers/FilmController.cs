using AutoMapper;
using FilmsCatalog.Entities;
using FilmsCatalog.Interfaces;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    [Authorize]
    public class FilmController : Controller
    {
        private readonly IDatabaseRepository<Film> _filmRepository;
        private readonly ILogger<FilmController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly FilesConfigModel _filesCongigModel;

        public FilmController(
            IDatabaseRepository<Film> filmRepository,
            ILogger<FilmController> logger,
            IMapper mapper,
            UserManager<User> userManager,
            IWebHostEnvironment appEnvironment,
            IOptions<FilesConfigModel> congig)
        {
            _filmRepository = filmRepository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
            _filesCongigModel = congig.Value;
        }

        /// <summary>
        /// Получает фильм по id и отправляет клиенту.
        /// </summary>
        /// <param name="idFilm">Идентификатор фильма.</param>
        /// <returns>Отправляет модель в представление.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(int idFilm)
        {
            if (idFilm <= 0)
                return BadRequest();

            var filmData = await _filmRepository.GetByIdAsync(idFilm);

            if (filmData == null)
                return NotFound();

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
            ViewBag.Formats = GetStringFormats();
            ViewBag.FileInfo = "Допустимые форматы: " + ViewBag.Formats + 
                ". Максимальный размер файла: " + (_filesCongigModel.Length / (1024 * 1024)) + " Mb.";

            return View(new FilmViewModel { });
        }

        /// <summary>
        /// Добавляет фильм в базу.
        /// </summary>
        /// <param name="model">Модель данных фильма.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(FilmViewModel model, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (!IsCorrectFormat(Path.GetExtension(uploadedFile.FileName)))
                {
                    ModelState.AddModelError(nameof(model.PosterPath),
                    "Недопустимый формат данных!");
                }
                if (!IsCorrectFileLength(uploadedFile.Length))
                {
                    ModelState.AddModelError(nameof(model.PosterPath),
                        "Размер превышает норму!");
                }
            }            

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = _mapper.Map<Film>(model);
            data.UserId = _userManager.GetUserId(User);

            if (uploadedFile != null)
            {
                string path = GetFilePath(uploadedFile.FileName);
                using var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create);
                await uploadedFile.CopyToAsync(fileStream);
                data.PosterPath = path;
            }

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
            if (idFilm <= 0)
            {
                return BadRequest();
            }

            var filmData = await _filmRepository.GetByIdAsync(idFilm);

            if (filmData == null)
            {
                return NotFound();
            }

            if (_userManager.GetUserId(User) != filmData.UserId)
            {
                return Forbid();
            }

            var model = _mapper.Map<FilmViewModel>(filmData);
            model.PosterPath ??= _filesCongigModel.DefaultFilePath;

            ViewBag.Formats = GetStringFormats();
            ViewBag.FileInfo = "Допустимые форматы: " + ViewBag.Formats +
                ". Максимальный размер файла: " + (_filesCongigModel.Length / (1024 * 1024)) + " Mb.";

            return View(model);
        }

        /// <summary>
        /// Обновляет данные в базе.
        /// </summary>
        /// <param name="model">Модель данных.</param>
        /// <returns>Переход на страницу списка.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(FilmViewModel model, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (!IsCorrectFormat(Path.GetExtension(uploadedFile.FileName)))
                {
                    ModelState.AddModelError(nameof(model.PosterPath),
                    "Недопустимый формат данных!");
                }
                if (!IsCorrectFileLength(uploadedFile.Length))
                {
                    ModelState.AddModelError(nameof(model.PosterPath),
                        "Размер превышает норму!");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var filmData = await _filmRepository.GetByIdAsync(model.Id);

            if (filmData == null)
            {
                return NotFound();
            }

            filmData.IssueYear = model.IssueYear;
            filmData.Name = model.Name;
            filmData.Producer = model.Producer;

            if (uploadedFile != null)
            {
                string path = GetFilePath(uploadedFile.FileName);
                using var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create);
                await uploadedFile.CopyToAsync(fileStream);
                filmData.PosterPath = path;
            }

            await _filmRepository.UpdateAsync(filmData);

            _logger.LogDebug("Update film: ", @filmData);

            return RedirectToAction("Index", "Catalog");
        }

        /// <summary>
        /// Получает путь к файлу.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>Строковое представление пути.</returns>
        private string GetFilePath(string fileName)
        {
            string fileExt = Path.GetExtension(fileName);

            return _filesCongigModel.SavePath + Guid.NewGuid() + fileExt;
        }

        /// <summary>
        /// Формирует строку форматов из массива.
        /// </summary>
        /// <returns>Возвращает строку форматов.</returns>
        private string GetStringFormats()
        {
            if (_filesCongigModel.Formats.Length == 0)
            {
                return "";
            }

            var formatStr = "";
            foreach (var format in _filesCongigModel.Formats)
            {
                formatStr += format + ", ";
            }
            formatStr = formatStr.Remove(formatStr.Length - 2);

            return formatStr;
        }

        /// <summary>
        /// Проверяет допустимость формата файла.
        /// </summary>
        /// <param name="format">Формат в виде строки.</param>
        /// <returns>True если формат допустим.</returns>
        private bool IsCorrectFormat(string format)
        {
            if (_filesCongigModel.Formats.Contains(format))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверяет допустимость размера файла.
        /// </summary>
        /// <param name="length">Длина файла в байтах.</param>
        /// <returns>True если размер в допустимых пределах.</returns>
        private bool IsCorrectFileLength(long length)
        {
            if (length > _filesCongigModel.Length)
            {
                return false;
            }

            return true;
        }
    }
}
