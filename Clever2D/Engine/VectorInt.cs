using System;

namespace Clever2D.Engine
{
    /// <summary>
    /// The base class of all Vector types.
    /// </summary>
    public abstract class VectorInt
    {
        /// <summary>
        /// X component of the vector.
        /// </summary>
        public int x;
        /// <summary>
        /// Y component of the vector.
        /// </summary>
        public int y;
        /// <summary>
        /// Z component of the vector.
        /// </summary>
        public int z;

        /// <summary>
        /// Returns a formatted string for this vector.
        /// </summary>
        public abstract override string ToString();
        /// <summary>
        /// Returns true if the given vector is exactly equal to this vector.
        /// </summary>
        public abstract override bool Equals(object obj);
        /// <summary>
        /// Gets the hash code for the Vector value.
        /// </summary>
        public abstract override int GetHashCode();

        /// <summary>
        /// Compare Vectors.
        /// </summary>
        public static bool operator ==(VectorInt a, VectorInt b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        /// <summary>
        /// Compare Vectors (doesn't match).
        /// </summary>
        public static bool operator !=(VectorInt a, VectorInt b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }
    
        /// <summary>
        /// Converts any Vector value to Vector2Int.
        /// </summary>
        public Vector2Int ToVector2Int()
        {
            return new Vector2Int(this.x, this.y);
        }
        /// <summary>
        /// Converts any Vector value to Vector3Int.
        /// </summary>
        public Vector3Int ToVector3Int()
        {
            return new Vector3Int(this.x, this.y, this.z);
        }
    }

    /// <summary>
    /// Representation of 2D vectors and points.
    /// </summary>
    public class Vector2Int : VectorInt
    {
        /// <summary>
        /// Representation of 2D vectors and points.
        /// </summary>
        public Vector2Int()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        /// <summary>
        /// Representation of 2D vectors and points.
        /// </summary>
        public Vector2Int(int x)
        {
            this.x = x;
            this.y = 0;
            this.z = 0;
        }
        /// <summary>
        /// Representation of 2D vectors and points.
        /// </summary>
        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        /// <summary>
        /// Shorthand for writing Vector2Int(1, 0).
        /// </summary>
        public readonly static Vector2Int right = new(1, 0);
        /// <summary>
        /// Shorthand for writing Vector2Int(-1, 0).
        /// </summary>
        public readonly static Vector2Int left = new(-1, 0);
        /// <summary>
        /// Shorthand for writing Vector2Int(0, 1).
        /// </summary>
        public readonly static Vector2Int up = new(0, 1);
        /// <summary>
        /// Shorthand for writing Vector2Int(0, -1).
        /// </summary>
        public readonly static Vector2Int down = new(0, -1);
        /// <summary>
        /// Shorthand for writing Vector2Int(1, 1).
        /// </summary>
        public readonly static Vector2Int one = new(1, 1);
        /// <summary>
        /// Shorthand for writing Vector2Int(0, 0).
        /// </summary>
        public readonly static Vector2Int zero = new(0, 0);
        
