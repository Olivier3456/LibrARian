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
        //Debug.Log("[ARImageTracker] OnChanged");

        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            Debug.Log("[ARImageTracker] image added");
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            //Debug.Log("[ARImageTracker] image updated");
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            Debug.Log("[ARImageTracker] image removed");
            //m_ARInfoPrefab.SetActive(false);
            m_BookDataDisplayer.HideUI();
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        // Display the name of the tracked image in the canvas
        text.text = trackedImage.referenceImage.name;

        // Assign and Place Game Object
        AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position, trackedImage.transform.rotation);

        Debug.Log($"[ARImageTracker] trackedImage.referenceImage.name: {trackedImage.referenceImage.name}");
    }

    void AssignGameObject(string name, Vector3 newPosition, Quaternion newRotation)
    {
        if (m_ARInfoGO != null)
        {
            Debug.Log("[ARImageTracker] m_ARInfoGO != null");

            //m_ARInfoGO.SetActive(true);
            m_ARInfoGO.transform.position = newPosition;
            m_ARInfoGO.transform.rotation = newRotation;
            //m_ARInfoGO.transform.localScale = scaleFactor;
            foreach (var book in m_booksData.books)
            {
                Debug.Log("[ARImageTracker] book.title = " + book.title);
                Debug.Log("[ARImageTracker] name = " + name);

                if (book.title == name)
                {
                    //Debug.Log("[ARImageTracker] book.title = name");

                    //m_ARInfoGO.SetActive(true);
                    if (!m_BookDataDisplayer.IsActive)
                    {
                        Debug.Log("[ARImageTracker] m_BookDataDisplayer is not active");

                        m_BookDataDisplayer.ReceiveBookDatasAndDisplayUI(book);
                    }
                    //m_BookDataDisplayer.DisplayUI();                    
                }
            }
        }
    }
}