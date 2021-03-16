
namespace FilmsCatalog.Models
{
    /// <summary>
    /// Конфигурация настроек хранения файлов.
    /// </summary>
    public class FilesConfigModel
    {
        /// <summary>
        /// Путь к постеру по умолчанию. Используетсяесть постер не задан.
        /// </summary>
        public string DefaultFilePath { get; set; }

        /// <summary>
        /// Путь хранения файлов.
        /// </summary>
        public string SavePath { get; set; }
    }
}
