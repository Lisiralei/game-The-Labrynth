namespace OOPgameLbrynth.components
{
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


        public char GetRepr()
        {
            return symbol;
        }
    }

    abstract class Entity : GameObject
    {
        private Map? currentMap;
        protected int health = 100;
        protected bool active = true;

        public List<Position> vision;

        public Entity(Position EntityPosition) : base(EntityPosition)
        {

        }

        public void TakeDamage(int damage)
        {
            if (active)
            {
                health -= damage;
                if (health < 0)
                {
                    Perish();
                }
            }
            else { }
        }
        public virtual void Perish()
        {
            active = false;
            passable = true;

        }

        public abstract void UpdateVision();

    }

    abstract class AgressiveEntity : Entity, MovementBehaviour, AttackBehaviour
    {
        public uint damage;
        public AgressiveEntity(Position EntityPosition) : base(EntityPosition)
        {

        }

        public void Move(Position targetPosition)
        {
            Position = targetPosition;
        }
        public virtual void AttackTarget(Entity targetEntity)
        {
            targetEntity.TakeDamage((int)damage);
        }

    }
}
