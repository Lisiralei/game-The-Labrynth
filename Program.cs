using OOPgameLbrynth.components;
using System.Reflection.Metadata.Ecma335;


namespace OOPgameLbrynth
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Game game = new Game();
			game.KnightsToSpawn = 5;
			game.ArchersToSpawn = 5;
			game.Start();
			game.Update();
		}
	}
}