        /// <summary>
        /// Set x and y components of an existing Vector2.
        /// </summary>
        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Convert Vector to string.
        /// </summary>
        public override string ToString()
        {
            return $"({this.x.ToString()}, {this.y.ToString()})";
        }
        /// <summary>
        /// Compare Vectors.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj != null &&
                this.x == (obj as Vector).x &&
                this.y == (obj as Vector).y &&
                this.z == (obj as Vector).z;
        }
        /// <summary>
        /// Returns the hash code of this Vector.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(
                this.x.ToString().GetHashCode() + 
                ",".GetHashCode() + 
                this.y.ToString().GetHashCode() + 
                ",".GetHashCode() + 
                this.z.ToString().GetHashCode()
            );
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }
        public static Vector3Int operator +(Vector2Int a, Vector3Int b)
        {
            return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, a.y - b.y);
        }
        public static Vector3Int operator -(Vector2Int a, Vector3Int b)
        {
            return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vector3Int operator -(Vector3Int a, Vector2Int b)
        {
            return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector2Int operator *(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x * b.x, a.y * b.y);
        }
        public static Vector3Int operator *(Vector2Int a, Vector3Int b)
        {
            return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static Vector3Int operator *(Vector3Int a, Vector2Int b)
        {
            return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector2Int operator /(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y);
        }
        public static Vector3Int operator /(Vector2Int a, Vector3Int b)
        {
            return new Vector3Int(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }
        public static Vector3Int operator /(Vector3Int a, Vector2Int b)
        {
            return new Vector3Int(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }

        public static Vector2Int operator *(Vector2Int a, int b)
        {
            return new Vector2Int(a.x * b, a.y * b);
        }
    }

    /// <summary>
    /// Representation of 3D vectors and points.
    /// </summary>
    public class Vector3Int : VectorInt
    {
        /// <summary>
        /// Representation of 3D vectors and points.
        /// </summary>
        public Vector3Int()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        /// <summary>
        /// Representation of 3D vectors and points.
        /// </summary>
        public Vector3Int(int x)
        {
            this.x = x;
            this.y = 0;
            this.z = 0;
        }
        /// <summary>
        /// Representation of 3D vectors and points.
        /// </summary>
        public Vector3Int(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }
        /// <summary>
        /// Representation of 3D vectors and points.
        /// </summary>
        public Vector3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Shorthand for writing Vector3Int(1, 0, 0).
        /// </summary>
        public readonly static Vector3Int right = new(1, 0, 0);
        /// <summary>
        /// Shorthand for writing Vector3Int(-1, 0, 0).
        /// </summary>
        public readonly static Vector3Int left = new(-1, 0, 0);
        /// <summary>
        /// Shorthand for writing Vector3Int(0, 1, 0).
        /// </summary>
        public readonly static Vector3Int up = new(0, 1, 0);
        /// <summary>
        /// Shorthand for writing Vector3Int(0, -1, 0).
        /// </summary>
        public readonly static Vector3Int down = new(0, -1, 0);
        /// <summary>
        /// Shorthand for writing Vector3Int(0, 0, 1).
        /// </summary>
        public readonly static Vector3Int forward = new(0, 0, 1);
        /// <summary>
        /// Shorthand for writing Vector3Int(0, 0, -1).
        /// </summary>
        public readonly static Vector3Int back = new(0, 0, -1);
        /// <summary>
        /// Shorthand for writing Vector3Int(1, 1, 1).
        /// </summary>
        public readonly static Vector3Int one = new(1, 1, 1);
        /// <summary>
        /// Shorthand for writing Vector3Int(0, 0, 0).
        /// </summary>
        public readonly static Vector3Int zero = new(0, 0, 0);
        
        /// <summary>
        /// Set x and y components of an existing Vector3Int.
        /// </summary>
        public void Set(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"({this.x.ToString()}, {this.y.ToString()}, {this.z.ToString()})";
        }
        public override bool Equals(object obj)
        {
            return obj != null &&
                this.x == (obj as Vector).x &&
                this.y == (obj as Vector).y &&
                this.z == (obj as Vector).z;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(
                this.x.ToString().GetHashCode() + 
                ",".GetHashCode() + 
                this.y.ToString().GetHashCode() + 
                ",".GetHashCode() + 
                this.z.ToString().GetHashCode()
            );
        }

        public static Vector3Int operator +(Vector3Int a, Vector2Int b)
        {
            return new Vector3Int(a.x + b.x, a.y + b.y, a.z);
        }
        public static Vector3Int operator +(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3Int operator -(Vector3Int a, Vector2Int b)
        {
            return new Vector3Int(a.x - b.x, a.y - b.y, a.z);
        }
        public static Vector3Int operator -(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3Int operator *(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static Vector3Int operator *(Vector3Int a, Vector2Int b)
        {
            return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3Int operator /(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }
        public static Vector3Int operator /(Vector3Int a, Vector2Int b)
        {
            return new Vector3Int(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }

        public static Vector3Int operator *(Vector3Int a, int b)
        {
            return new Vector3Int(a.x * b, a.y * b, a.z * b);
        }
    }
}
