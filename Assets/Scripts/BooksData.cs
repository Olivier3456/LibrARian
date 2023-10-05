using System;
using UnityEngine;

[Serializable]
public struct BookReview
{
    public string author;
    public float note;
    [TextArea]
    public string review;
}

[CreateAssetMenu(fileName = "NewBookData", menuName = "Book Data")]
public class BooksData : ScriptableObject
{
    [Serializable]
    public struct BookData
    {
        public string title;
        public string author;
        public string type;
        public float averageNote;
        public float bestNote;
        [TextArea]
        public string summary;
        public BookReview[] reviews;
    }

    public BookData[] books;
}
