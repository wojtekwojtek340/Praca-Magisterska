using RaspberryServer.RaspberryBoard;
using System.Diagnostics;

Console.WriteLine("W8 for debuger");

while (!Debugger.IsAttached)
{
    Thread.Sleep(1000);
}

Console.WriteLine("Debuger Attached");
Console.WriteLine("Server Open");
var raspberryPiServer = new RaspberryPiServer();
raspberryPiServer.Start();