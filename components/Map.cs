﻿namespace OOPgameLbrynth.components
{
    class Map
    {
        public MapTile[,] mapTiles;
        public int height;
        public int width;

        public Finish? finish;

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
            return
                ObjectsWithinTile.Count == 0
                ? groundTile.GetRepr()
                : ObjectsWithinTile[ObjectsWithinTile.Count - 1].GetRepr()
                ;
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
        public GroundTile(Position position) : base(position)
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
        public DungeonWall(Position WallPosition) : base(WallPosition)
        {
            symbol = '#';
            passable = false;
        }
    }

    class Finish : GroundTile
    {
        public Finish(Position position) : base(position)
        {
            symbol = 'F';
            passable = true;
        }
    }

    class MazeGenerator
        {
        private Map mazeMap;
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

}
