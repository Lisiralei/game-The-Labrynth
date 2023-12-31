﻿namespace OOPgameLbrynth.components
{
	class Game
	{
		public PlayerCharacter? player;
		public PlayerControls? controls;
		private Map? currentMap;
		private MazeGenerator? mazeGen;

		private GameRenderer? activeRenderer;

		private EntityController? entityController;


		int fps = 20;
		bool exitCall = false;

		public int KnightsToSpawn { get; set; }
		public int ArchersToSpawn { get; set; }

		public void SetFPS(int fps)
		{
			if (fps < 5 || fps > 240) this.fps = 20;
			else this.fps = fps;
		}

		public Game()
		{
			KnightsToSpawn = 4;
			ArchersToSpawn = 4;

			player = new PlayerCharacter(new Position(1, 1));

			controls = new PlayerControls(player, null);

			entityController = new EntityController();
		}

		public void Start()
		{
			currentMap = new Map();
			currentMap.Create();
			mazeGen = new MazeGenerator(currentMap);
			mazeGen.generateMazeFullyConnected(new Position(1, 1));
			controls.currentMap = currentMap;

			Position finishPosition = new Position(currentMap.width - 2, currentMap.height - 2);
			Finish exit = new Finish(finishPosition);


			currentMap[player.Position].ObjectsWithinTile.Add(player);

			currentMap[finishPosition].groundTile = exit;
			currentMap.finish = exit;

			entityController.currentMap = currentMap;


			for (int i = 0; i < KnightsToSpawn; i++)
			{
				entityController.SpawnEntity(new Knight(new Position(1, 1)));
			}

			for (int i = 0; i < ArchersToSpawn; i++)
			{
				entityController.SpawnEntity(new Archer(new Position(1, 1)));
			}

			activeRenderer = new GameRenderer(currentMap, player);
 


		}


		public void Update()
		{
			while (!exitCall)
			{
				if (player.getPlayerHealth() <= 0)
				{
                    Thread.Sleep(100);

                    Console.Clear();

                    Console.WriteLine("You died");

                    player = new PlayerCharacter(new Position(1, 1));
                    entityController = new EntityController();
                    controls = new PlayerControls(player, currentMap);

                    activeRenderer.player = player;

                    Thread.Sleep(5000);

                    Start();
                }
				Thread.Sleep(1000 / fps);
				if (controls.finished)
				{

					player = new PlayerCharacter(new Position(1, 1));
					entityController = new EntityController();
					controls = new PlayerControls(player, currentMap);

					activeRenderer.player = player;

					Start();
					controls.finished = false;


				}
				entityController.UpdateEntities();

				Console.Clear();
				activeRenderer.DrawFrame();

				if (Console.KeyAvailable)
					KeyDownFunction(Console.ReadKey(true).Key);
				Console.WriteLine("");

			}
		}

		private void KeyDownFunction(ConsoleKey key)
		{
			switch (key)
			{
				case ConsoleKey.Escape:
					exitCall = true;
					break;

				case ConsoleKey.W:
					controls.Move(new Position(player.Position.x, player.Position.y - 1));
					break;
				case ConsoleKey.UpArrow:
					controls.Move(new Position(player.Position.x, player.Position.y - 1));
					break;

				case ConsoleKey.S:
					controls.Move(new Position(player.Position.x, player.Position.y + 1));
					break;

				case ConsoleKey.DownArrow:
					controls.Move(new Position(player.Position.x, player.Position.y + 1));
					break;

				case ConsoleKey.A:
					controls.Move(new Position(player.Position.x - 1, player.Position.y));
					break;

				case ConsoleKey.LeftArrow:
					controls.Move(new Position(player.Position.x - 1, player.Position.y));
					break;

				case ConsoleKey.D:
					controls.Move(new Position(player.Position.x + 1, player.Position.y));
					break;

				case ConsoleKey.RightArrow:
					controls.Move(new Position(player.Position.x + 1, player.Position.y));
					break;

				case ConsoleKey.E:
					controls.Attack();
					break;
			}
		}
	}
}
