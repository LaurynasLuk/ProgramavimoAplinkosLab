using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading;
namespace DataProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

       
            List<Student> studentList = new List<Student>();
            bool menuFlag = true;

            while (menuFlag)
            {
                Console.Clear();
                Console.WriteLine("1. Add student");
                Console.WriteLine("2. Add students from file");
                Console.WriteLine("3. Display student list");
                Console.WriteLine("4. Filter by failed and passed");
                Console.WriteLine("5. Exit");
                switch (Console.ReadLine())
                {
                    case "1":

                        bool resultFlag = true;
                        while (resultFlag)
                        {
                            Console.Clear();
                            Console.WriteLine("1. Add Results manually");
                            Console.WriteLine("2. Generate results randomly");
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    studentList.Add(addStudentmanually());
                                    resultFlag = false;
                                    break;
                                case "2":
                                    studentList.Add(addStudentRandom());
                                    resultFlag = false;
                                    break;

                            }
                        }
                        Console.Clear();

                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("1. File with 10000 students");
                        Console.WriteLine("2. File with 100000 students");
                        Console.WriteLine("3. File with 1000000 students");
                        Console.WriteLine("4. File with 10000000 students");
                        switch (Console.ReadLine())
                        {
                            case "1":                               
                                generateStudents("students1.txt", 10000);                                                                                          
                                studentList = addStudentsFromFile("students1.txt").ToList();
                                break;
                            case "2":                               
                                generateStudents("students2.txt", 100000);
                                studentList = addStudentsFromFile("students2.txt").ToList();
                                break;
                            case "3":
                                generateStudents("students3.txt", 1000000);

                                studentList = addStudentsFromFile("students3.txt").ToList();
                                break;
                            case "4":
                                generateStudents("students4.txt", 10000000);
                                studentList = addStudentsFromFile("students4.txt").ToList();
                                break;
                        }
                        break;
                    case "3":
                        displayStudents(studentList);
                        break;
                    case "4":
                        if (studentList.Count > 0)
                        {
                            filterFailedPassed(studentList);
                            Console.Clear();
                            Console.WriteLine("successfully filtered");
                            Console.ReadKey();
                        }

                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Nothing to filter empty student list");
                            Console.ReadKey();

                        }
                        break;
                    case "5":
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
        public static void generateStudents(string path, int amount)
        {
            Random rnd = new Random();
            using (var stream = new StreamWriter(File.OpenWrite(path)))
            {
                stream.WriteLine("Surname Name HW1 HW2 HW3 HW4 HW5 Exam");
                for (int i = 1; i <= amount; i++)
                {

                    stream.WriteLine("Surname" + i + " Name" + i + " " + rnd.Next(1, 11) + " " + rnd.Next(1, 11) + " " + rnd.Next(1, 11) + " " + rnd.Next(1, 11) + " " + rnd.Next(1, 11) + " " + rnd.Next(1, 11));
                }
                stream.Close();
            }
            
        }

        public static void displayStudents(List<Student> studentList)
        {
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
                    double finalPoints = ((avg / item.results.Count) * 0.3) + (item.ExamResult * 0.7);
                    List<int> medianList = item.results.ToList();
                    //medianList.Add(item.ExamResult);                            
                    if (item.results.Count > 0 && item.ExamResult > 0) Console.WriteLine(String.Format("|{0,-16} |{1,-15} |{2,-17} |{3:N2}", item.LastName, item.Name, GetMedian(medianList), finalPoints));
                    else Console.WriteLine(String.Format("|{0,-16} |{1,-15} |{2,-17} |{3}", item.LastName, item.Name, "No results achieved", "No results achieved"));

                }
                Console.ReadKey();
            }
        }

        public static List<Student> addStudentsFromFile(string path)
        {
            List<Student> studentList = new List<Student>();
            try
            {
                using (var sr = new StreamReader(path))
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
                            for (int i = 2; i < 7; i++)
                            {

                                studentFile.results.Add(Int32.Parse(values[i]));


                            }
                            studentFile.ExamResult = Int32.Parse(values[7]);
                            studentList.Add(studentFile);
                        }
                        
                    }
                    sr.Close();
                    Console.Clear();
                    Console.WriteLine("Added students from file");
                    Console.ReadKey();
                }
            }
            catch (IOException e)
            {
                Console.Clear();
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            return studentList;
        }

        public static Student addStudentmanually()
        {
            Console.Clear();
            Student student = new Student();
            Console.Write("Enter first name: ");
            string tempName = Console.ReadLine();
            student.Name = tempName;
            Console.Write("Enter last name: ");
            string tempLastName = Console.ReadLine();
            student.LastName = tempLastName;

            bool homeworkFlag = true;
            while (homeworkFlag)
            {
                Console.Clear();
                Console.WriteLine("1.Add homework result");
                Console.WriteLine("2.Stop adding homework results");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        string tempRead = Console.ReadLine();
                        int tempResult;
                        if (Int32.TryParse(tempRead, out tempResult)) student.results.Add(tempResult);
                        else
                        {
                            Console.WriteLine("Wrong input result not added");
                            Console.ReadKey();
                            break;
                        }
                        Console.Clear();
                        Console.WriteLine("Result added\n");
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("1.Enter exam result: ");
                        string tempExam = Console.ReadLine();
                        int tempExamResult;
                        if (Int32.TryParse(tempExam, out tempExamResult)) student.ExamResult = tempExamResult;
                        else
                        {
                            Console.WriteLine("Wrong input result not added");
                            Console.ReadKey();
                            break;
                        }
                        Console.Clear();
                        Console.WriteLine("Exam Result added\n");
                        homeworkFlag = false;
                        break;
                }
            }
            return student;
        }

        public static Student addStudentRandom()
        {
            Console.Clear();
            Student student = new Student();
            Console.Write("Enter first name: ");
            string tempName = Console.ReadLine();
            student.Name = tempName;
            Console.Write("Enter last name: ");
            string tempLastName = Console.ReadLine();
            student.LastName = tempLastName;

            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                int randomNum = rnd.Next(1, 11);
                student.results.Add(randomNum);
            }
            student.ExamResult = rnd.Next(1, 11);
            Console.WriteLine("Results generated randomly");

            return student;
        }

        public static void filterFailedPassed(List<Student> studentList)
        {
            if (File.Exists("passed.txt") && File.Exists("failed.txt"))
            {
                File.Delete("passed.txt");
                File.Delete("failed.txt");
                FileStream fs1 = File.Create("passed.txt");
                FileStream fs2 = File.Create("failed.txt");
                fs1.Close();
                fs2.Close();
            }
            var streamP = new StreamWriter(File.OpenWrite("passed.txt"));
            var streamF = new StreamWriter(File.OpenWrite("failed.txt"));
            streamP.WriteLine("Surname Name Avg");
            streamF.WriteLine("Surname Name Avg");
            foreach (var item in studentList)
            {
                double avg = 0;
                foreach (var result in item.results)
                {
                    avg = avg + result;
                }
                double finalPoints = ((avg / item.results.Count) * 0.3) + (item.ExamResult * 0.7);

                if (finalPoints >= 5)
                {
                    streamP.WriteLine("{0} {1} {2:N2}", item.LastName, item.Name, finalPoints);
                }
                else
                {
                    streamF.WriteLine("{0} {1} {2:N2}", item.LastName, item.Name, finalPoints);
                }


            }
        }
    }
}
