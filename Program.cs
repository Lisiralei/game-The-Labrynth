﻿using OOPgameLbrynth;
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


    }


    abstract class Entity : GameObject
    {

    }

    class Projectile : Entity, MovementBehaviour
    {
        public void Move(Position targetPosition)
        {

        }
    }

    class Archer : Entity, MovementBehaviour, AttackBehaviour
    {
        public void Move(Position targetPosition)
        {

        }

        public void AttackTarget(Entity target)
        {

        }


    }

    class Knight : Entity, MovementBehaviour, AttackBehaviour
    {
        public void Move(Position targetPosition)
        {

        }

        public void AttackTarget(Entity target)
        {

        }
    }

    class DungeonWall : GameObject
    {
        DungeonWall(Position WallPosition)
        {
            Position = WallPosition;
            passable = false;
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
                mapTiles[i, 0] = mapTiles[i, width - 1] = new DungeonWall();
                for (int j = 1; j < width - 1; j++)
                    mapTiles[i, j] = new Empty();
            }
            for (int i = 0; i < width; i++)
            {
                mapTiles[0, i] = mapTiles[height - 1, i] = new DungeonWall();
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

abstract class MapTile
    {
        public Position tilePosition;
        public List<Object> ObjectsWithinTile;
        readonly bool passable;
    }

    class RoadTile : MapTile
    {

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
