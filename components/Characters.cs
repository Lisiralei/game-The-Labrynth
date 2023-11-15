namespace OOPgameLbrynth.components
{
    class Projectile : AgressiveEntity
    {
        public Projectile(Position ProjectPosition) : base(ProjectPosition)
        {
            symbol = '-';
            health = 40;
        }

        public override void UpdateVision()
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


        public override void UpdateVision()
        {
            vision.Clear();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    vision.Add(new Position(Position + (i, j)));
                }
            }
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
            damage = 1;
            vision = new List<Position>();
            //UpdateVision();
        }

        public override void UpdateVision()
        {
            vision.Clear();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    vision.Add(new Position(Position + (i, j)));
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
            health = 10000000;
            symbol = 'p';
        }

        public void AttackTarget(Entity target)
        {

        }

        public void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            Console.Clear();

            Console.WriteLine($"You took {damage} damage");

            Thread.Sleep(500);


        }

        public override void UpdateVision()
        {

        }

    }

}
