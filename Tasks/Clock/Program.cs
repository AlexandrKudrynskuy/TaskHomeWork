
//Напишите программу, которая создает две задачи на выполнение некоторой работы, 
//используя класс Task. Первая задача должна запустить бесконечный цикл и каждую секунду выводить на консоль текущее время. 
//Вторая задача должна ждать 5 секунд и затем выводить на консоль сообщение о том, что прошло 5 секунд. После запуска обеих задач,
//программа должна ждать их завершения и выводить сообщение о завершении работы каждой задачи.

var token1 = new CancellationTokenSource();
var token2 = new CancellationTokenSource();


var taskCurentTime = Task.Factory.StartNew(CurentTime, token1.Token);
var taskAfterFiveSec = Task.Factory.StartNew(AfterFiveSec, token2.Token);
try
{
    if (Console.ReadKey().Key == ConsoleKey.Enter)
    {
        token1.Cancel();
        token2.Cancel();
    }
}
catch (TaskCanceledException ex)
{
    Console.WriteLine(nameof(ex));
    Console.WriteLine(ex.Message);
}


Console.ReadLine();
void AfterFiveSec(object obj)
{
    var token = (CancellationToken)obj;
    token.ThrowIfCancellationRequested();
    int counter = 5;
    while (true)
    {
        if (token.IsCancellationRequested)
        {
            Console.WriteLine($"{nameof(AfterFiveSec)} завершив свою роботу");
            token.ThrowIfCancellationRequested();

        }

        Thread.Sleep(5000);
        Console.WriteLine($"Пройшло {counter} сек");
        counter += 5;

    }
}

void CurentTime(object obj)
{
    var token = (CancellationToken)obj;
    token.ThrowIfCancellationRequested();
    while (true)
    {
        if (token.IsCancellationRequested)
        {
            Console.WriteLine($"{nameof(CurentTime)} завершив свою роботу");
            token.ThrowIfCancellationRequested();
        }
        Console.WriteLine($"H:{DateTime.Now.Hour} M:{DateTime.Now.Minute} S:{DateTime.Now.Second}");
        Thread.Sleep(1000);
    }
}