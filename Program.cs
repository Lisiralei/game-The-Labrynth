namespace OOPgameLbrynth
{

    class Position
    {
        public int x;
        public int y;
        public Position(int x, int y) { this.x = x; this.y = y; }
    }

    interface MovementBehaviour
    {
        public void Move();
    }

    interface AttackBehaviour
    {
        public void AttackTarget(Entity target);
    }


    abstract class Object
    {
        public int Id { get; set; }
        public Position Position { get; set; }

        public bool passable;

    }


    abstract class Entity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Position Position { get; set; }

        public abstract 

    }

    class Projectile : Entity, MovementBehaviour
    {
        public void Move()
        {

        }
    }

    class Archer : Entity, MovementBehaviour, AttackBehaviour
    {
        public void Move()
        {

        }

        public void AttackTarget(Entity target)
        {

        }


    }


    internal class Program
    {
        static void Main(string[] args)
        {
           
        }
    }
}