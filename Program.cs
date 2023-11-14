using OOPgameLbrynth;
using System.ComponentModel;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace OOPgameLbrynth
{

	struct Position
	{
		public int x;
		public int y;
		public Position(int x, int y) { this.x = x; this.y = y; }
		public Position(Position position) { this.x = position.x; this.y = position.y; }
	}

	interface MovementBehaviour
	{
		public void Move(Position targetPosition);
	}

	interface AttackBehaviour
	{
		public void AttackTarget(Entity target);
	}


	abstract class GameObject
	{
		public int Id { get; set; }

		protected char symbol = ' ';
		public Position Position { get; set; }

		public bool passable;

		public GameObject(Position pos)
		{
			Position = pos;
		}

		public char GetRepr()
		{
			return symbol;
		}
	}


	abstract class Entity : GameObject
	{
		private int health = 100;
		private bool active = true;
		public Entity(Position EntityPosition) : base(EntityPosition)
		{

		}

		public void TakeDamage(int damage)
		{
			if (active)
			{
				health -= damage;
				if (health < 0)
				{
					Perish();
				}
			}
			else { }
		}
		public void Perish()
		{
			active = false;
			passable = true;

		}
	}

	class Projectile : Entity, MovementBehaviour, AttackBehaviour
	{
		public uint damage;
		public Projectile(Position ProjectPosition) : base(ProjectPosition)
		{

		}
		public void Move(Position targetPosition)
		{

		}
		public void AttackTarget(Entity targetEntity)
		{
			targetEntity.TakeDamage((int)damage);
		}
	}

	class Archer : Entity, MovementBehaviour, AttackBehaviour
	{
		public Archer(Position EntityPosition) : base(EntityPosition)
		{
			symbol = 'D';
		}
		public void Move(Position targetPosition)
		{

		}

		public void AttackTarget(Entity target)
		{

		}


	}

	class Knight : Entity, MovementBehaviour, AttackBehaviour
	{
		public uint damage;
		public Knight(Position EntityPosition) : base(EntityPosition)
		{
			symbol = 'k';
			damage = 10;
		}

		public void Move(Position targetPosition)
		{

		}

		public void AttackTarget(Entity target)
		{

		}
	}

	class PlayerCharacter : Entity, AttackBehaviour
	{
		public uint damage;
		public PlayerCharacter(Position PlayerPosition) : base(PlayerPosition)
		{
			symbol = 'p';
		}

		public void AttackTarget(Entity target)
		{

		}


	}



	class Map
	{
		public MapTile[,] mapTiles;
		public int height;
		public int width;

		public Map()
		{
			Create();
		}

		public MapTile this[Position position]
		{
			get => mapTiles[position.y, position.x];
			set => mapTiles[position.y, position.x] = value;

		}
		public MapTile this[int x, int y]
		{
			get => mapTiles[y, x];
			set => mapTiles[y, x] = value;
		}


		private void SetOuterWalls(int height, int width)  //Infill Edges with impassable walls
		{
			for (int i = 1; i < height - 1; i++)
			{
				mapTiles[i, 0] = new MapTile(
					new DungeonWall(new Position(i, 0)),
					new Position(i, 0)
					);
				mapTiles[i, width - 1] = new MapTile(
					new DungeonWall(new Position(i, width - 1)),
					new Position(i, width - 1)
					);

				//for (int j = 1; j < width - 1; j++)
				//	mapTiles[i, j] = new MapTile(
				//		new RoadTile(new Position(i, j)),
				//		new Position(i, j)
				//		);
			}
			for (int i = 0; i < width; i++)
			{
				mapTiles[0, i] = new MapTile(
					new DungeonWall(new Position(0, i)),
					new Position(0, i)
					);
				mapTiles[height - 1, i] = new MapTile(
					new DungeonWall(new Position(height - 1, i)),
					new Position(height - 1, i)
					);
			}
		}

		public void Create()
		{
			height = 21;
			width = 41;
			mapTiles = new MapTile[height, width];

			SetOuterWalls(height, width);

			
		}


	}
}


class MapTile
{
	public Position tilePosition;
	public GroundTile groundTile;
	public List<GameObject> ObjectsWithinTile;

	public bool Passable { get; protected set; }

	public MapTile(GroundTile? groundTile, Position tilePosition)
	{
		this.groundTile = groundTile;
		UpdatePassable();
		
		this.tilePosition = tilePosition;
		ObjectsWithinTile = new List<GameObject>();
	}

	public char GetRepr()
	{
		return (
			(ObjectsWithinTile.Count == 0)
			? (groundTile.GetRepr())
			: (ObjectsWithinTile[ObjectsWithinTile.Count - 1].GetRepr())
			);
	}

