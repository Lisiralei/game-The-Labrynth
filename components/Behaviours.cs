namespace OOPgameLbrynth.components
{
    interface MovementBehaviour
    {
        public void Move(Position targetPosition);
    }

    interface AttackBehaviour
    {
        public void AttackTarget(Entity target);
    }
}