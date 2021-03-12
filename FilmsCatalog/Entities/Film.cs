namespace FilmsCatalog.Entities
{
    /// <summary>
    /// Информация о фильме.
    /// </summary>
    public class Film
    {
        public int Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Год выпуска.
        /// </summary>
        public int IssueYear { get; set; }

        /// <summary>
        /// Режиссёр.
        /// </summary>
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
