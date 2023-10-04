using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneLoader : MonoBehaviour
{
    [SerializeField] List<int> _requiredSceneBuildIndex = new List<int>();
    void Awake()
    {
        for (int i = 0; i < _requiredSceneBuildIndex.Count; i++)
        {
            SceneManager.LoadScene(_requiredSceneBuildIndex[i], LoadSceneMode.Additive);
        }
    }
}
