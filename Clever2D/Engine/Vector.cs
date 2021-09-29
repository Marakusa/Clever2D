using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    public abstract class Vector
    {
        public float x;
        public float y;
        public float z;

        public abstract override string ToString();

        public static bool operator ==(Vector a, Vector b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector a, Vector b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }
    }
    public class Vector2 : Vector
    {
        public Vector2()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        public Vector2(float x)
        {
            this.x = x;
            this.y = 0;
            this.z = 0;
        }
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public override string ToString()
        {
            return $"({this.x}, {this.y})";
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector3 operator +(Vector2 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector3 operator -(Vector2 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vector3 operator -(Vector3 a, Vector2 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }
        public static Vector3 operator *(Vector2 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static Vector3 operator *(Vector3 a, Vector2 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }
        public static Vector3 operator /(Vector2 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        public static Vector3 operator /(Vector3 a, Vector2 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
    }
    public class Vector3 : Vector
    {
        public Vector3()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        public Vector3(float x)
        {
            this.x = x;
            this.y = 0;
            this.z = 0;
        }
        public Vector3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"({this.x}, {this.y}, {this.z})";
        }

        public static Vector3 operator +(Vector3 a, Vector2 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z);
        }
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector2 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static Vector3 operator *(Vector3 a, Vector2 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        public static Vector3 operator /(Vector3 a, Vector2 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
    }
}
