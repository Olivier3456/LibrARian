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
    [SerializeField] private TextMeshProUGUI authorTitleText;
    [SerializeField] private TextMeshProUGUI summaryText;
    [SerializeField] private TextMeshProUGUI averateNoteText;
    [SerializeField] private TextMeshProUGUI bestNoteText;
    [SerializeField] private TextMeshProUGUI typeText;



    private BooksData.BookData bookData;


    //private void Start()
    //{
    //    //DEBUG
    //    StartCoroutine(AppearCoroutine());
    //}


    
    public void ReceiveBookDatasAndDisplayUI(BooksData.BookData bookData)
    {
        this.bookData = bookData;

        authorTitleText.text = $"{bookData.author} - {bookData.title}";
        summaryText.text = bookData.summary;
        averateNoteText.text = bookData.averageNote.ToString();

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


    private IEnumerator DisappearCoroutine(bool destroyObject = true)
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

        if (destroyObject)
        {
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
    }
}
