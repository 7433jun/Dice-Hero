using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    void Start()
    {
        
    }

    void Update()
    {

    }

    //마우스 위치의 3D 오브젝트 가져오는 함수
    public GameObject SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject selectedObject = null;

        if (Physics.Raycast(ray, out hit))
        {
            selectedObject = hit.collider.gameObject;
        }

        Debug.Log(selectedObject);

        return selectedObject;
    }
}
