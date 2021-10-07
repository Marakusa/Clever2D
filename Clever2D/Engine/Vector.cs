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
        protected float Magnitude => (float)Math.Sqrt(Math.Pow(this.x, 2f) + Math.Pow(this.y, 2f) + Math.Pow(this.z, 2f));

        /// <summary>
        /// Equals.
        /// </summary>
        public static bool operator ==(Vector a, Vector b)
        {
            try
            {
                return a.x == b.x && a.y == b.y && a.z == b.z;
            }
            catch (Exception e)
            {
                Player.LogError(e.Message, e);
                return false;
            }
        }
        /// <summary>
        /// Doesn't match.
        /// </summary>
        public static bool operator !=(Vector a, Vector b)
        {
            try
            {
                return a.x != b.x || a.y != b.y || a.z != b.z;
            }
            catch (Exception e)
            {
                Player.LogError(e.Message, e);
                return false;
            }
        }
    
        /// <summary>
        /// Converts any Vector value to Vector2.
        /// </summary>
        public Vector2 ToVector2()
        {
            return new(this.x, this.y);
        }
        /// <summary>
        /// Converts any Vector value to Vector3.
        /// </summary>
        public Vector3 ToVector3()
        {
            return new(this.x, this.y, this.z);
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
        public static Vector2 Right => new(1, 0);
        /// <summary>
        /// Shorthand for writing Vector2(-1, 0).
        /// </summary>
        public static Vector2 Left => new(-1, 0);
        /// <summary>
        /// Shorthand for writing Vector2(0, 1).
        /// </summary>
        public static Vector2 Up => new(0, 1);
        /// <summary>
        /// Shorthand for writing Vector2(0, -1).
        /// </summary>
        public static Vector2 Down => new(0, -1);
        /// <summary>
        /// Shorthand for writing Vector2(1, 1).
        /// </summary>
        public static Vector2 One => new(1, 1);
        /// <summary>
        /// Shorthand for writing Vector2(0, 0).
        /// </summary>
        public static Vector2 Zero => new(0, 0);
        /// <summary>
        /// Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).
        /// </summary>
        public static Vector2 PositiveInfinity => new(float.PositiveInfinity, float.PositiveInfinity);
        /// <summary>
        /// Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).
        /// </summary>
        public static Vector2 NegativeInfinity => new(float.NegativeInfinity, float.NegativeInfinity);

        /// <summary>
        /// Set x and y components of an existing Vector2.
        /// </summary>
        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Returns a formatted string for this vector.
        /// </summary>
        public override string ToString()
        {
            return $"({this.x}, {this.y})";
        }
        /// <summary>
        /// Returns true if the given vector is exactly equal to this vector.
        /// </summary>
        public override bool Equals(object obj)
        {
            try
            {
                return obj != null &&
                    this.x == (obj as Vector).x &&
                    this.y == (obj as Vector).y &&
                    this.z == (obj as Vector).z;
            }
            catch (Exception e)
            {
                Player.LogError(e.Message, e);
                return false;
            }
        }
        /// <summary>
        /// Gets the hash code for the Vector value.
        /// </summary>
        public override int GetHashCode()
        {
            try
            {
                return HashCode.Combine(
                    this.x.ToString().GetHashCode() +
                    ",".GetHashCode() +
                    this.y.ToString().GetHashCode() +
                    ",".GetHashCode() +
                    this.z.ToString().GetHashCode()
                );
            }
            catch (Exception e)
            {
                Player.LogError(e.Message, e);
                return -1;
            }
        }
        
        /// <summary>
        /// Gets this vector with a magnitude of 1.
        /// </summary>
        public Vector2 Normalized
        {
            get
            {
                var magnitude = this.Magnitude;
                return new Vector2(magnitude == 0 ? 0 : this.x / magnitude, magnitude == 0 ? 0 : this.y / magnitude);
            }
        }

        /// <summary>
        /// Add.
        /// </summary>
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new(a.x + b.x, a.y + b.y);
        }
        /// <summary>
        /// Add.
        /// </summary>
        public static Vector3 operator +(Vector2 a, Vector3 b)
        {
            return new(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        /// <summary>
        /// Subtract.
        /// </summary>
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new(a.x - b.x, a.y - b.y);
        }
        /// <summary>
        /// Subtract.
        /// </summary>
        public static Vector3 operator -(Vector2 a, Vector3 b)
        {
            return new(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        /// <summary>
        /// Subtract.
        /// </summary>
        public static Vector3 operator -(Vector3 a, Vector2 b)
        {
            return new(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new(a.x * b.x, a.y * b.y);
        }
        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector3 operator *(Vector2 a, Vector3 b)
        {
            return new(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector3 operator *(Vector3 a, Vector2 b)
        {
            return new(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new(a.x * b, a.y * b);
        }
        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector2 operator *(Vector2 a, int b)
        {
            return new(a.x * b, a.y * b);
        }
        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector2 operator *(Vector2 a, double b)
        {
            return new(a.x * (float)b, a.y * (float)b);
        }
        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector2 operator *(Vector2 a, decimal b)
        {
            return new(a.x * (float)b, a.y * (float)b);
        }

        /// <summary>
        /// Divide.
        /// </summary>
        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y);
        }
        /// <summary>
        /// Divide.
        /// </summary>
        public static Vector3 operator /(Vector2 a, Vector3 b)
        {
            return new(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }
        /// <summary>
        /// Divide.
        /// </summary>
        public static Vector3 operator /(Vector3 a, Vector2 b)
        {
            return new(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }
        /// <summary>
        /// Divide.
        /// </summary>
        public static Vector2 operator /(Vector2 a, float b)
        {
            return new(b == 0 ? 0 : a.x / b, b == 0 ? 0 : a.y / b);
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
        public static Vector3 Right => new(1, 0, 0);
        /// <summary>
        /// Shorthand for writing Vector3(-1, 0, 0).
        /// </summary>
        public static Vector3 Left => new(-1, 0, 0);
        /// <summary>
        /// Shorthand for writing Vector3(0, 1, 0).
        /// </summary>
        public static Vector3 Up => new(0, 1, 0);
        /// <summary>
        /// Shorthand for writing Vector3(0, -1, 0).
        /// </summary>
        public static Vector3 Down => new(0, -1, 0);
        /// <summary>
        /// Shorthand for writing Vector3(0, 0, 1).
        /// </summary>
        public static Vector3 Forward => new(0, 0, 1);
        /// <summary>
        /// Shorthand for writing Vector3(0, 0, -1).
        /// </summary>
        public static Vector3 Back => new(0, 0, -1);
        /// <summary>
        /// Shorthand for writing Vector3(1, 1, 1).
        /// </summary>
        public static Vector3 One => new(1, 1, 1);
        /// <summary>
        /// Shorthand for writing Vector3(0, 0, 0).
        /// </summary>
        public static Vector3 Zero => new(0, 0, 0);
        /// <summary>
        /// Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).
        /// </summary>
        public static Vector3 PositiveInfinity => new(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        /// <summary>
        /// Shorthand for writing Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity).
        /// </summary>
        public static Vector3 NegativeInfinity => new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        /// <summary>
        /// Set x and y components of an existing Vector3.
        /// </summary>
        public void Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Returns a formatted string for this vector.
        /// </summary>
        public override string ToString()
        {
            return $"({this.x}, {this.y}, {this.z})";
        }
        /// <summary>
        /// Returns true if the given vector is exactly equal to this vector.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj != null &&
                this.x == (obj as Vector).x &&
                this.y == (obj as Vector).y &&
                this.z == (obj as Vector).z;
        }
        /// <summary>
        /// Gets the hash code for the Vector value.
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

        /// <summary>
        /// Add.
        /// </summary>
        public static Vector3 operator +(Vector3 a, Vector2 b)
        {
            return new(a.x + b.x, a.y + b.y, a.z);
        }
        /// <summary>
        /// Add.
        /// </summary>
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        /// <summary>
        /// Subtract.
        /// </summary>
        public static Vector3 operator -(Vector3 a, Vector2 b)
        {
            return new(a.x - b.x, a.y - b.y, a.z);
        }
        /// <summary>
        /// Subtract.
        /// </summary>
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector3 operator *(Vector3 a, Vector2 b)
        {
            return new(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector3 operator *(Vector3 a, float b)
        {
            return new(a.x * b, a.y * b, a.z * b);
        }
        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector3 operator *(Vector3 a, int b)
        {
            return new(a.x * b, a.y * b, a.z * b);
        }
        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector3 operator *(Vector3 a, double b)
        {
            return new(a.x * (float)b, a.y * (float)b, a.z * (float)b);
        }
        /// <summary>
        /// Multiply.
        /// </summary>
        public static Vector3 operator *(Vector3 a, decimal b)
        {
            return new(a.x * (float)b, a.y * (float)b, a.z * (float)b);
        }

        /// <summary>
        /// Divide.
        /// </summary>
        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return new(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }
        /// <summary>
        /// Divide.
        /// </summary>
        public static Vector3 operator /(Vector3 a, Vector2 b)
        {
            return new(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y, b.z == 0 ? 0 : a.z / b.z);
        }
        /// <summary>
        /// Divide.
        /// </summary>
        public static Vector3 operator /(Vector3 a, float b)
        {
            return new(b == 0 ? 0 : a.x / b, b == 0 ? 0 : a.y / b, b == 0 ? 0 : a.z / b);
        }
    }
}
