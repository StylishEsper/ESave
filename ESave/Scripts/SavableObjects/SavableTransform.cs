//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: June 2024
// Description: Savable transform object.
//***************************************************************************************

using UnityEngine;

namespace Esper.ESave.SavableObjects
{
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
            position = transform.position.ToSavable();
            localPosition = transform.localPosition.ToSavable();
            rotation = transform.rotation.ToSavable();
            localRotation = transform.localRotation.ToSavable();
            localScale = transform.localScale.ToSavable();
        }
    }
}