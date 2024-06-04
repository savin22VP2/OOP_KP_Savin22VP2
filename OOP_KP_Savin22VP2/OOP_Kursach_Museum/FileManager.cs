using System.Collections.Generic;
using System.IO;

namespace OOP_Kursach_09
{
    /// <summary>
    /// Статический класс для управления файлами, содержащими данные о конкурсе
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Путь к файлу с данными.
        /// </summary>
        private static string filePath = "input.txt";

        /// <summary>
        /// Считывает данные из файла и возвращает список
        /// </summary>
        public static List<Competition> ReadFromFile()
        {
            List<Competition> museums = new List<Competition>();
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 5 && int.TryParse(parts[1], out int year) && int.TryParse(parts[3], out int vis) && int.TryParse(parts[2], out int mem) && int.TryParse(parts[4], out int works))
                    {
                        museums.Add(new Competition(parts[0], year, mem, vis,works));
                    }
                }
            }
            return museums;
        }

        /// <summary>
        /// Записывает список в файл, перезаписывая его.
        /// </summary>
        /// <param name="museums">Список конкурсов для записи в файл.</param>
        public static void WriteToFile(List<Competition> comps)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                foreach (var comp in comps)
                {
                    sw.WriteLine($"{comp.GetName()}|{comp.GetYear()}|{comp.GetMem()}|{comp.GetVisitors()}|{comp.GetWorks()}");
                }
            }
        }

        /// <summary>
        /// Добавляет в файл.
        /// </summary>
        public static void AppendToFile(Competition comp)
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine($"{comp.GetName()}|{comp.GetYear()}|{comp.GetMem()}|{comp.GetVisitors()}|{comp.GetWorks()}");
            }
        }

        /// <summary>
        /// Удаляет файл с данными.
        /// </summary>
        public static void DeleteFile()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
