using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageTracker : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;
    [SerializeField]
    XRReferenceImageLibrary m_ReferenceImageLibrary;
    [SerializeField]
    BooksData m_booksData;
    [SerializeField]
    GameObject m_ARInfoPrefab;
    [SerializeField]
    Text text; 



    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //m_TrackedImageManager.referenceLibrary = m_ReferenceImageLibrary;
        foreach (var book in m_booksData.books)
        {
            text.text += "\n" + book.title;
        }
    }

   private void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            // Handle added event
            text.text = newImage.referenceImage.name;
            foreach (var book in m_booksData.books)
            {
                //text.text += "\n" + book.title;
                if (newImage.referenceImage.name.Equals(book.title))
                {
                //    //GameObject ARBookInfo = Instantiate(m_ARInfoPrefab, newImage.transform);
                //    Debug.Log(book.title + " found");
                    text.text = "found" + book.title;
                }
            }

        }

        foreach (var updatedImage in eventArgs.updated)
        {
            if(updatedImage.trackingState == TrackingState.Limited)
            {
                Destroy(updatedImage.gameObject);

            }
        }

        foreach (var removedImage in eventArgs.removed)
        {
            Destroy(removedImage.gameObject);
        }
    }
}
