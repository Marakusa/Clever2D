using System;

namespace Clever2D.Engine
{
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
            float magnitude = this.magnitude;
            this.x = this.x / magnitude;
            this.y = this.y / magnitude;
            this.z = this.z / magnitude;
        }

        /// <summary>
        /// Gets the magnitude of this vector.
        /// </summary>
        public float magnitude
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
        /// <returns></returns>
        public Vector2 ToVector2()
        {
            return new Vector2(this.x, this.y);
        }
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
        public readonly static Vector2 right = new Vector2(1, 0);
        /// <summary>
        /// Shorthand for writing Vector2(-1, 0).
        /// </summary>
        public readonly static Vector2 left = new Vector2(-1, 0);
        /// <summary>
        /// Shorthand for writing Vector2(0, 1).
        /// </summary>
        public readonly static Vector2 up = new Vector2(0, 1);
        /// <summary>
        /// Shorthand for writing Vector2(0, -1).
        /// </summary>
        public readonly static Vector2 down = new Vector2(0, -1);
        /// <summary>
        /// Shorthand for writing Vector2(1, 1).
        /// </summary>
        public readonly static Vector2 one = new Vector2(1, 1);
        /// <summary>
        /// Shorthand for writing Vector2(0, 0).
        /// </summary>
        public readonly static Vector2 zero = new Vector2(0, 0);
        /// <summary>
        /// Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).
        /// </summary>
        public readonly static Vector2 positiveInfinity = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        /// <summary>
        /// Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).
        /// </summary>
        public readonly static Vector2 negativeInfinity = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

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
        public Vector2 normalized
        {
            get
            {
                float magnitude = this.magnitude;
                return new Vector2(this.x / magnitude, this.y / magnitude);
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
        public readonly static Vector3 right = new Vector3(1, 0, 0);
        /// <summary>
        /// Shorthand for writing Vector3(-1, 0, 0).
        /// </summary>
        public readonly static Vector3 left = new Vector3(-1, 0, 0);
        /// <summary>
        /// Shorthand for writing Vector3(0, 1, 0).
        /// </summary>
        public readonly static Vector3 up = new Vector3(0, 1, 0);
        /// <summary>
        /// Shorthand for writing Vector3(0, -1, 0).
        /// </summary>
        public readonly static Vector3 down = new Vector3(0, -1, 0);
        /// <summary>
        /// Shorthand for writing Vector3(0, 0, 1).
        /// </summary>
        public readonly static Vector3 forward = new Vector3(0, 0, 1);
        /// <summary>
        /// Shorthand for writing Vector3(0, 0, -1).
        /// </summary>
        public readonly static Vector3 back = new Vector3(0, 0, -1);
        /// <summary>
        /// Shorthand for writing Vector3(1, 1, 1).
        /// </summary>
        public readonly static Vector3 one = new Vector3(1, 1, 1);
        /// <summary>
        /// Shorthand for writing Vector3(0, 0, 0).
        /// </summary>
        public readonly static Vector3 zero = new Vector3(0, 0, 0);
        /// <summary>
        /// Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).
        /// </summary>
        public readonly static Vector3 positiveInfinity = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        /// <summary>
        /// Shorthand for writing Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity).
        /// </summary>
        public readonly static Vector3 negativeInfinity = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

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
        public Vector3 normalized
        {
            get
            {
                float magnitude = this.magnitude;
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
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        public static Vector3 operator /(Vector3 a, Vector2 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
    }
}
