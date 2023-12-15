using System.ComponentModel;

namespace ClassLibrary1;

public class Threads
{
    public static void Demo()
    {
        var data = 5;
        var thread = new Thread(new ThreadStart(() =>
        {
            Thread.Sleep(1000);
            Console.WriteLine("Hello from thread");
            // var data = 5;
            data *= 5;
            // -: how to return data...
            // -: how to cancel..
            // -: how to handle exceptions..
            // -: how to handle threadpool..
            // -: how to call next thread?..

        }));
        thread.Start();
        thread.Join(); // wait for thread to finish
        // thread.Abort();
        Console.WriteLine($"data: {data}");
    }

    private static void Bw_DoWork(object sender, DoWorkEventArgs e)
    {
        throw new NotImplementedException();
    }
}
