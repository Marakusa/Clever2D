namespace Clever2D.Engine
{
    public class Transform
    {
        public Vector2 position;
        public Vector2 rotation;
        public Vector2 scale;

        public Transform()
        {
            position = Vector2.zero;
            rotation = Vector2.zero;
            scale = Vector2.one;
        }
        public Transform(Vector2 position)
        {
            this.position = position;
            rotation = Vector2.zero;
            scale = Vector2.one;
        }
        public Transform(Vector2 position, Vector2 rotation)
        {
            this.position = position;
            this.rotation = rotation;
            scale = Vector2.one;
        }
        public Transform(Vector2 position, Vector2 rotation, Vector2 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }
}
