using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBackground : MonoBehaviour
{
    public float scrollSpeed = 1f;
    private RawImage rawImage;

    private float offset;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void FixedUpdate()
    {
        offset += Time.fixedDeltaTime * scrollSpeed;
        rawImage.uvRect = new Rect(offset, 0, 1, 1);
    }

    //private void Update()
    //{
    //    offset += Time.deltaTime * scrollSpeed;
    //    rawImage.uvRect = new Rect(offset, 0, 1, 1);
    //}
}
