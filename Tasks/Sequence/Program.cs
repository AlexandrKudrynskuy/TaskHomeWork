//Задание 1: Поиск самой длинной общей подпоследовательности между двумя строками с помощью многопоточности.
//В этом задании нужно реализовать алгоритм поиска длинной общей подпоследовательности между двумя строками
//с использованием многопоточности.
//Для выполнения задания нужно выполнить следующие шаги:
//Написать функцию, которая будет принимать на вход две строки и возвращать длину наибольшей общей подпоследовательности между ними. В качестве возвращаемого значения может быть целое число.
//Реализовать многопоточную версию функции. Для этого можно разбить обе строки на несколько частей и запустить для каждой пары частей по отдельному потоку. После завершения всех потоков необходимо собрать результаты и выбрать максимальную длину общей подпоследовательности.
//Общая подпоследовательность (англ. Longest Common Subsequence, LCS) -это последовательность элементов, которые встречаются в исходных последовательностях, но не обязательно на одной и той же позиции.
//Например, для двух строк "abcde" и "ace" общей подпоследовательностью будет "ace", так как она содержит элементы, которые есть и в первой, и во второй строке (a и e в первой строке, c во второй строке), и при этом не нарушается порядок элементов в общей последовательности.
//Поиск общей подпоследовательности является важной задачей в биоинформатике, например, при сравнении геномов. Одним из классических алгоритмов для нахождения длины наибольшей общей подпоследовательности является динамическое программирование.



using System.Diagnostics;
string directoeyPath =  Directory.GetCurrentDirectory();
var taskReadFirst = Task<string>.Run(() => { return File.ReadAllText(directoeyPath + "\\" + "1.txt"); });
var taskReadSecond = Task<string>.Run(() => { return File.ReadAllText(directoeyPath + "\\" + "2.txt"); });

string strFirst = taskReadFirst.Result;
string strSecond = taskReadSecond.Result;

var sw = new Stopwatch();
sw.Start();
int res = SubSequence(strFirst, strSecond);
//Console.WriteLine(strFirst);
//Console.WriteLine(strSecond);
Console.WriteLine($"Count = {res}");
Console.WriteLine("_____________________\n");
sw.Stop();
Console.WriteLine($"time {sw.ElapsedMilliseconds}");

int numberOfParts = 5;
var arrFirst = SplitStr(strFirst, numberOfParts);
var arrSecond = SplitStr(strSecond, numberOfParts);

//Console.BackgroundColor = ConsoleColor.Blue;
//Console.ForegroundColor = ConsoleColor.Yellow;
//foreach (var it in arrFirst)
//{
//    Console.Write($"{it}\t");
//}
//Console.ResetColor();

//Console.WriteLine("\n_____________________\n");
//Console.BackgroundColor = ConsoleColor.Yellow;
//Console.ForegroundColor = ConsoleColor.DarkBlue;
//foreach (var it in arrSecond)
//{
//    Console.Write($"{it}\t");
//}
//Console.ResetColor();
//Console.WriteLine("\n_____________________\n");


var tasks = new List<Task<int>>();
sw.Start();
for (int i = 0; i < numberOfParts; i++)
{
    tasks.Add(Task<int>.Run(() => { return SubSequence(arrFirst[i], arrSecond[i]); }));
    Thread.Sleep(100); ///// ?
}
Task.WaitAll(tasks.ToArray());
int resAfterPart = 0;

foreach (var ts in tasks)
{
    resAfterPart += ts.Result;
}
Console.WriteLine($"Count after partition = {resAfterPart}");
sw.Stop();
Console.WriteLine($"time after partition {sw.ElapsedMilliseconds}");


string[] SplitStr(string str, int count)
{
    string[] arr = new string[count];
    int pos = 0;
    for (int i = 0; i < count; i++)
    {
        if (i == count - 1)
        {
            arr[i] = str.Substring(pos, str.Length - pos);
            //Console.Write($"ar ={arr[i]}, count = {arr[i].Length}");
            //Console.WriteLine();
            break;
        }
        arr[i] = str.Substring(pos, str.Length / count);
        //Console.Write($"ar ={arr[i]}, count = {arr[i].Length}");
        //Console.WriteLine();
        pos += str.Length / count;
    }
    return arr;

}


int SubSequence(string firstStr, string secondStr)
{
    int a = firstStr.Length + 1;
    int b = secondStr.Length + 1;
    int[,] ar = new int[a, b];

    for (int i = 1; i < ar.GetLength(0); i++)
    {
        for (int j = 1; j < ar.GetLength(1); j++)
        {
            if (firstStr[i - 1] == secondStr[j - 1])
            {
                ar[i, j] = ar[i - 1, j - 1] + 1;
            }
            else
            {
                ar[i, j] = (ar[i - 1, j] > ar[i, j - 1]) ? ar[i - 1, j] : ar[i, j - 1];
            }
        }
    }
    //for (int i = 0; i < ar.GetLength(0); i++)
    //{
    //    for (int j = 0; j < ar.GetLength(1); j++)
    //    {
    //        Console.Write($"{ar[i, j]} ");
    //    }
    //    Console.WriteLine();

    //}
    return ar[ar.GetLength(0) - 1, ar.GetLength(1) - 1];

}