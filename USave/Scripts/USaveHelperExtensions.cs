//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: April 2024
// Description: USave helper extensions.
//***************************************************************************************

using Esper.USave.SavableObjects;
using UnityEngine;

namespace Esper.USave
{
    public static class USaveHelperExtensions
    {
        /// <summary>
        /// Converts a Vector2 to a float array.
        /// </summary>
        /// <param name="v2">This Vector2.</param>
        /// <returns>Vector2 converted to a float array.</returns>
        public static float[] ToFloat2(this Vector2 v2)
        {
            return new float[] { v2.x, v2.y };
        }

        /// <summary>
        /// Converts a Vector3 to a float array.
        /// </summary>
        /// <param name="v3">This Vector3.</param>
        /// <returns>Vector3 converted to a float array.</returns>
        public static float[] ToFloat2(this Vector3 v3)
        {
            return new float[] { v3.x, v3.y };
        }

        /// <summary>
        /// Converts a float array to a Vector2.
        /// </summary>
        /// <param name="f2">This float array.</param>
        /// <returns>Float array converted to Vector2.</returns>
        public static Vector2 ToVector2(this float[] f2)
        {
            return new Vector2(f2[0], f2[1]);
        }

        /// <summary>
        /// Converts a Vector3 to a float array.
        /// </summary>
        /// <param name="v3">This Vector3.</param>
        /// <returns>Vector3 converted to a float array.</returns>
        public static float[] ToFloat3(this Vector3 v3)
        {
            return new float[] { v3.x, v3.y, v3.z };
        }

        /// <summary>
        /// Converts a Vector2 to a float array.
        /// </summary>
        /// <param name="v2">This Vector2.</param>
        /// <returns>Vector2 converted to a float array.</returns>
        public static float[] ToFloat3(this Vector2 v2)
        {
            return new float[] { v2.x, v2.y, 0 };
        }

        /// <summary>
        /// Converts a float array to a Vector3.
        /// </summary>
        /// <param name="f3">This float array.</param>
        /// <returns>Float array converted to Vector3.</returns>
        public static Vector3 ToVector3(this float[] f3)
        {
            return new Vector3(f3[0], f3[1], f3[2]);
        }

        /// <summary>
        /// Converts a Quaternion to float array.
        /// </summary>
        /// <param name="q">This quaternion.</param>
        /// <returns>Quaternion converted to a float array.</returns>
        public static float[] ToFloat4(this Quaternion q)
        {
            return new float[] { q.x, q.y, q.z, q.w };
        }

        /// <summary>
        /// Converts a float array to a Quaternion.
        /// </summary>
        /// <param name="f4">This float array.</param>
        /// <returns>Float array converted to Quaternion.</returns>
        public static Quaternion ToQuaternion(this float[] f4)
        {
            return new Quaternion(f4[0], f4[1], f4[2], f4[3]);
        }

        /// <summary>
        /// Copies the position, rotation, and scale of another transform.
        /// </summary>
        /// <param name="transform">This transform.</param>
        /// <param name="other">The transform to copy.</param>
        public static void CopyTransformValues(this Transform transform, Transform other)
        {
            transform.position = other.position;
            transform.rotation = other.rotation;
            transform.localScale = other.localScale;
        }

        /// <summary>
        /// Copies the position, rotation, and scale of another transform.
        /// </summary>
        /// <param name="transform">This transform.</param>
        /// <param name="other">The transform to copy.</param>
        public static void CopyTransformValues(this Transform transform, SavableTransform other)
        {
            transform.position = other.position.ToVector3();
            transform.rotation = other.rotation.ToQuaternion();
            transform.localScale = other.localScale.ToVector3();
        }
    }
}