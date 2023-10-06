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

    private List<GameObject> m_ARInfoGOs;
    private Dictionary<string,BookDataDisplayer> m_BookDataDisplayers;

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    // Start is called before the first frame update
    void Start()
    {
        m_ARInfoGOs = new List<GameObject>();
        m_BookDataDisplayers = new Dictionary<string,BookDataDisplayer>();
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
            GameObject go = Instantiate(m_ARInfoPrefab);
            m_ARInfoGOs.Add(go);
            m_BookDataDisplayers[trackedImage.referenceImage.name] = go.GetComponent<BookDataDisplayer>();
            Debug.Log("[ARImageTracker] image added");
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            //Debug.Log("[ARImageTracker] image updated");
            UpdateARImage(trackedImage);
            //if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), BoundsChecker.CalculateBounds(m_BookDataDisplayers[trackedImage.referenceImage.name].canvasRectTransform)))
            //{
            //    //the trackedImage is outside the frustum of the camera so we can hide the element
            //    Debug.Log("[ARImageTracker] hide UI because of frustum");

            //    m_BookDataDisplayers[trackedImage.referenceImage.name].HideUI();

            //}
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            Debug.Log("[ARImageTracker] image removed");
            //m_ARInfoPrefab.SetActive(false);
            m_BookDataDisplayers[trackedImage.referenceImage.name].HideUI();
        }

    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        // Display the name of the tracked image in the canvas
        text.text = trackedImage.referenceImage.name;
        if (trackedImage.trackingState != TrackingState.Tracking)
        {
            Debug.Log("[ARImageTracker] hide");

            m_BookDataDisplayers[trackedImage.referenceImage.name].HideUI();
        }
        else
        {
            // Assign and Place Game Object
            AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position, trackedImage.transform.rotation);

            Debug.Log($"[ARImageTracker] trackedImage.referenceImage.name: {trackedImage.referenceImage.name}");
        }
    }

    void AssignGameObject(string name, Vector3 newPosition, Quaternion newRotation)
    {
        if (m_BookDataDisplayers[name] != null)
        {
            Debug.Log("[ARImageTracker] m_ARInfoGO != null");

            //m_ARInfoGO.SetActive(true);
            m_BookDataDisplayers[name].transform.position = newPosition;
            m_BookDataDisplayers[name].transform.rotation = newRotation;
            //m_ARInfoGO.transform.localScale = scaleFactor;
            foreach (var book in m_booksData.books)
            {
                Debug.Log("[ARImageTracker] book.title = " + book.title);
                Debug.Log("[ARImageTracker] name = " + name);

                if (book.title == name)
                {
                    //Debug.Log("[ARImageTracker] book.title = name");

                    //m_ARInfoGO.SetActive(true);
                    if (!m_BookDataDisplayers[name].IsActive)
                    {
                        Debug.Log("[ARImageTracker] m_BookDataDisplayer is not active");

                        m_BookDataDisplayers[name].ReceiveBookDatasAndDisplayUI(book);
                    }
                    break;
                    //m_BookDataDisplayer.DisplayUI();                    
                }
            }
        }
    }
}