using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class FilmViewModel
    {
        public int Id { get; set; }

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
        /// Пользователь, добавивший фильм.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Изображение постера.
        /// </summary>
        public byte[] Poster { get; set; }
    }
}
