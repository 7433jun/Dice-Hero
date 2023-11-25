using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public Enums.State state;

    [SerializeField] GameObject battleManager;

    [SerializeField] GameObject script;

    void Start()
    {
        Path();
    }

    public void Path()
    {
        state = Enums.State.path;
        script.SetActive(true);
        battleManager.SetActive(false);
    }

    public void Battle()
    {
        battleManager.SetActive(true);
        script.SetActive(false);
    }

    // Ŭ�� ��ġ�� �ݶ��̴� ������Ʈ �������� �Լ�
    public GameObject SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject selectedObject = null;

        if (Physics.Raycast(ray, out hit))
        {
            selectedObject = hit.collider.gameObject;
        }

        return selectedObject;
    }
}
