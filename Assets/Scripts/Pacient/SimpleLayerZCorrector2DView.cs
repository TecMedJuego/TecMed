using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLayerZCorrector2DView : MonoBehaviour
{
    private SpriteRenderer render;
    // Start is called before the first frame update
    void Awake()
    {
        render = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //render.rendererPriority = (int)(transform.position.y * 100);
        var h = render.bounds.min.y;
        render.sortingOrder = -(int)(h* 100);
    }
}
