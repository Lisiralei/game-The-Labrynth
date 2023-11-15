namespace OOPgameLbrynth.components
{
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
				if (!(selectedMapTile == null) && selectedMapTile.Passable && selectedMapTile.ObjectsWithinTile.Count == 0)
				{
					selectedMapTile.ObjectsWithinTile.Add(entity);
					selectedMapTile.UpdatePassable();
					entity.Position = spawnPosition;
					spawned = true;
					entities.Add(entity);
				}
				else spawnTries++;
			}

		}

		public void UpdateEntities()
		{
			for (int i = 0; i < entities.Count; i++) UpdateEntity(entities[i]);
		}

		public void MakeEntityTryAttack(AgressiveEntity entity)
		{
			if (entity is Archer)
			{
				entity = entity as Archer;
			}



			for (int i = 0; i < entity.vision.Count; i++)
			{
				Position checkPosition = new Position(entity.vision[i]);
				MapTile checkTile = currentMap[new Position(checkPosition)];

				if (!(checkTile == null) && !(checkTile.ObjectsWithinTile == null))
				{
					for (int j = 0; j < checkTile.ObjectsWithinTile.Count; j++)
					{
						if (checkTile.ObjectsWithinTile[j] is PlayerCharacter)
						{
							PlayerCharacter playerCharacter = checkTile.ObjectsWithinTile[j] as PlayerCharacter;
							if (entity is Archer)
							{
								(int, int) offset;
								int dx = playerCharacter.Position.x - entity.Position.x;
								int dy = playerCharacter.Position.y - entity.Position.y;

                                int xOffset = dx == 0 ? Math.Sign(dx) * 1 : dx;
								int yOffset = dy == 0 ? Math.Sign(dy) * 1 : dy;

								offset = (xOffset,  yOffset);

								Projectile projectile = (entity as Archer).shootAtTarget(offset);
							}
							entity.AttackTarget(playerCharacter);

							entity.actionCooldown = 1000;
								
						}
					}
				}
			}
			
		}

		public void TryMoveEntity(Entity entity)
		{
			Random moveSelector = new Random();
			MapTile currentEntityTile = currentMap[entity.Position];

			Dictionary<int, (int, int)> movements = new Dictionary<int, (int, int)>()
			{
				{ 0, (0, -1) },
				{ 1, (1, 0) },
				{ 2, (0, 1) },
				{ 3, (-1, 0) },


			};
			List<MapTile> availableMoves = new List<MapTile>();
			for (int i = 0; i < 4; i++ )
			{
				Position target = new Position(entity.Position + movements[i]);

				PlayerCharacter? playerCharacter = null;


				MapTile targetTile = currentMap[target];
				for (int j = 0; j < targetTile.ObjectsWithinTile.Count; j++)
				{
					if (targetTile.ObjectsWithinTile[j] is PlayerCharacter)
					{
						playerCharacter = targetTile.ObjectsWithinTile[j] as PlayerCharacter;
					}
				}

				if (playerCharacter == null)
				{
					if (targetTile.Passable) availableMoves.Add(targetTile);

				}
				else
				{
					if (targetTile.Passable && !targetTile.ObjectsWithinTile.Contains(playerCharacter))
					{ 
						availableMoves.Add(targetTile);
					}
				}
				
			}
			if (availableMoves.Any())
			{
				MapTile selectedTile = availableMoves[moveSelector.Next(availableMoves.Count)];
				selectedTile.ObjectsWithinTile.Add(entity);
				selectedTile.UpdatePassable();
				currentEntityTile.ObjectsWithinTile.Remove(entity);
				currentEntityTile.UpdatePassable();
				entity.Position = new Position(selectedTile.tilePosition);
				entity.movementCooldown = 1000;
			}
		}

		public void UpdateEntity(Entity entity)
		{
			if (entity.active)
			{
				if (entity is AgressiveEntity)
				{
					entity.UpdateVision(currentMap);

					if (entity.actionCooldown == 0) {
						MakeEntityTryAttack(entity as AgressiveEntity);
					}
					
					if (entity.movementCooldown == 0)
					{
						TryMoveEntity(entity);
					}

					if (entity.movementCooldown > 0) { entity.movementCooldown -= 10; }
					if (entity.movementCooldown < 0) { entity.movementCooldown = 0; }
					if (entity.actionCooldown > 0) { entity.actionCooldown -= 50; }
					if (entity.actionCooldown < 0) { entity.actionCooldown = 0; }
				}
			}
		}
	}

	class PlayerControls
	{
		private PlayerCharacter character;

		public Map? currentMap;

		public bool finished = false;

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

			if (character.Position == currentMap.finish.Position)
			{

				Thread.Sleep(100);

				Console.Clear();

				Console.WriteLine("Congratulations");


				Thread.Sleep(5000);

				finished = true;


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
				currentMap[targetEntities[i].Position].UpdatePassable();
			}

		}
	}
}
