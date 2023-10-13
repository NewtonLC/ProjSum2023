using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineText : MonoBehaviour
{
    public GameObject scrollerContent;
    public float numScale;

    // Start is called before the first frame update
    // Make the object a child of the canvas and make sure it changes world position.
    // Transform the text so it is regular size.
    void Start()
    {
        gameObject.transform.SetParent(scrollerContent.transform, false);
        gameObject.transform.localScale = new Vector3(numScale,numScale,numScale);
    }
}
