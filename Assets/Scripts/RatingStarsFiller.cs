using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RatingStarsFiller : MonoBehaviour
{
    [SerializeField] private Image _ratingStars;
    [Range(0, 5)]
    [SerializeField] private float _rating;
    private int _numberOfStars = 5;
    private int _previousVal = 0;

    private void Start()
    {
        SetStarFilling();
    }

    void Update()
    {
        Debug.Log(_ratingStars.fillAmount);
        if (_previousVal != _rating)
        {
            SetStarFilling();
        }
    }

    private void SetStarFilling()
    {
        StarFiller(ConvertNote(_rating));
    }

    private float ConvertNote(float value)
    {
        float finalValue = 0;

        if (value < 0.1)
        {
            finalValue = 0;
        } 
        else
        {
            finalValue = value / _numberOfStars;
        }
        
        return finalValue;
    }

    public void StarFiller(float value)
    {
        _ratingStars.fillAmount = value;
    }
}
