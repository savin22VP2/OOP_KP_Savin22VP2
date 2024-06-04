using System;

namespace OOP_Kursach_09
{
    /// <summary>
    /// Структура, представляющая конкурс.
    /// </summary>
    public class Competition
    {
        private string Name;
        private int Year;
        private int CountMembers;
        private int CountWorks;
        private int CountVisitors;
        public Competition(string name, int year, int count1, int count2, int count3)
        {
            Name = name;
            Year = year;
            CountMembers = count1;
            CountWorks = count3;
            CountVisitors = count2;
        }
        public string GetName() {return Name;}
        public int GetYear() {return Year; }
        public int GetMem() { return CountMembers; }
        public int GetWorks() { return CountWorks; }
        public int GetVisitors() { return CountVisitors; }
    }
}
