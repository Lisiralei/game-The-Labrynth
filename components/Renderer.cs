namespace OOPgameLbrynth.components
{
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
                    CreatedFrame += currentMap.mapTiles[i, j].GetRepr().ToString() + ' ';
                }
                CreatedFrame += "\n";

            }
        }
        public void DrawFrame()
        {
            CalculateFrame();
            Console.Write(CreatedFrame);
        }
    }
}
