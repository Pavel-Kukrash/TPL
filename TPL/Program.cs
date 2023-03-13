
Task task1 = new Task(() =>
{
    Console.WriteLine("Task1 Starts");
    Thread.Sleep(1000);
    Console.WriteLine("Task1 Ends");
});
task1.Start();
//task1.RunSynchronously();
Console.WriteLine($"task1 Id: {task1.Id}");
Console.WriteLine($"task1 Status: {task1.Status}");

Task task2 = Task.Factory.StartNew(() => Console.WriteLine("Task2 is executed"));

Task task3 = Task.Run(() => Console.WriteLine("Task3 is executed"));

task1.Wait();
task2.Wait();
task3.Wait();

var outer = Task.Factory.StartNew(() =>      // outer task
{
    Console.WriteLine("Outer task starting...");
    var inner = Task.Factory.StartNew(() =>  // inner task
    {
        Console.WriteLine("Inner task starting...");
        Thread.Sleep(2000);
        Console.WriteLine("Inner task finished.");
    }, TaskCreationOptions.AttachedToParent);
});
outer.Wait(); // waiting for an outer task execution 

Task[] task4 = new Task[3]
{
    new Task(() => Console.WriteLine("First Task")),
    new Task(() => Console.WriteLine("Second Task")),
    new Task(() => Console.WriteLine("Third Task"))
};
foreach (var t in task4)
{
    t.Start();
}

Task[] task5 = new Task[3];
int j = 1;
for (int i = 0; i < task5.Length; i++)
{
    //task5[i] = Task.Factory.StartNew(() => Console.WriteLine($"Task {j++}"));
    task5[i] = new Task(() =>
    {
        Thread.Sleep(1000);// process simulation
        Console.WriteLine($"Task{i} finished");
    });
    task5[i].Start();
}

Task.WaitAll(task5);

// Result return

int n1 = 3;
int n2 = 2;

Task<int> sumTask = new Task<int>(() => Sum(n1, n2));
Task printTask = sumTask.ContinueWith(t => PrintResult(t.Result));

sumTask.Start();
printTask.Wait();

int result = sumTask.Result;
Console.WriteLine($"{n1} + {n2} = {result}"); // 3 + 2 = 5


Console.WriteLine("End of Main");

// Class Parallel

Parallel.Invoke
(
    Print,
    () =>
    {
        Console.WriteLine($"{Task.CurrentId} task's execution");
        Thread.Sleep(3000);
    }
    //() => Square(5)
);

CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
CancellationToken token = cancelTokenSource.Token;

new Task(() =>
{
    Thread.Sleep(1000);
    cancelTokenSource.Cancel();
}).Start();

try
{ 
Parallel.For(1, 5, new ParallelOptions {CancellationToken = token }, Square);
//ParallelLoopResult resultP = Parallel.ForEach<int>(

//    new List<int>() { 1, 3, 5, 8 },
//    Square
//    );

}
catch (OperationCanceledException) 
{
    Console.WriteLine("Transaction was interrapted");
}

finally
{
    cancelTokenSource.Dispose();
}


Console.ReadKey();

void Print()
{
    Console.WriteLine($"{Task.CurrentId} task's execution");
    Thread.Sleep(3000);
}

void Square(int n)
{
    Console.WriteLine($"{Task.CurrentId} task's execution");
    Thread.Sleep(3000);
    Console.WriteLine($"Result is {n * n}");
}


int Sum(int a, int b) => a + b;
void PrintResult(int sum) => Console.WriteLine($"Sum: {sum}");