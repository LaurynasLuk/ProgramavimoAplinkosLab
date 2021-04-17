using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace DataProcessing
{
    class Student
    {
        private string name;
        private string lastName;
        private List<int> Results = new List<int>();

        public Student() { }
        public Student(string name, string lastName, List<int> results)
        {
            this.Name = name;
            this.LastName = lastName;
            Results = results;
        }

        public string Name { get => name; set => name = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public List<int> results { get { return Results; } }



    }
    class Program
    {
        static void Main(string[] args)
        {

            List<Student> studentList = new List<Student>();
            bool menuFlag = true;

            while (menuFlag)
            {
                Console.Clear();
                Console.WriteLine("1. Add student");
                Console.WriteLine("2. Add students from file");
                Console.WriteLine("3. display students");
                Console.WriteLine("4. Exit");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        Student student = new Student();
                        Console.Write("Enter first name: ");
                        string tempName = Console.ReadLine();
                        student.Name = tempName;
                        Console.Write("Enter last name: ");
                        string tempLastName = Console.ReadLine();
                        student.LastName = tempLastName;


                        bool resultFlag = true;
                        while (resultFlag)
                        {
                            Console.WriteLine("1. Add Result");
                            Console.WriteLine("2. Stop adding results");
                            Console.WriteLine("3. Generate results randomly");
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    Console.Clear();
                                    Console.WriteLine("Enter homework/exam result: ");
                                    int tempResult = Int32.Parse(Console.ReadLine());
                                    student.results.Add(tempResult);
                                    Console.Clear();
                                    Console.WriteLine("Result added\n");
                                    break;

                                case "2":
                                    Console.WriteLine("No results added");
                                    resultFlag = false;
                                    break;
                                case "3":
                                    Random rnd = new Random();
                                    for (int i = 1; i < 6; i++)
                                    {
                                        int randomNum = rnd.Next(1, 11);
                                        student.results.Add(randomNum);
                                    }
                                    Console.WriteLine("Results generated randomly");
                                    resultFlag = false;
                                    break;

                            }
                        }
                        studentList.Add(student);
                        Console.Clear();

                        break;
                    case "2":

                        using (var sr = new StreamReader("students.txt"))
                        {
                            sr.ReadLine();
                            while (!sr.EndOfStream)
                            {
                                Student studentFile = new Student();
                                var line = sr.ReadLine();
                                var values = line.Split();
                                if (line.Split().Length > 2)
                                {
                                    studentFile.LastName = values[0];
                                    studentFile.Name = values[1];
                                    for (int i = 2; i < 8; i++)
                                    {
                                        studentFile.results.Add(Int32.Parse(values[i]));
                                    }
                                    studentList.Add(studentFile);
                                }
                            }
                            Console.Clear();
                            Console.WriteLine("Added students from file");
                            Console.ReadKey();
                        }
                        break;
                    case "3":
                        Console.Clear();
                        List<Student> sortedList = studentList.OrderBy(student => student.LastName).ToList();
                        if (studentList.Count == 0)
                        {
                            Console.WriteLine("No registered students");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Surname            Name            Final Points(Med.) Final points (Avg.)");
                            Console.WriteLine("-----------------------------------------------------------------------------------------");

                            foreach (var item in sortedList)
                            {
                                double avg = 0;
                                foreach (var result in item.results)
                                {
                                    avg = avg + result;
                                }
                                if (avg > 0) Console.WriteLine(String.Format("|{0,-16} |{1,-15} |{2,-17} |{3:N2}", item.LastName, item.Name, GetMedian(item.results), avg / item.results.Count));
                                else Console.WriteLine(String.Format("|{0,-16} |{1,-15} |{2,-17} |{3}", item.LastName, item.Name, "No results achieved", "No results achieved"));

                            }
                            Console.ReadKey();
                        }
                        break;
                    case "4":
                        menuFlag = false;
                        break;
                }

            }


        }

        public static int GetMedian(List<int> results)
        {
            if (results == null || results.Count == 0)
                throw new System.Exception("Median of empty array not defined.");

            int[] sortedNum = new int[results.Count];
            results.CopyTo(sortedNum);
            Array.Sort(sortedNum);

            int size = sortedNum.Length;
            int mid = size / 2;
            int median = (size % 2 != 0) ? (int)sortedNum[mid] : ((int)sortedNum[mid] + (int)sortedNum[mid - 1]) / 2;
            return median;
        }
    }
}
