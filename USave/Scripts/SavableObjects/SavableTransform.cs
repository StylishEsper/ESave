//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: April 2024
// Description: Savable transform object. Only saves the position
//***************************************************************************************

using UnityEngine;

namespace Esper.USave.SavableObjects
{
    [System.Serializable]
    public class SavableTransform
    {
        public float[] position;
        public float[] localPosition;
        public float[] rotation;
        public float[] localRotation;
        public float[] localScale;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SavableTransform()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public SavableTransform(Transform transform) 
        {
            position = transform.position.ToFloat3();
            localPosition = transform.localPosition.ToFloat3();
            rotation = transform.rotation.ToFloat4();
            localRotation = transform.localRotation.ToFloat4();
            localScale = transform.localScale.ToFloat3();
        }
    }
}