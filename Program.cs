using OOPgameLbrynth;
using System.Numerics;

namespace OOPgameLbrynth
{

    struct Position
    {
        public int x;
        public int y;
        public Position(int x, int y) { this.x = x; this.y = y; }
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

        public char GetSym()
        {
            return symbol;
        }
    }

    class Empty : GameObject
    {
        public Empty(Position EmptyPosition):base(EmptyPosition)
        {
            passable = true;
        }
    }

    class DungeonWall : GameObject
    {
        public DungeonWall(Position WallPosition):base(WallPosition)
        {
            passable = false;
            symbol = '#';
        }
    }

    abstract class Entity : GameObject
    {
        public Entity(Position EntityPosition) : base(EntityPosition)
        {

        }
    }

    class Projectile : Entity, MovementBehaviour
    {
        public Projectile(Position ProjectPosition) : base(ProjectPosition)
        {

        }
        public void Move(Position targetPosition)
        {

        }
    }

    class Archer : Entity, MovementBehaviour, AttackBehaviour
    {
        public Archer(Position EntityPosition) : base(EntityPosition)
        {

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
        public Knight(Position EntityPosition) : base(EntityPosition)
        {
        }

        public void Move(Position targetPosition)
        {

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

        public void Create()
        {
            height = 10;
            width = 20;
            mapTiles = new MapTile[height, width];
            for (int i = 1; i < height - 1; i++)
            {
                mapTiles[i, 0] = new MapTile(new DungeonWall(new Position(i,0)), new Position(i, 0), true);
                mapTiles[i, width - 1] = new MapTile(new DungeonWall(new Position(i, width-1)), new Position(i, width-1), true);

                for (int j = 1; j < width - 1; j++)
                    mapTiles[i, j] = new MapTile(new Empty(new Position(i, j)), new Position(i,j), true);
            }
            for (int i = 0; i < width; i++)
            {
                mapTiles[0, i] = new MapTile(new DungeonWall(new Position(0,i)), new Position(0,i), true);
                mapTiles[height - 1, i] = new MapTile(new DungeonWall(new Position(height-1,i)), new Position(height-1,i), true);
            }
        }

        public void Show()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write(mapTiles[i, j].GetSym());
                }
                Console.WriteLine();
            }
        }
    }



}

class MapTile
{
    public Position tilePosition;
    public GameObject MainTile;
    public List<GameObject> ObjectsWithinTile;
    public bool passable { get; protected set; }
    
    public MapTile(GameObject mainTile, Position tilePosition, bool passable)
    {
        MainTile = mainTile;
        this.passable = passable;
        this.tilePosition = tilePosition;
        ObjectsWithinTile = new List<GameObject>();
    }
    public char GetSym()
    {
        return ((ObjectsWithinTile.Count == 0) ?
            (MainTile.GetSym()) :
            (ObjectsWithinTile[ObjectsWithinTile.Count - 1].GetSym()));
    }
}

//class RoadTile : MapTile
//{
//    public RoadTile(GameObject mainTile,Position tilePosition)
//    {
//        this.tilePosition = tilePosition;
//        MainTile= mainTile;
//        passable = true;
//    }
//}


internal class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
        game.Update();
    }
}


class Game
{
    public GameObject? player;
    private Map? myMap;
    byte fps = 20;
    string str = "";
    bool exit = false;
    public void SetFPS(byte fps)
    {
        if (fps < 5 || fps > 240) this.fps = 20;
        else this.fps = fps;
    }
    public void Start()
    {
        myMap = new Map();
        myMap.Create();

    }

    public void Update()
    {
        Thread.Sleep(1000 / fps);
        Console.Clear();
        myMap.Show();
        if (Console.KeyAvailable)
            KeyDownFunction(Console.ReadKey(true).Key);
        Console.WriteLine(str);

        if (!exit) Update();
    }

    private void KeyDownFunction(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.Escape:
                exit = true;
                break;
            case ConsoleKey.W:

                break;
            case ConsoleKey.S:

                break;
            case ConsoleKey.A:

                break;
            case ConsoleKey.D:

                break;
        }
    }
}
