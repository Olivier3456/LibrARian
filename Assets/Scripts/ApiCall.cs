using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static BooksDataAPI;

public class ApiCall : MonoBehaviour
{
    public string baseURL = "https://www.googleapis.com/books/v1/volumes?q=";
    public TMP_Text textGameObject;
    public BookDataAPI booksDataAPI;

    private void Start()
    {
        SearchBook("Hiroya Oku", "Gantz, tome 1", "fr");
    }

    public void SearchBook(string author, string title, string langRestrict)
    {
        string formattedAuthor = UnityWebRequest.EscapeURL(author);
        string formattedTitle = UnityWebRequest.EscapeURL(title);
        string formattedlangRestrict = UnityWebRequest.EscapeURL(langRestrict);
        string formattedQuery = $"intitle:{formattedTitle}+inauthor:{formattedAuthor}&langRestrict={formattedlangRestrict}";
        string url = $"{baseURL}{formattedQuery}";

        Debug.Log(url);

        StartCoroutine(FetchData(url));
    }

    IEnumerator FetchData(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string jsonResult = www.downloadHandler.text;
                GoogleBooksResponse response = JsonUtility.FromJson<GoogleBooksResponse>(jsonResult);

                if (response.items != null && response.items.Count > 0)
                {
                    VolumeInfo bookInfo = response.items[0].volumeInfo;

                    booksDataAPI.title = bookInfo.title;
                    booksDataAPI.authors = bookInfo.authors;
                    booksDataAPI.description = bookInfo.description;
                    booksDataAPI.averageRating = bookInfo.averageRating;
                    booksDataAPI.ratingsCount = bookInfo.ratingsCount;
                    booksDataAPI.reviews = bookInfo.reviews;
                }
                else
                {
                    Debug.LogWarning("No results found.");
                }
            }
        }
    }
}
