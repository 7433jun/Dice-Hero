using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EncounterAnchor : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    private RectTransform rectTransform;
    private float offset;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public IEnumerator Create()
    {
        while(rectTransform.position.x <= -500)
        {
            offset = Time.deltaTime * speed;
            rectTransform.position -= new Vector3(offset, 0, 0);
        }

        yield return null;
    }

    public IEnumerator Remove()
    {
        while(rectTransform.position.x <= -2100)
        {
            offset = Time.deltaTime * speed;
            rectTransform.position -= new Vector3(offset, 0, 0);
        }

        yield return null;

        Destroy(gameObject);
    }
}
