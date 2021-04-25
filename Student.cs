using System.Collections.Generic;
namespace DataProcessing
{
    class Student
    {
        private string name;
        private string lastName;
        private List<int> Results = new List<int>();
        private int examResult;

        public Student() { }
        public Student(string name, string lastName, List<int> results, int examResult)
        {
            this.Name = name;
            this.LastName = lastName;
            Results = results;
            this.ExamResult = examResult;
        }

        public string Name { get => name; set => name = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public List<int> results { get { return Results; } }
        public int ExamResult { get => examResult; set => examResult = value; }
    }
}
