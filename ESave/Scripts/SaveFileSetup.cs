//***************************************************************************************
// Writer: Stylish Esper
//***************************************************************************************

using UnityEngine;

namespace Esper.ESave
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
        /// <returns>The save file.</returns>
        public SaveFile GetSaveFile()
        {
            if (saveFile == null)
            {
                saveFile = new SaveFile(saveFileData, false);
            }

            return saveFile;
        }
    }
}