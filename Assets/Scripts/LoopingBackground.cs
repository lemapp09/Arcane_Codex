using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 0.1f; // Speed of scrolling
    [SerializeField]
    private GameObject background; 

    private Vector3 _backgroundStartPosition;

    void Start()
    {
        // Store the initial positions of the backgrounds
        _backgroundStartPosition = background.transform.position;
    }

    void Update()
    {
        // Move both backgrounds vertically
        var scrollAmount = scrollSpeed * Time.deltaTime;
        background.transform.position = new Vector3(_backgroundStartPosition.x,
            background.transform.position.y + scrollAmount, _backgroundStartPosition.z);

        // Check if Background1 has gone off-screen
        if (background.transform.position.y >= 12.97f) // assuming image height is 12.97
        {
            // Reposition Background1 behind Background2
            background.transform.position = new Vector3(_backgroundStartPosition.x,
                background.transform.position.y - 12.97f, _backgroundStartPosition.z);
        }
    }
}