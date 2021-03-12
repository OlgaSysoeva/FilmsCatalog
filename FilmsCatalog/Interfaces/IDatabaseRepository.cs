﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmsCatalog.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория базы.
    /// </summary>
    /// <typeparam name="T">Сущность.</typeparam>
    public interface IDatabaseRepository<T> where T : class 
    {
        /// <summary>
        /// Производит поиск данных по id.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Найденную информацию сущности.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Получает список всех данных сущности.
        /// </summary>
        /// <returns>Список данных.</returns>
        Task<IEnumerable<T>> ListAllAsync();

        /// <summary>
        /// Добавляет новые данные в базу.
        /// </summary>
        /// <param name="entity">Данные сущности.</param>
        /// <returns>Добавленную нформацию.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Обновляет данные в базе.
        /// </summary>
        /// <param name="entity">Данные сущности для обновления.</param>
        /// <returns></returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Удаляет данные из базы.
        /// </summary>
        /// <param name="entity">Информация для удаления.</param>
        /// <returns></returns>
        Task DeleteAsync(T entity);
    }
}
