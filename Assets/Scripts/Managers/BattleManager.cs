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
            //// 이미지 프리팹을 캔버스에 생성
            //GameObject spawnedImage = Instantiate(linePrefab, mousePosition, Quaternion.identity);
            //
            //// 부모를 캔버스로 설정
            //spawnedImage.transform.SetParent(canvas.transform, false);


            // 3D 오브젝트의 월드 좌표를 스크린 좌표로 변환
            Vector3 objectScreenPoint = Camera.main.WorldToScreenPoint(GameManager.instance.SelectObject().transform.position);
            
            // 스크린 좌표를 캔버스의 로컬 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, objectScreenPoint, Camera.main, out startCanvasLocalPoint);
            
            // 이미지 프리팹을 캔버스에 생성
            spawnedImage = Instantiate(linePrefab, startCanvasLocalPoint, Quaternion.identity);
            
            // 부모를 캔버스로 설정
            spawnedImage.transform.SetParent(canvas.transform, false);

            // 이미지 RectTransform 변수에 저장
            imageRectTransform = spawnedImage.GetComponent<RectTransform>();
        }

        if (Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, Camera.main, out endCanvasLocalPoint);

            // 길이 계산
            float length = Vector2.Distance(startCanvasLocalPoint, endCanvasLocalPoint);
            imageRectTransform.sizeDelta = new Vector2(10f, length);

            // 각도 계산
            float angle = Vector2.SignedAngle(Vector2.down, startCanvasLocalPoint - endCanvasLocalPoint);
            imageRectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Destroy(spawnedImage);
        }
    }
}
