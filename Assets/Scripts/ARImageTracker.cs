using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageTracker : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;
    [SerializeField]
    XRReferenceImageLibrary m_ReferenceImageLibrary;
    [SerializeField]
    ScriptableObject m_dataBase;
    [SerializeField]
    GameObject m_ARInfoPrefab;



    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    // Start is called before the first frame update
    void Start()
    {
        m_TrackedImageManager = new ARTrackedImageManager();
        m_TrackedImageManager.referenceLibrary = m_ReferenceImageLibrary;
    }

   private void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            // Handle added event
            if(newImage.referenceImage.name == m_dataBase.name)
            {
                GameObject ARBookInfo = Instantiate(m_ARInfoPrefab, newImage.transform);

            }
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            // Handle updated event
        }

        foreach (var removedImage in eventArgs.removed)
        {
            Destroy(removedImage.GetComponent<Canvas>().gameObject);
        }
    }
}
