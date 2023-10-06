using System.Collections.Generic;
[System.Serializable]
public class VolumeInfo
{
    public string title;
    public string[] authors;
    public float averageRating;
    public float ratingsCount;
    public float bestNote;
    public string description;
    public BookReviewAPI[] reviews;
}

[System.Serializable]
public class Item
{
    public VolumeInfo volumeInfo;
}

[System.Serializable]
public class GoogleBooksResponse
{
    public List<Item> items;
}