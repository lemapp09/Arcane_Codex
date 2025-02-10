using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class FadeOut : MonoBehaviour
{
    public Material blackoutMaterial; // Assign this in the Inspector
    public float fadeDuration = 7.5f;   // Duration of the fade (in seconds)
    private Color initialColor;

    private void Start()
    {
        // Get the initial color of the material
        initialColor = blackoutMaterial.color;
        
        // Start the fade coroutine when the game starts
        StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeFromBlack()
    {
        float startAlpha = 1f;
        initialColor.a = startAlpha;
        blackoutMaterial.color = initialColor;

        yield return new WaitForSeconds(3f);
        
        // Loop to reduce alpha from 1 to 0
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float lerpedAlpha = Mathf.Lerp(startAlpha, 0f, timeElapsed / fadeDuration);
            initialColor.a = lerpedAlpha;

            blackoutMaterial.color = initialColor;
            
            yield return new WaitForSeconds(0.01f);
        }

        // Ensure the final alpha is exactly 0
        initialColor.a = 0f;
        blackoutMaterial.color = initialColor;
    }
}