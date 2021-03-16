using System.Collections.Generic;

namespace FilmsCatalog.Models
{
    public class CatalogViewModel
    {
        /// <summary>
        /// Количество элементов в списке фильмов.
        /// </summary>
        public int TotalNumber { get; set; }

        /// <summary>
        /// Идентификатор екущего пользователя.
        /// </summary>
        public string CurrentUser { get; set; }

        /// <summary>
        /// Список фильмов.
        /// </summary>
        public IEnumerable<FilmViewModel> Films { get; set; }
    }
}
