//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: June 2024
// Description: For ease of saving vectors and quaternions.
//***************************************************************************************

using Newtonsoft.Json;
using UnityEngine;

namespace Esper.ESave.SavableObjects
{
    [System.Serializable]
    public class SavableVector
    {
        public float x; 
        public float y;
        public float z;
        public float w;

        /// <summary>
        /// Returns the savable vector as a Vector2.
        /// </summary>
        [JsonIgnore]
        public Vector2 vector2Value
        {
            get
            {
                return new Vector2(x, y);
            }
        }

        /// <summary>
        /// Returns the savable vector as a Vector3.
        /// </summary>
        [JsonIgnore]
        public Vector3 vector3Value
        {
            get
            {
                return new Vector3(x, y, z);
            }
        }

        /// <summary>
        /// Returns the savable vector as a Vector4.
        /// </summary>
        [JsonIgnore]
        public Vector3 vector4Value
        {
            get
            {
                return new Vector4(x, y, z, w);
            }
        }

        /// <summary>
        /// Returns the savable vector as a Quaternion.
        /// </summary>
        [JsonIgnore]
        public Quaternion quaternionValue
        {
            get
            {
                return new Quaternion(x, y, z, w);
            }
        }

        /// <summary>
        /// Returns the savable vector as a Color.
        /// </summary>
        [JsonIgnore]
        public Color colorValue
        {
            get
            {
                return new Color(x, y, z, w);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SavableVector()
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="z">Z value.</param>
        /// <param name="w">W value.</param>
        public SavableVector (float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="v2">The vector 2.</param>
        public SavableVector(Vector2 v2) 
        {
            x = v2.x; 
            y = v2.y;
            z = 0;
            w = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="v3">The vector 3.</param>
        public SavableVector(Vector3 v3)
        {
            x = v3.x;
            y = v3.y;
            z = v3.z;
            w = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="v4">The vector 4.</param>
        public SavableVector(Vector4 v4)
        {
            x = v4.x;
            y = v4.y;
            z = v4.z;
            w = v4.w;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="q">The quaternion.</param>
        public SavableVector(Quaternion q)
        {
            x = q.x;
            y = q.y;
            z = q.z;
            w = q.w;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="color">The color.</param>
        public SavableVector(Color color)
        {
            x = color.r;
            y = color.g;
            z = color.b;
            w = color.a;
        }
    }
}