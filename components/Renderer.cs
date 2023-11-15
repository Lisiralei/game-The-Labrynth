namespace OOPgameLbrynth.components
{
	class GameRenderer
	{
		private Map currentMap;

		public  PlayerCharacter player;
		public string? CreatedFrame { get; private set; }

		public GameRenderer(Map map, PlayerCharacter player)
		{
			currentMap = map;
			this.player = player;
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
            CreatedFrame += $"\tPlayer's current health = {player.getPlayerHealth()}";
        }
		public void DrawFrame()
		{
			CalculateFrame();
			Console.Write(CreatedFrame);
		}
	}
}
