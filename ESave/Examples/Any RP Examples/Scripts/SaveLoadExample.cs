//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: June 2024
// Description: Save/load example for sample game.
//***************************************************************************************

using System.Collections.Generic;
using UnityEngine;

namespace Esper.ESave.Example
{
    public class SaveLoadExample : MonoBehaviour
    {
        private const string objectPositionDataKey = "ObjectPosition";
        private const string objectColorDataKey = "ObjectColor";

        [SerializeField]
        private SpriteRenderer[] sprites;

        private SaveFileSetup saveFileSetup;
        private SaveFile saveFile;

        private List<Vector3> randomPositions = new();
        private List<Quaternion> randomRotations = new();
        private List<Vector3> randomScales = new();
        private List<Color> randomColors = new();

        private float timeElapsed;
        private bool isLoaded;

        private void Start()
        {
            // Get save file
            saveFileSetup = GetComponent<SaveFileSetup>();
            saveFile = saveFileSetup.GetSaveFile();

            // Load game when the save file operation is completed
            if (saveFile.isOperationOngoing)
            {
                saveFile.operation.onOperationEnded.AddListener(LoadGame);
            }
            else
            {
                LoadGame();
            }
        }

        private void Update()
        {
            if (!isLoaded)
            {
                return;
            }

            // Lerp each object to a state
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].transform.position = Vector3.Lerp(sprites[i].transform.position, randomPositions[i], timeElapsed);
                sprites[i].transform.rotation = Quaternion.Lerp(sprites[i].transform.rotation, randomRotations[i], timeElapsed);
                sprites[i].transform.localScale = Vector3.Lerp(sprites[i].transform.localScale, randomScales[i], timeElapsed);
                sprites[i].color = Color.Lerp(sprites[i].color, randomColors[i], timeElapsed);
            }

            if (timeElapsed >= 1)
            {
                GenerateRandomStates();
                timeElapsed = 0;
            }
            else
            {
                timeElapsed += Time.deltaTime * 0.4f;
            }
        }

        /// <summary>
        /// Generates random states for each sprite.
        /// </summary>
        public void GenerateRandomStates()
        {
            randomPositions.Clear();
            randomRotations.Clear();
            randomScales.Clear();
            randomColors.Clear();

            // Randomly get new state for each
            for (int i = 0; i < sprites.Length; i++)
            {
                randomPositions.Add(Random.insideUnitCircle * 8);
                randomRotations.Add(Random.rotation);
                randomScales.Add(Vector3.one * Random.Range(0.5f, 1.5f));
                randomColors.Add(Random.ColorHSV());
            }
        }

        /// <summary>
        /// Loads the game.
        /// </summary>
        public void LoadGame()
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                string positionKey = objectPositionDataKey + i;
                string colorKey = objectColorDataKey + i;

                if (saveFile.HasData(positionKey))
                {
                    // Get Vector3 from a special method because Vector3 is not savable data
                    var savableTransform = saveFile.GetTransform(positionKey);
                    sprites[i].transform.CopyTransformValues(savableTransform);
                }

                if (saveFile.HasData(colorKey))
                {
                    sprites[i].color = saveFile.GetColor(colorKey);
                }
            }

            // Populate lists with current state
            for (int i = 0; i < sprites.Length; i++)
            {
                var sprite = sprites[i];
                randomPositions.Add(sprite.transform.position);
                randomRotations.Add(sprite.transform.rotation);
                randomScales.Add(sprite.transform.localScale);
                randomColors.Add(sprite.color);
            }

            isLoaded = true;

            Debug.Log("Loaded game.");
        }

        /// <summary>
        /// Saves the game.
        /// </summary>
        public void SaveGame()
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                saveFile.AddOrUpdateData(objectPositionDataKey + i, sprites[i].transform);
                saveFile.AddOrUpdateData(objectColorDataKey + i, sprites[i].color);
            }

            saveFile.Save();

            Debug.Log("Saved game.");
        }
    }
}