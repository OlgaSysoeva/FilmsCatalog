using AutoMapper;
using FilmsCatalog.Entities;

namespace FilmsCatalog.Models
{
    /// <summary>
    /// Карта конвертации моделей.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Film, FilmViewModel>().ReverseMap();
        }
    }
}
