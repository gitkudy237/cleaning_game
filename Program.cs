using CleaningGame2;

Point bottomR = new Point(25, 50);

Game game = new Game(bottomR, 20, 100);
game.InitGame();
game.DisplayBoard();

Console.ReadLine();
game.Start();