	public void UpdatePassable()
	{
		bool isPassable = true;

		if (groundTile != null)
		{
			if (groundTile.passable == true)
			{
				if (ObjectsWithinTile != null)
				{
					for (int i = 0; i < ObjectsWithinTile.Count; i++)
					{
						if (ObjectsWithinTile[i].passable == true)
						{
						}
						else isPassable = false;
					}
				}
			}
			else isPassable = false;
		}
		else isPassable = false;
		
		Passable = isPassable;
	}
}

abstract class GroundTile : GameObject
{
	public GroundTile(Position position) :base(position)
	{
		Position = position;
	} 


}

class RoadTile : GroundTile
{
	public RoadTile(Position tilePosition) : base(tilePosition)
	{
		Position = tilePosition;
		passable = true;
	}
}

class DungeonWall : GroundTile
{
	//protected new char symbol = '#';
	public DungeonWall(Position WallPosition) : base(WallPosition)
	{
		symbol = '#';
		passable = false;
	}
}


internal class Program
{
	static void Main(string[] args)
	{
		Game game = new Game();
		game.Start();
		game.Update();
	}
}


class GameRenderer
{
	private Map currentMap;
	public string? CreatedFrame { get; private set; }

	public GameRenderer(Map map)
	{
		currentMap = map;
	}


	private void CalculateFrame()
	{
		CreatedFrame = "";
		for (int i = 0; i < currentMap.height; i++)
		{
			for (int j = 0; j < currentMap.width; j++)
			{
				CreatedFrame += (currentMap.mapTiles[i, j].GetRepr()).ToString();
				//Console.Write(mapTiles[i, j].GetRepr());
			}
			CreatedFrame += "\n";

			//Console.WriteLine();
		}
	}
	public void DrawFrame()
	{
		CalculateFrame();
		Console.Write(CreatedFrame);
	}
	
	
}


class MazeGenerator
{
	private Map mazeMap;
	//private List<int>[,] visitedTiles = new List<MapTile>();
	//private List<int>[,] totalTiles;
	//private List<int>[,] canBeVisited = new List<MapTile>();
	//private Position currentTile;
	Random randomness = new Random();



	public void pathDiggerRecursive(Position fromPosition)
	{
		mazeMap[fromPosition] = new MapTile(new RoadTile(fromPosition), fromPosition);

		int[] directive = { 0, 0, 0, 0 };

		int directionSelect;

		for (int ordinal = 4; ordinal > 0; ordinal--)
		{
			directionSelect = randomness.Next(ordinal);

			for (int i = 0; i <= directionSelect; i++)
			{
				if (directive[i] == 1)
				{
					directionSelect++;
				}
			}
			directive[directionSelect] = 1;

			switch (directionSelect)
			{
				case 0:
					if (fromPosition.x > 2) //outOfBounds checks
					{
						Position digPosition = new Position(fromPosition.x - 1, fromPosition.y);
						Position destinationPosition = new Position(fromPosition.x - 2, fromPosition.y);

						if (mazeMap[destinationPosition] == null) // Check if the digger has been there
						{
							mazeMap[digPosition] = new MapTile(new RoadTile(digPosition), digPosition);
							pathDiggerRecursive(destinationPosition);
						}
					}
					break;

				case 1:
					if (fromPosition.y > 2)
					{
						Position digPosition = new Position(fromPosition.x, fromPosition.y - 1);
						Position destinationPosition = new Position(fromPosition.x, fromPosition.y - 2);

						if (mazeMap[destinationPosition] == null)
						{
							mazeMap[digPosition] = new MapTile(new RoadTile(digPosition), digPosition);
							pathDiggerRecursive(destinationPosition);
						}
					}
					break;

				case 2:
					if (fromPosition.x < mazeMap.width - 3)
					{
						Position digPosition = new Position(fromPosition.x + 1, fromPosition.y);
						Position destinationPosition = new Position(fromPosition.x + 2, fromPosition.y);

						if (mazeMap[destinationPosition] == null)
						{
							mazeMap[digPosition] = new MapTile(new RoadTile(digPosition), digPosition);
							pathDiggerRecursive(destinationPosition);
						}
					}
					break;

				case 3:
					if (fromPosition.y < mazeMap.height - 3)
					{
						Position digPosition = new Position(fromPosition.x, fromPosition.y + 1);
						Position destinationPosition = new Position(fromPosition.x, fromPosition.y + 2);

						if (mazeMap[destinationPosition] == null)
						{
							mazeMap[digPosition] = new MapTile(new RoadTile(digPosition), digPosition);
							pathDiggerRecursive(destinationPosition);
						}
					}
					break;

			}

		}
	}

	public MazeGenerator(Map mazeMap)
	{
		this.mazeMap = mazeMap;
	}

