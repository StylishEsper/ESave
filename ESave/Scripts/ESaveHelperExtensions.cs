//***************************************************************************************
// Writer: Stylish Esper
//***************************************************************************************

using Esper.ESave.SavableObjects;
using System;
using UnityEngine;

namespace Esper.ESave
{
    /// <summary>
    /// ESave helper extensions.
    /// </summary>
    public static class ESaveHelperExtensions
    {
        /// <summary>
        /// Converts a Vector2 to a float array.
        /// </summary>
        /// <param name="v2">This Vector2.</param>
        /// <returns>Float array that represents the vector.</returns>
        public static float[] ToFloat2(this Vector2 v2)
        {
            return new float[] { v2.x, v2.y };
        }

        /// <summary>
        /// Converts a Vector3 to a float array.
        /// </summary>
        /// <param name="v3">This Vector3.</param>
        /// <returns>Float array that represents the vector.</returns>
        public static float[] ToFloat2(this Vector3 v3)
        {
            return new float[] { v3.x, v3.y };
        }

        /// <summary>
        /// Converts a float array to a Vector2.
        /// </summary>
        /// <param name="f2">This float array.</param>
        /// <returns>Vector2 that represents the float array.</returns>
        public static Vector2 ToVector2(this float[] f2)
        {
            return new Vector2(f2[0], f2[1]);
        }

        /// <summary>
        /// Converts a Vector4 to a float array.
        /// </summary>
        /// <param name="v4">This Vector4.</param>
        /// <returns>Float array that represents the vector.</returns>
        public static float[] ToFloat4(this Vector4 v4)
        {
            return new float[] { v4.x, v4.y, v4.z, v4.w };
        }

        /// <summary>
        /// Converts a Vector3 to a float array.
        /// </summary>
        /// <param name="v3">This Vector3.</param>
        /// <returns>Float array that represents the vector.</returns>
        public static float[] ToFloat4(this Vector3 v3)
        {
            return new float[] { v3.x, v3.y, v3.z, 0 };
        }

        /// <summary>
        /// Converts a Vector2 to a float array.
        /// </summary>
        /// <param name="v2">This Vector2.</param>
        /// <returns>Float array that represents the vector.</returns>
        public static float[] ToFloat4(this Vector2 v2)
        {
            return new float[] { v2.x, v2.y, 0, 0 };
        }

        /// <summary>
        /// Converts a Vector3 to a float array.
        /// </summary>
        /// <param name="v3">This Vector3.</param>
        /// <returns>Float array that represents the vector.</returns>
        public static float[] ToFloat3(this Vector3 v3)
        {
            return new float[] { v3.x, v3.y, v3.z };
        }

        /// <summary>
        /// Converts a Vector2 to a float array.
        /// </summary>
        /// <param name="v2">This Vector2.</param>
        /// <returns>Float array that represents the vector.</returns>
        public static float[] ToFloat3(this Vector2 v2)
        {
            return new float[] { v2.x, v2.y, 0 };
        }

        /// <summary>
        /// Converts a float array to a Vector4.
        /// </summary>
        /// <param name="f4">This float array.</param>
        /// <returns>Vector4 that represents the float array.</returns>
        public static Vector4 ToVector4(this float[] f4)
        {
            return new Vector4(f4[0], f4[1], f4[2], f4[3]);
        }

        /// <summary>
        /// Converts a float array to a Vector3.
        /// </summary>
        /// <param name="f3">This float array.</param>
        /// <returns>Vector3 that represents the float array.</returns>
        public static Vector3 ToVector3(this float[] f3)
        {
            return new Vector3(f3[0], f3[1], f3[2]);
        }

        /// <summary>
        /// Converts a Quaternion to float array.
        /// </summary>
        /// <param name="q">This quaternion.</param>
        /// <returns>Float array that represents the quaternion.</returns>
        public static float[] ToFloat4(this Quaternion q)
        {
            return new float[] { q.x, q.y, q.z, q.w };
        }

