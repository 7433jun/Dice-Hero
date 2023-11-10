using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    public GameObject canvas;
    public GameObject linePrefab;

    private LineRenderer currentLine;
    private Vector2 mousePosition;

    RectTransform canvasRectTransform;
    RectTransform imageRectTransform;
    Vector2 startCanvasLocalPoint;
    Vector2 endCanvasLocalPoint;

    private GameObject spawnedImage;

    void Start()
    {
        canvasRectTransform = canvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
            //Vector2 mousePosition;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, Camera.main, out mousePosition);
            //
            //// �̹��� �������� ĵ������ ����
            //GameObject spawnedImage = Instantiate(linePrefab, mousePosition, Quaternion.identity);
            //
            //// �θ� ĵ������ ����
            //spawnedImage.transform.SetParent(canvas.transform, false);


            // 3D ������Ʈ�� ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
            Vector3 objectScreenPoint = Camera.main.WorldToScreenPoint(GameManager.instance.SelectObject().transform.position);
            
            // ��ũ�� ��ǥ�� ĵ������ ���� ��ǥ�� ��ȯ
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, objectScreenPoint, Camera.main, out startCanvasLocalPoint);
            
            // �̹��� �������� ĵ������ ����
            spawnedImage = Instantiate(linePrefab, startCanvasLocalPoint, Quaternion.identity);
            
            // �θ� ĵ������ ����
            spawnedImage.transform.SetParent(canvas.transform, false);

            // �̹��� RectTransform ������ ����
            imageRectTransform = spawnedImage.GetComponent<RectTransform>();
        }

        if (Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, Camera.main, out endCanvasLocalPoint);

            // ���� ���
            float length = Vector2.Distance(startCanvasLocalPoint, endCanvasLocalPoint);
            imageRectTransform.sizeDelta = new Vector2(10f, length);

            // ���� ���
            float angle = Vector2.SignedAngle(Vector2.down, startCanvasLocalPoint - endCanvasLocalPoint);
            imageRectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Destroy(spawnedImage);
        }
    }
}
