﻿using AutoMapper;
using FilmsCatalog.Entities;
using FilmsCatalog.Interfaces;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IDatabaseRepository<Film> _filmRepository;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;

        public CatalogController(
            IDatabaseRepository<Film> filmRepository,
            IMapper mapper,
            SignInManager<User> signInManager)
        {
            _filmRepository = filmRepository;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Получает список всех фильмов.
        /// </summary>
        /// <returns>Отправляет список моделей в представление.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            var filmDatas = await _filmRepository.ListAllAsync();
            var models = _mapper.Map<IEnumerable<FilmViewModel>>(filmDatas);

            return View(models);
        }
    }
}
