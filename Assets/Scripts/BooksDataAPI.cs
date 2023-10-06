using System;
using UnityEngine;

[Serializable]
public struct BookReviewAPI
{
    public string author;
    public float note;
    [TextArea]
    public string review;
}

[CreateAssetMenu(fileName = "NewBookDataAPI", menuName = "Book Data API")]
public class BooksDataAPI : ScriptableObject
{
    [Serializable]
    public struct BookDataAPI
    {
        public string title;
        public string[] authors;
        public float averageRating;
        public float ratingsCount;
        public float bestNote;
        [TextArea]
        public string description;
        public BookReviewAPI[] reviews;
    }

    public BookDataAPI[] books;
}
