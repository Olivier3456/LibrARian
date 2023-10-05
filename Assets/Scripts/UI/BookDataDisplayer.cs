using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookDataDisplayer : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [Space(10)]
    [SerializeField] private float fullScale = 0.01f;
    [SerializeField] private float scalingDuration = 0.5f;
    [SerializeField] private AnimationCurve curve;
    [Space(10)]
    [SerializeField] private RatingStarsFiller averateNotesRatingStarsFiller;
    [SerializeField] private RatingStarsFiller bestNoteRatingStarsFiller;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI authorTitleText;
    [SerializeField] private TextMeshProUGUI summaryText;
    //[SerializeField] private TextMeshProUGUI averateNoteText;
    //[SerializeField] private TextMeshProUGUI bestNoteText;
    [SerializeField] private TextMeshProUGUI typeText;


    // Not currently in use.
    private BooksData.BookData bookData;

        
    public void ReceiveBookDatasAndDisplayUI(BooksData.BookData bookData)
    {
        this.bookData = bookData;

        authorTitleText.text = $"{bookData.author} - {bookData.title}";
        summaryText.text = bookData.summary;
        averateNotesRatingStarsFiller.SetStarFilling(bookData.averageNote);
        bestNoteRatingStarsFiller.SetStarFilling(bookData.bestNote);

        Debug.Log("[BookDataDisplayer] Book datas updated");

        StartCoroutine(AppearCoroutine());
    }

  
    public void DisplayUI()
    {
        StartCoroutine(AppearCoroutine());
    }


    public void HideUI()
    {
        StartCoroutine(DisappearCoroutine());
    }


    private IEnumerator DisappearCoroutine(bool destroyObject = false)
    {
        float scaleState = 0;
        Vector2 fullScaleVector = new Vector2(fullScale, fullScale);

        while (scaleState < 1)
        {
            scaleState += Time.deltaTime / scalingDuration;
            canvasRectTransform.localScale = Vector2.Lerp(Vector2.zero, fullScaleVector, curve.Evaluate(1 - scaleState));
            canvasGroup.alpha = curve.Evaluate(1 - scaleState);
            yield return null;
        }

        canvasRectTransform.localScale = Vector2.zero;
        canvasGroup.alpha = 0;

        Debug.Log("[BookDataDisplayer] UI not visible anymore.");

        if (destroyObject)
        {
            Debug.Log("[BookDataDisplayer] this UI game Object is destroyed.");
            Destroy(gameObject);
        }
    }


    private IEnumerator AppearCoroutine()
    {
        float scaleState = 0;
        Vector2 fullScaleVector = new Vector2(fullScale, fullScale);

        while (scaleState < 1)
        {
            scaleState += Time.deltaTime / scalingDuration;
            canvasRectTransform.localScale = Vector2.Lerp(Vector2.zero, fullScaleVector, curve.Evaluate(scaleState));
            canvasGroup.alpha = curve.Evaluate(scaleState);
            yield return null;
        }

        canvasRectTransform.localScale = fullScaleVector;
        canvasGroup.alpha = 1;

        Debug.Log("[BookDataDisplayer] UI displayed.");
    }
}