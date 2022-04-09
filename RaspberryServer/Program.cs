using RaspberryServer.RaspberryBoard;

Console.WriteLine("Server Open");
var raspberryPiServer = new RaspberryPiServer();
raspberryPiServer.Start();