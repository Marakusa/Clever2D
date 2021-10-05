using System;

namespace Clever2D.Engine
{
    /// <summary>
    /// The base class of all Vector types.
    /// </summary>
    public abstract class Vector
    {
        /// <summary>
        /// X component of the vector.
        /// </summary>
        public float x;
        /// <summary>
        /// Y component of the vector.
        /// </summary>
        public float y;
        /// <summary>
        /// Z component of the vector.
        /// </summary>
        public float z;

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
        /// Makes this vector have a magnitude of 1.
        /// </summary>
        public void Normalize()
        {
            float magnitude = this.Magnitude;
            x /= magnitude;
            y /= magnitude;
            if (magnitude != 0)
                z /= magnitude;
        }

        /// <summary>
        /// Gets the magnitude of this vector.
        /// </summary>
        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(Math.Pow(this.x, 2f) + Math.Pow(this.y, 2f) + Math.Pow(this.z, 2f));
            }
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector a, Vector b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }
    
        /// <summary>
        /// Converts any Vector value to Vector2.
        /// </summary>
        public Vector2 ToVector2()
        {
            return new Vector2(this.x, this.y);
        }
        /// <summary>
        /// Converts any Vector value to Vector3.
        /// </summary>
        public Vector3 ToVector3()
        {
            return new Vector3(this.x, this.y, this.z);
        }
    }

    /// <summary>
    /// Representation of 2D vectors and points.
    /// </summary>
    public class Vector2 : Vector
    {
        /// <summary>
        /// Representation of 2D vectors and points.
        /// </summary>
        public Vector2()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        /// <summary>
        /// Representation of 2D vectors and points.
        /// </summary>
        public Vector2(float x)
        {
            this.x = x;
            this.y = 0;
            this.z = 0;
        }
        /// <summary>
        /// Representation of 2D vectors and points.
        /// </summary>
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        /// <summary>
        /// Shorthand for writing Vector2(1, 0).
        /// </summary>
        public readonly static Vector2 right = new(1, 0);
        /// <summary>
        /// Shorthand for writing Vector2(-1, 0).
        /// </summary>
        public readonly static Vector2 left = new(-1, 0);
        /// <summary>
        /// Shorthand for writing Vector2(0, 1).
        /// </summary>
        public readonly static Vector2 up = new(0, 1);
        /// <summary>
        /// Shorthand for writing Vector2(0, -1).
        /// </summary>
        public readonly static Vector2 down = new(0, -1);
        /// <summary>
        /// Shorthand for writing Vector2(1, 1).
        /// </summary>
        public readonly static Vector2 one = new(1, 1);
        /// <summary>
        /// Shorthand for writing Vector2(0, 0).
        /// </summary>
        public readonly static Vector2 zero = new(0, 0);
        /// <summary>
        /// Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).
        /// </summary>
        public readonly static Vector2 positiveInfinity = new(float.PositiveInfinity, float.PositiveInfinity);
        /// <summary>
        /// Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).
        /// </summary>
        public readonly static Vector2 negativeInfinity = new(float.NegativeInfinity, float.NegativeInfinity);

        /// <summary>
        /// Set x and y components of an existing Vector2.
        /// </summary>
        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({this.x}, {this.y})";
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
        
        /// <summary>
        /// Gets this vector with a magnitude of 1.
        /// </summary>
        public Vector2 Normalized
        {
            get
            {
                float magnitude = this.Magnitude;
                return new Vector2(magnitude == 0 ? 0 : this.x / magnitude, magnitude == 0 ? 0 : this.y / magnitude);
            }
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
            return new Vector2(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y);
        }
        public static Vector3 operator /(Vector2 a, Vector3 b)
        {
            return new Vector3(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }
        public static Vector3 operator /(Vector3 a, Vector2 b)
        {
            return new Vector3(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }
        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2(b == 0 ? 0 : a.x / b, b == 0 ? 0 : a.y / b);
        }

        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator *(Vector2 a, int b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator *(Vector2 a, double b)
        {
            return new Vector2(a.x * (float)b, a.y * (float)b);
        }
        public static Vector2 operator *(Vector2 a, decimal b)
        {
            return new Vector2(a.x * (float)b, a.y * (float)b);
        }
    }

    /// <summary>
    /// Representation of 3D vectors and points.
    /// </summary>
    public class Vector3 : Vector
    {
        /// <summary>
        /// Representation of 3D vectors and points.
        /// </summary>
        public Vector3()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        /// <summary>
        /// Representation of 3D vectors and points.
        /// </summary>
        public Vector3(float x)
        {
            this.x = x;
            this.y = 0;
            this.z = 0;
        }
        /// <summary>
        /// Representation of 3D vectors and points.
        /// </summary>
        public Vector3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }
        /// <summary>
        /// Representation of 3D vectors and points.
        /// </summary>
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Shorthand for writing Vector3(1, 0, 0).
        /// </summary>
        public readonly static Vector3 right = new(1, 0, 0);
        /// <summary>
        /// Shorthand for writing Vector3(-1, 0, 0).
        /// </summary>
        public readonly static Vector3 left = new(-1, 0, 0);
        /// <summary>
        /// Shorthand for writing Vector3(0, 1, 0).
        /// </summary>
        public readonly static Vector3 up = new(0, 1, 0);
        /// <summary>
        /// Shorthand for writing Vector3(0, -1, 0).
        /// </summary>
        public readonly static Vector3 down = new(0, -1, 0);
        /// <summary>
        /// Shorthand for writing Vector3(0, 0, 1).
        /// </summary>
        public readonly static Vector3 forward = new(0, 0, 1);
        /// <summary>
        /// Shorthand for writing Vector3(0, 0, -1).
        /// </summary>
        public readonly static Vector3 back = new(0, 0, -1);
        /// <summary>
        /// Shorthand for writing Vector3(1, 1, 1).
        /// </summary>
        public readonly static Vector3 one = new(1, 1, 1);
        /// <summary>
        /// Shorthand for writing Vector3(0, 0, 0).
        /// </summary>
        public readonly static Vector3 zero = new(0, 0, 0);
        /// <summary>
        /// Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).
        /// </summary>
        public readonly static Vector3 positiveInfinity = new(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        /// <summary>
        /// Shorthand for writing Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity).
        /// </summary>
        public readonly static Vector3 negativeInfinity = new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        /// <summary>
        /// Set x and y components of an existing Vector3.
        /// </summary>
        public void Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"({this.x}, {this.y}, {this.z})";
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

        /// <summary>
        /// Gets this vector with a magnitude of 1.
        /// </summary>
        public Vector3 Normalized
        {
            get
            {
                float magnitude = this.Magnitude;
                return new Vector3(this.x / magnitude, this.y / magnitude, this.z / magnitude);
            }
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
            return new Vector3(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }
        public static Vector3 operator /(Vector3 a, Vector2 b)
        {
            return new Vector3(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }
        public static Vector3 operator /(Vector3 a, float b)
        {
            return new Vector3(b == 0 ? 0 : a.x / b, b == 0 ? 0 : a.y / b, b == 0 ? 0 : a.z / b);
        }

        public static Vector3 operator *(Vector3 a, float b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }
        public static Vector3 operator *(Vector3 a, int b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }
        public static Vector3 operator *(Vector3 a, double b)
        {
            return new Vector3(a.x * (float)b, a.y * (float)b, a.z * (float)b);
        }
        public static Vector3 operator *(Vector3 a, decimal b)
        {
            return new Vector3(a.x * (float)b, a.y * (float)b, a.z * (float)b);
        }
    }
}
