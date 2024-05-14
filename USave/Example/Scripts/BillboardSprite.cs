//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: April 2024
// Description: Force sprite to face camera.
//***************************************************************************************

using UnityEngine;

namespace Esper.USave.Example
{
    [ExecuteInEditMode]
    public class BillboardSprite : MonoBehaviour
    {
        private void Start()
        {
            // Apply shadow casting on the main sprite
            var sprite = GetComponent<SpriteRenderer>();
            if (sprite) sprite.ShadowcastSprite();

            // Go through each child object and check for more sprites
            foreach (Transform t in transform)
            {
                // Apply shadow casting
                sprite = t.GetComponent<SpriteRenderer>();
                if (sprite) sprite.ShadowcastSprite();
            }
        }

        private void Update()
        {
            // Make this object always face the camera
            transform.forward = Camera.main.transform.forward;
        }
    }


    /// <summary>
    /// A quick SpriteRenderer extension class to help set the sprite.
    /// </summary>
    public static class SpriteExtension
    {
        public static void ShadowcastSprite(this SpriteRenderer sprite)
        {
            // Make this sprite a shadow caster/receiver
            sprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            sprite.receiveShadows = true;

            // Apply shadow cast material
            sprite.material = (Material)Resources.Load("Materials/SpriteShadow");
        }
    }
}