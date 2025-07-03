//***************************************************************************************
// Writer: Stylish Esper
//***************************************************************************************

using UnityEngine;

namespace Esper.ESave.SavableObjects
{
    /// <summary>
    /// A savable version of a Trasnform.
    /// </summary>
    [System.Serializable]
    public class SavableTransform : SavableObject
    {
        public SavableVector position;
        public SavableVector localPosition;
        public SavableVector rotation;
        public SavableVector localRotation;
        public SavableVector localScale;

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
            position = transform.position;
            localPosition = transform.localPosition;
            rotation = transform.rotation;
            localRotation = transform.localRotation;
            localScale = transform.localScale;
        }

        public static implicit operator SavableTransform(Transform t)
        {
            return new SavableTransform(t);
        }
    }
}