using System.Collections;
using UnityEngine;

public class TitleCard : MonoBehaviour
{
    [SerializeField]
    private GameObject _titleCard;
    [SerializeField]
    [Tooltip("Length of time to scale title card")]
    float lengthOfScale = 5.0f;
    [SerializeField]
    [Tooltip("Length of time between scale and slide")]
    float lengthOfDelay = 2.0f;
    [SerializeField]
    [Tooltip("Speed of slide")]
    float slideSpeed = 7.5f;
    private Vector3 _titleCardPosition; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // _titleCard = this.GetComponent<GameObject>();
        _titleCardPosition = _titleCard.transform.position;
        StartCoroutine(ScaleTitleCard());
        StartCoroutine(TitleCardSlide());
    }

    private IEnumerator ScaleTitleCard() // and rotate
    {
        Vector3 initialScale = _titleCard.transform.localScale;
        
        float targetScale = 1.75f;
        float duration = Time.time + lengthOfScale;
        _titleCard.transform.localScale = initialScale * targetScale;
        Quaternion initialRotation = _titleCard.transform.rotation;
        
        // rotate title card to 30 degrees on the x-axis. Must stay 90 degrees on the y-axis,
        // and -90 degrees on the z-axis.
        _titleCard.transform.rotation = Quaternion.Euler(30f, 90f, -90f);
        

        while (Time.time < duration)
        {
            _titleCard.transform.localScale = initialScale * targetScale;
            targetScale = (((duration - Time.time) / lengthOfScale) * 0.75f) + 1f;
            float tempXRot = ((1 - ((duration - Time.time) / lengthOfScale)) * 60.0f) + 30f;
            _titleCard.transform.rotation = Quaternion.Euler(tempXRot, 90f, -90f);
            yield return new WaitForSeconds(0.01f);
        }
        _titleCard.transform.localScale = initialScale;
        _titleCard.transform.rotation = initialRotation;
    }

    // coroutine that delays for two seconds, then slides the title card up and fades it out
    IEnumerator TitleCardSlide()
    {
        yield return new WaitForSeconds(lengthOfScale + lengthOfDelay);
        
        while (_titleCardPosition.y < 13.5f)
        {
            _titleCardPosition.y += slideSpeed / 100f;
            _titleCard.transform.position = _titleCardPosition;
            yield return new WaitForSeconds(0.01f);
        }

        // trigger event to start game
        GameManager.Instance.StartGame();
        
        this.gameObject.SetActive(false);
    }
}
