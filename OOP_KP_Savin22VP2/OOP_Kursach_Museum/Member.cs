using System;
using System.Collections.Generic;

namespace OOP_Kursach_Museum
{
    /// <summary>
    /// Структура, представляющая музейный экспонат.
    /// </summary>
    public class Member
    {
        /// <summary>
        /// Получает или задает имя автора.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает название экспоната.
        /// </summary>
        public string NameOfWork { get; set; }


        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="Member"/>.
        /// </summary>
        /// <param name="name">Имя автора.</param>
        /// <param name="year">Год создания экспоната.</param>
        /// <param name="exhibitName">Название экспоната.</param>
        /// <param name="onExhibit">Значение, указывающее, находится ли экспонат на выставке.</param>
        public Member(string name, int year, string exhibitName, bool onExhibit)
        {
            Name = name;
            NameOfWork = exhibitName;
        }
    }

    public class Competition
    {
        private List<Member> users = new List<Member>();
        private string Name { get; set; }
        private int Year { get; set; }
        public Competition(string name, int year)
        {
            Name = name;
            Year = year;
        }
        public void AddUser(Member user)
        {
            users.Add(user);
        }

    }
}
