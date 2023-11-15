namespace OOPgameLbrynth.components
{
    struct Position
    {
        public int x;
        public int y;
        public Position(int x, int y) { this.x = x; this.y = y; }
        public Position(Position position) { x = position.x; y = position.y; }
        public static bool operator ==(Position a, Position b) { if (a.x == b.x && a.y == b.y) return true; return false; }
        public static bool operator !=(Position a, Position b) { return !(a == b); }

        public static Position operator +(Position pos, (int x, int y) offset)
        {
            return new Position(pos.x + offset.x, pos.y + offset.y);
        }

    }
}
