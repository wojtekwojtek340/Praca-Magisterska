using RaspberryServer.RaspberryBoard;

Console.WriteLine("Server Open");
var raspberryProvider = new RaspberryProvider();
raspberryProvider.Start();