        /// <summary>
        /// Converts a float array to a Quaternion.
        /// </summary>
        /// <param name="f4">This float array.</param>
        /// <returns>Quaternion that represents the float array.</returns>
        public static Quaternion ToQuaternion(this float[] f4)
        {
            return new Quaternion(f4[0], f4[1], f4[2], f4[3]);
        }

        /// <summary>
        /// Converts a color to a float array.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>Float array that represents the color.</returns>
        public static float[] ToFloat4(this Color color)
        {
            return new float[] { color.r, color.g, color.b, color.a };
        }

        /// <summary>
        /// Converts a color to a float array.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>Float array that represents the color.</returns>
        public static float[] ToFloat3(this Color color)
        {
            return new float[] { color.r, color.g, color.b };
        }

        /// <summary>
        /// Converts a float array to a color.
        /// </summary>
        /// <param name="fN">The float array.</param>
        /// <returns>Color that represents the float array.</returns>
        public static Color ToColor(this float[] fN)
        {
            if (fN.Length == 3)
            {
                return new Color(fN[0], fN[1], fN[2]);
            }
            else if (fN.Length == 4)
            {
                return new Color(fN[0], fN[1], fN[2], fN[3]);
            }

            Debug.LogWarning("Float array is not a valid color.");
            return new Color();
        }

        /// <summary>
        /// Converts a Vector2 to a SavableVector.
        /// </summary>
        /// <param name="v2">The Vector2.</param>
        /// <returns>The savable vector.</returns>
        public static SavableVector ToSavable(this Vector2 v2)
        {
            return new SavableVector(v2);
        }

        /// <summary>
        /// Converts a Vector3 to a SavableVector.
        /// </summary>
        /// <param name="v3">The Vector3.</param>
        /// <returns>The savable vector.</returns>
        public static SavableVector ToSavable(this Vector3 v3)
        {
            return new SavableVector(v3);
        }

        /// <summary>
        /// Converts a Vector4 to a SavableVector.
        /// </summary>
        /// <param name="v4">The Vector4.</param>
        /// <returns>The savable vector.</returns>
        public static SavableVector ToSavable(this Vector4 v4)
        {
            return new SavableVector(v4);
        }

        /// <summary>
        /// Converts a Quaternion to a SavableVector.
        /// 
        /// NOTE: Quaternions are similar to Vector4 (they have x, y, z, and w values).
        /// </summary>
        /// <param name="q">The Quaternion.</param>
        /// <returns>The savable vector.</returns>
        public static SavableVector ToSavable(this Quaternion q)
        {
            return new SavableVector(q);
        }

        /// <summary>
        /// Converts a Color to a SavableVector.
        /// 
        /// NOTE: Colors are similar to Vector4 (x = r, y = g, z = b, w = a).
        /// </summary>
        /// <param name="c">The Color.</param>
        /// <returns>The savable vector.</returns>
        public static SavableVector ToSavable(this Color c)
        {
            return new SavableVector(c);
        }

        /// <summary>
        /// Converts a Transform to a SavableTransform.
        /// </summary>
        /// <param name="transform">The Transform.</param>
        /// <returns>The savable transform.</returns>
        public static SavableTransform ToSavable(this Transform transform)
        {
            return new SavableTransform(transform);
        }

        /// <summary>
        /// Copies the position, rotation, and scale of another transform.
        /// </summary>
        /// <param name="transform">This transform.</param>
        /// <param name="other">The transform to copy.</param>
        public static void CopyTransformValues(this Transform transform, Transform other)
        {
            transform.position = other.position;
            transform.localPosition = other.localPosition;
            transform.rotation = other.rotation;
            transform.localRotation = other.localRotation;
            transform.localScale = other.localScale;
        }

        /// <summary>
        /// Copies the position, rotation, and scale of a savable transform.
        /// </summary>
        /// <param name="transform">This transform.</param>
        /// <param name="other">The savable transform to copy.</param>
        public static void CopyTransformValues(this Transform transform, SavableTransform other)
        {
            transform.position = other.position.vector3Value;
            transform.localPosition = other.localPosition.vector3Value;
            transform.rotation = other.rotation.quaternionValue;
            transform.localRotation = other.localRotation.quaternionValue;
            transform.localScale = other.localScale.vector3Value;
        }
    }
}