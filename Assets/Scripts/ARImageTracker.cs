using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
//using UnityEditor.XR.ARKit;
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
    [SerializeField]
    Vector3 scaleFactor;

    private GameObject m_ARInfoGO;
    private BookDataDisplayer m_BookDataDisplayer;

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    // Start is called before the first frame update
    void Start()
    {
        m_ARInfoGO = Instantiate(m_ARInfoPrefab);
        m_BookDataDisplayer = m_ARInfoGO.GetComponent<BookDataDisplayer>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //m_TrackedImageManager.referenceLibrary = m_ReferenceImageLibrary;
        foreach (var book in m_booksData.books)
        {
            text.text += "\n" + book.title;
        }
    }

    private void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            m_ARInfoPrefab.SetActive(false);
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        // Display the name of the tracked image in the canvas
        text.text = trackedImage.referenceImage.name;

        // Assign and Place Game Object
        AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position);

        Debug.Log($"trackedImage.referenceImage.name: {trackedImage.referenceImage.name}");
    }

    void AssignGameObject(string name, Vector3 newPosition)
    {
        if (m_ARInfoGO != null)
        {
            m_ARInfoGO.SetActive(true);
            m_ARInfoGO.transform.position = newPosition;
            m_ARInfoGO.transform.localScale = scaleFactor;
            foreach (var book in m_booksData.books)
            {
                if (book.title == name)
                {
                    m_ARInfoGO.SetActive(true);
                    m_BookDataDisplayer.ReceiveBookDatasAndDisplayUI(book);
                    m_BookDataDisplayer.DisplayUI();
                }
            }
        }
    }
}