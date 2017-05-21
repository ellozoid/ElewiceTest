using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel.Interfaces
{
    /// <summary>
    /// Базовый интерфейс
    /// </summary>
    public interface IBaseRepository<T> where T: class
    {
        ///// <summary>
        ///// Сохранение в базу
        ///// </summary>
        //void Save();


        /// <summary>
        /// Возвращает список всех элементов таблицы
        /// </summary>
        IEnumerable<T> GetAll();
    }
}
