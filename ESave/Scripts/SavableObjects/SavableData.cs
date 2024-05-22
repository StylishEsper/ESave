//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: March 2024
// Description: Savable data object. Supports any type, but not all types can be saved.
//***************************************************************************************

namespace Esper.ESave.SavableObjects
{
    [System.Serializable]
    public class SavableData<T> : SavableObject
    {
        public T value;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="value">The value.</param>
        public SavableData(string id, T value)
        {
            this.id = id;
            this.value = value;
        }
    }
}