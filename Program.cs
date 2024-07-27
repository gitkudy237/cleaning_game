using CleaningGame2;

Point bottomR = new Point(25, 25);

Game game = new Game(bottomR, 70, 300);
game.InitGame();
game.DisplayBoard();
Console.WriteLine("Press enter to start the game.");
Console.ReadLine();
game.Start();