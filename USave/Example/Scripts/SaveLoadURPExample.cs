//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: April 2024
// Description: Save/load example for sample game.
//***************************************************************************************

using TMPro;
using UnityEngine;

namespace Esper.USave.Example
{
    public class SaveLoadURPExample : MonoBehaviour
    {
        // Const data IDs 
        private const string playerPositionDataKey = "PlayerPosition";
        private const string distanceTraveledDataKey = "DistanceTraveled";

        [SerializeField]
        private CharacterController characterController;
        [SerializeField]
        private TextMeshProUGUI distanceText;

        private SaveFileSetup saveFileSetup;
        private SaveFile saveFile;

        private Vector3 prevPlayerPosition;

        private float distanceTraveled;

        private void Start()
        {
            // Get save file
            saveFileSetup = GetComponent<SaveFileSetup>();
            saveFile = saveFileSetup.GetSaveFile();

            // Load game
            LoadGame();
        }

        private void Update()
        {
            distanceTraveled += Vector3.Distance(prevPlayerPosition, characterController.transform.position);
            distanceText.text = $"Distance Traveled: {distanceTraveled}";
            prevPlayerPosition = characterController.transform.position;
        }

        /// <summary>
        /// Loads the game.
        /// </summary>
        public void LoadGame()
        {
            // Temporarily disable character controller so it doesn't override the position
            characterController.enabled = false;

            if (saveFile.HasData(distanceTraveledDataKey))
            {
                distanceTraveled = saveFile.GetData<float>(distanceTraveledDataKey);
            }

            if (saveFile.HasData(playerPositionDataKey))
            {
                // Get Vector3 from a special method because Vector3 is not savable data
                var savableTransform = saveFile.GetTransform(playerPositionDataKey);
                characterController.transform.CopyTransformValues(savableTransform);
            }

            prevPlayerPosition = characterController.transform.position;

            characterController.enabled = true;

            Debug.Log("Loaded game.");
        }

        /// <summary>
        /// Saves the game.
        /// </summary>
        public void SaveGame()
        {
            saveFile.AddOrUpdateData(distanceTraveledDataKey, distanceTraveled);
            saveFile.AddOrUpdateData(playerPositionDataKey, characterController.transform);
            saveFile.Save();

            Debug.Log("Saved game.");
        }
    }
}