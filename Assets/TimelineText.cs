using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineText : MonoBehaviour
{
    public Canvas renderCanvas;

    // Start is called before the first frame update
    // Make the object a child of the canvas and make sure it changes world position.
    // Transform the text so it is regular size.
    void Start()
    {
        gameObject.transform.SetParent(renderCanvas.transform, true);
        gameObject.transform.localScale = new Vector3(1,1,1);
    }
}
