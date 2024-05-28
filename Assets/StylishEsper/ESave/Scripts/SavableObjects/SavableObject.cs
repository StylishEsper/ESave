//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: March 2024
// Description: Base savable object class.
//***************************************************************************************

using System;

namespace Esper.ESave.SavableObjects
{
    [Serializable]
    public class SavableObject
    {
        public string id;

        /// <summary>
        /// Gets the saved value.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The saved value of type T.</returns>
        public T GetValue<T>()
        {
            return ((SavableData<T>)this).value;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="value">The value.</param>
        public void SetValue<T>(T value)
        {
            ((SavableData<T>)this).value = value;
        }
    }
}