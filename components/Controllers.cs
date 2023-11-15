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
                            entity.AttackTarget(playerCharacter);
                        }
                    }
                }
            }
        }

        public void UpdateEntity(Entity entity)
        {
            if (entity is AgressiveEntity)
            {
                entity.UpdateVision();
                MakeEntityTryAttack(entity as AgressiveEntity);
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
