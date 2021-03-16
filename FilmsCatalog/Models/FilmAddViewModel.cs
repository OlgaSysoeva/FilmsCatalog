using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class FilmAddViewModel
    {
        /// <summary>
        /// Название.
        /// </summary>
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        [Display(Name = "Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Год выпуска.
        /// </summary>
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Год выпуска")]
        public int IssueYear { get; set; }

        /// <summary>
        /// Режиссёр.
        /// </summary>
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Режиссёр")]
        public string Producer { get; set; }

        /// <summary>
        /// Изображение постера.
        /// </summary>
        [Display(Name = "Постер")]
        public string PosterPath { get; set; }
    }
}
