//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: April 2024
// Description: Sets up a save file.
//***************************************************************************************

using UnityEngine;

namespace Esper.USave
{
    public class SaveFileSetup : MonoBehaviour
    {
        public SaveFileSetupData saveFileData;

        /// <summary>
        /// The created save file.
        /// </summary>
        public SaveFile saveFile { get; private set; }

        private void Awake()
        {
            if (saveFile == null)
            {
                GetSaveFile();
            }
        }

        /// <summary>
        /// Generates random tokens for the AES key and IV.
        /// </summary>
        public void GenerateAESTokens()
        {
            saveFileData.GenerateAESTokens();
        }

        /// <summary>
        /// Gets the save file.
        /// </summary>
        /// <returns></returns>
        public SaveFile GetSaveFile()
        {
            if (saveFile == null)
            {
                saveFile = new SaveFile(saveFileData);
            }

            return saveFile;
        }
    }
}