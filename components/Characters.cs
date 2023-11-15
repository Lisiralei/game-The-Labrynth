namespace OOPgameLbrynth.components
{
	class Projectile : AgressiveEntity
	{
		public (int, int) direction;
		public Projectile(Position ProjectPosition, (int, int) direction) : base(ProjectPosition)
		{
			this.direction = direction;
			active = true;
			symbol = '-';
			health = 40;
		}

		public override void UpdateVision(Map currentMap)
		{

		}
	}

	class Archer : AgressiveEntity
	{
		public Archer(Position EntityPosition) : base(EntityPosition)
		{
			symbol = 'D';
			vision = new List<Position>();
			//UpdateVision();
		}


		public override void UpdateVision(Map currentMap)
		{
			vision.Clear();
			for (int i = -3; i < 4; i++)
			{
				if (Position.x + i < currentMap.width && Position.x + i > 0)
				{
					vision.Add(new Position(Position + (i, 0)));
				}
			}

			for (int j = -3; j < 4; j++)
			{
				if (Position.y + j < currentMap.height && Position.y + j > 0)
				{
					if (Position + (0, j) != Position) vision.Add(new Position(Position + (0, j)));
				}
			}
		}

		public Projectile shootAtTarget((int, int) direction)
		{
			return new Projectile(new Position(this.Position + direction), direction) ;
		}

		public override void Perish()
		{
			base.Perish();
			symbol = '~'; // heh
		}

	}

	class Knight : AgressiveEntity
	{
		public Knight(Position EntityPosition) : base(EntityPosition)
		{
			symbol = 'K';
			damage = 1000;
			vision = new List<Position>();
			//UpdateVision();
		}

		public override void UpdateVision(Map currentMap)
		{
			vision.Clear();
			for (int i = -1; i < 2; i++)
			{
				for (int j = -1; j < 2; j++)
				{
					if (Position.x + i < currentMap.width && Position.x + i > 0 && Position.y + j <= currentMap.height && Position.y + j > 0)
					{
						vision.Add(new Position(Position + (i, j)));
					}
					
				}
			}
		}

		public override void Perish()
		{
			base.Perish();
			symbol = '~';
		}
	}

	class PlayerCharacter : Entity, AttackBehaviour
	{
		public uint damage;

		public PlayerCharacter(Position PlayerPosition) : base(PlayerPosition)
		{
			damage = 50;
			health = 10000;
			symbol = 'p';
		}

		public void AttackTarget(Entity target)
		{

		}

		public new void TakeDamage(int damage)
		{
			base.TakeDamage(damage);

			Console.Clear();

			Console.WriteLine($"You took {damage} damage");

			Thread.Sleep(500);


		}

		public override void UpdateVision(Map currentMap)
		{

		}

		public double getPlayerHealth()
		{
			return ((double)health)/100;
		}
	}

}
