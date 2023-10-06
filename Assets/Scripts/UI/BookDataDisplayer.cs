using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookDataDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] public RectTransform canvasRectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [Space(10)]
    [SerializeField] private float fullScale = 0.00035f;
    [SerializeField] private float scalingDuration = 0.5f;
    [SerializeField] private AnimationCurve curve;
    [Space(10)]
    [SerializeField] private RatingStarsFiller averateNotesRatingStarsFiller;
    [SerializeField] private RatingStarsFiller bestNoteRatingStarsFiller;
    [Space(10)]
    //[SerializeField] private TextMeshProUGUI authorTitleText;
    //[SerializeField] private TextMeshProUGUI typeText;
    //[SerializeField] private TextMeshProUGUI summaryText;
    //[SerializeField] private TextMeshProUGUI averateNoteText;
    //[SerializeField] private TextMeshProUGUI bestNoteText;    

    [SerializeField] private Text authorTitleText_legacy;
    [SerializeField] private Text summaryText_legacy;
    [SerializeField] private Text typeText_legacy;   
    [SerializeField] private Text reviews_legacy;   
    
    public bool IsActive { get; private set; }


    //[SerializeField]
    //BooksData m_booksData;
    //private void Start()
    //{
    //    ReceiveBookDatasAndDisplayUI(m_booksData.books[0]);
    //}



    public BooksData.BookData BookData { get; set; }

        
    public void ReceiveBookDatasAndDisplayUI(BooksData.BookData bookData)
    {
        this.BookData = bookData;

        reviews_legacy.text = "";
        authorTitleText_legacy.text = $"{bookData.author} - {bookData.title}";
        typeText_legacy.text = bookData.type;
        summaryText_legacy.text = bookData.summary;

        for(int i = 0;i < bookData.reviews.Length; i++)
        {
            string review = $"Author: {bookData.reviews[i].author}\nNote:{bookData.reviews[i].note}\nReview:{bookData.reviews[i].review}\n\n";
            reviews_legacy.text += review;
        }


        //authorTitleText.text = $"{bookData.author} - {bookData.title}";
        //typeText.text = bookData.type;
        //summaryText.text = bookData.summary;
        averateNotesRatingStarsFiller.SetStarFilling(bookData.averageNote);
        bestNoteRatingStarsFiller.SetStarFilling(bookData.bestNote);

        Debug.Log("[BookDataDisplayer] Book datas updated");

        StartCoroutine(AppearCoroutine());
    }

  
    public void DisplayUI()
    {
        if(!IsActive)
            StartCoroutine(AppearCoroutine());
    }


    public void HideUI()
    {
        Debug.Log("[BookDataDisplayer] Hiding UI");

        if (IsActive)
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

        IsActive = false;

        Debug.Log("[BookDataDisplayer] UI not visible anymore.");

        if (destroyObject)
        {
            Debug.Log("[BookDataDisplayer] This UI game object is destroyed.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("[BookDataDisplayer] The UI canvas game object is deactivated.");
            canvas.SetActive(false);
        }
    }


    private IEnumerator AppearCoroutine()
    {
        IsActive = true;

        float scaleState = 0;
        Vector2 fullScaleVector = new Vector2(fullScale, fullScale);
        canvas.SetActive(true);

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
