//Задание 2: Обработка большого объема данных с помощью многопоточности.
//В этом задании нужно обработать большой объем данных с помощью многопоточности.
//Для выполнения задания нужно выполнить следующие шаги:
//Написать функцию, которая будет обрабатывать данные. Это может быть, например, функция, 
//которая будет сортировать большой массив данных.
//Сгенерировать большой объем данных, который будет обрабатываться. Для этого можно использовать 
//функцию Enumerable.Range для генерации большого списка чисел.
//Разбить список данных на несколько частей и запустить для каждой части отдельный поток.
//После завершения всех потоков собрать результаты и объединить их в общий список.
using System.Diagnostics;
using System.Linq;


var arr = Enumerable.Range(1,100000).Select(x=>x*(new Random()).Next(1,100)).ToArray();
foreach (var item in arr)
{
    File.AppendAllText("random.txt", item + " ");
}

int countSplit=1000;
var arraySplit = Split1(arr, countSplit);

foreach (var item in arraySplit)
{
    foreach (var ma in item)
    {
        File.AppendAllText("Split.txt", ma.ToString() + " ");
    }
    File.AppendAllText("Split.txt", "\n");
}

var tasks = new List<Task>();
var watch = new Stopwatch();

foreach (var item in arraySplit)
{
    tasks.Add(Task.Run(() => SortArrayAsk(item)));
}


Task.WaitAny(tasks.ToArray());

int i = 0;
while (i < arraySplit.Count - 1)
{
    while (i < arraySplit.Count - 1)
    {
        arraySplit[i] = Marge(arraySplit[i], arraySplit[i + 1]);
        arraySplit.Remove(arraySplit[i + 1]);
        i++;
    }
    i = 0;
}
foreach (var item in arraySplit)
{
    foreach (var ma in item)
    {
        File.AppendAllText("Finich.txt", ma.ToString() + " ");
    }
    File.AppendAllText("Finich.txt", "\n");
}
void SortArrayAsk(int[] arr)
{ 
 Array.Sort(arr,(x,y)=>x.CompareTo(y));
}
List<int[]> PartArray(int[] mas, int countNewArray)
{
    var arrayOfArrays = new List<int[]>();
    for (int i = 0; i < countNewArray; i++)
    {
        arrayOfArrays.Add(new int[mas.Length/countNewArray]);
    }

    int pos = 0;
    for (int i = 0; i < countNewArray; i++)
    {
        Array.Copy(mas,pos,arrayOfArrays[i],4,5);
    }

    return arrayOfArrays;
}

int[] Marge(int[] mas1, int[] mas2, bool asc=true)
{
    int pos1 = 0;
    int pos2 = 0;
    var unionMas = new int[mas1.Length + mas2.Length];
    if (asc)
    {
        for (int i = 0; i < unionMas.Length; i++)
        {

            if (pos1 < mas1.Length && pos2 < mas2.Length)
            {
                if (mas1[pos1] < mas2[pos2])
                {
                    unionMas[i] = mas1[pos1];
                    pos1++;
                }
                else
                {
                    unionMas[i] = mas2[pos2];
                    pos2++;
                }
            }
            else
            {
                if (pos1 < mas1.Length)
                {
                    unionMas[i] = mas1[pos1];
                    pos1++;
                }
                else
                {
                    unionMas[i] = mas2[pos2];
                    pos2++;
                }
            }
        }
    }
    else
    {
        for (int i = 0; i < unionMas.Length; i++)
        {

            if (pos1 < mas1.Length && pos2 < mas2.Length)
            {
                if (mas1[pos1] > mas2[pos2])
                {
                    unionMas[i] = mas1[pos1];
                    pos1++;
                }
                else
                {
                    unionMas[i] = mas2[pos2];
                    pos2++;
                }
            }
            else
            {
                if (pos1 < mas1.Length)
                {
                    unionMas[i] = mas1[pos1];
                    pos1++;
                }
                else
                {
                    unionMas[i] = mas2[pos2];
                    pos2++;
                }
            }
        }
    }

    return unionMas;
}

List<int[]> Split1(int[] arr, int size)
{
    var res = arr.Select((s, i) => arr.Skip(i * size).Take(size)).Where(a => a.Any());
    var list = new List<int[]>();
    foreach (var item in res)
    {
        list.Add(item.ToArray());
    }
    return list;
}