	public void fillVoidWithWalls()
	{
		for (int i = 0; i < mazeMap.width; i++)
		{
			for (int j = 0; j < mazeMap.height; j++)
			{
				if (mazeMap[i, j] == null)
				{
					Position pos = new Position(i, j);

					mazeMap[i, j] = new MapTile(new DungeonWall(pos), pos);
				}
			}
		}
	}


	public void generateMazeFullyConnected(Position startingPosition)
	{
		pathDiggerRecursive(startingPosition);

		fillVoidWithWalls();
	}
}


class EntityController
{
	public Map? currentMap;

	private List<Entity> entities = new List<Entity>();



	public void SpawnEntity(Entity entity)
	{
		Random randomness = new Random();

		int width = currentMap.width;
		int height = currentMap.height;
		Position spawnPosition;

		bool spawned = false;
		int spawnTries = 0;
		while (!spawned)
		{
			spawnPosition = new Position(randomness.Next(width - 2) + 1, randomness.Next(height - 2) + 1);
			MapTile selectedMapTile = currentMap[spawnPosition];
			if (!(selectedMapTile == null) && (selectedMapTile.Passable) && (selectedMapTile.ObjectsWithinTile.Count == 0))
			{
				selectedMapTile.ObjectsWithinTile.Add(entity);
				selectedMapTile.UpdatePassable();
				entity.Position = spawnPosition;
				spawned = true;
			}
			else spawnTries++;
		}
	}


}

class PlayerControls
{
	private PlayerCharacter character;

	public Map? currentMap;
	

	public PlayerControls(PlayerCharacter character, Map? currentMap)
	{
		this.character = character;
		this.currentMap = currentMap;
	}

	public void Move(Position target)
	{
		MapTile targetTile = currentMap[target];

		if (targetTile.Passable)
		{
			MapTile currentPlayerTile = currentMap[character.Position];

			targetTile.ObjectsWithinTile.Add(character);
			currentPlayerTile.ObjectsWithinTile.Remove(character);
			character.Position = new Position(target);
		}

	}

	public void Attack()
	{
		List<Entity> targetEntities = new List<Entity>();
		if (character.Position.x > 1) //check for targets west of player
		{
			MapTile checkTile = currentMap[new Position(character.Position.x - 1, character.Position.y)];

			for (int i = 0; i < checkTile.ObjectsWithinTile.Count; i++)
			{
				if (checkTile.ObjectsWithinTile[i] is Entity)
				{
					targetEntities.Add(checkTile.ObjectsWithinTile[i] as Entity);
				}
			}
		}
		if (character.Position.y > 1) // check for targets north of player
		{
			MapTile checkTile = currentMap[new Position(character.Position.x, character.Position.y - 1)];

			for (int i = 0; i < checkTile.ObjectsWithinTile.Count; i++)
			{
				if (checkTile.ObjectsWithinTile[i] is Entity)
				{
					targetEntities.Add(checkTile.ObjectsWithinTile[i] as Entity);
				}
			}
		}
		if (character.Position.x < currentMap.width - 2) // check for targets east of player
		{
			MapTile checkTile = currentMap[new Position(character.Position.x + 1, character.Position.y)];

			for (int i = 0; i < checkTile.ObjectsWithinTile.Count; i++)
			{
				if (checkTile.ObjectsWithinTile[i] is Entity)
				{
					targetEntities.Add(checkTile.ObjectsWithinTile[i] as Entity);
				}
			}
		}

		if (character.Position.y < currentMap.height - 2) // check for targets south of player
		{
			MapTile checkTile = currentMap[new Position(character.Position.x, character.Position.y + 1)];

			for (int i = 0; i < checkTile.ObjectsWithinTile.Count; i++)
			{
				if (checkTile.ObjectsWithinTile[i] is Entity)
				{
					targetEntities.Add(checkTile.ObjectsWithinTile[i] as Entity);
				}
			}
		}

		for (int i = 0; i < targetEntities.Count; i++)
		{
			targetEntities[i].TakeDamage((int)character.damage);
		}
		
	}
}


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

	public void SetFPS(int fps)
	{
		if (fps < 5 || fps > 240) this.fps = 20;
		else this.fps = fps;
	}

	public Game()
	{
		player = new PlayerCharacter(new Position(1, 1));

		controls = new PlayerControls(player, null);

		entityController = new EntityController();
	}

	public void Start()
	{
		currentMap = new Map();
		currentMap.Create();
		controls.currentMap = currentMap;
		


		mazeGen = new MazeGenerator(currentMap);
		mazeGen.generateMazeFullyConnected(new Position(1, 1));

        currentMap[player.Position].ObjectsWithinTile.Add(player);


        activeRenderer = new GameRenderer(currentMap);


	}


	public void Update()
	{
		while (!exitCall)
		{
			Thread.Sleep(1000 / fps);

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
		}
	}
}
