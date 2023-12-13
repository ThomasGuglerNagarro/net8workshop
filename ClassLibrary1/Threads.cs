namespace ClassLibrary1;

public class Threads
{
    public static void Demo()
    {
        var thread = new Thread(new ThreadStart(() =>
        {
            Thread.Sleep(1000);
            Console.WriteLine("Hello from thread");
            // -: how to return data...
            // -: how to cancel..
            // -: how to handle exceptions..
            // -: how to handle threadpool..
            // -: how to call next thread?..
        }));
        thread.Start();
        thread.Join(); // wait for thread to finish
    }
}