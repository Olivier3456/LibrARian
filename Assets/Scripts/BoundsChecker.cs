using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsChecker
{
    // Start is called before the first frame update
    public static Bounds CalculateBounds(RectTransform transform)
    {
        Bounds bounds = new Bounds(transform.position, new Vector3(transform.rect.width, transform.rect.height, 0.0f));

        if (transform.childCount > 0)
        {
            foreach (RectTransform child in transform)
            {
                Bounds childBounds = new Bounds(child.position, new Vector3(child.rect.width, child.rect.height, 0.0f));
                bounds.Encapsulate(childBounds);
            }
        }

        return bounds;
    }

}
