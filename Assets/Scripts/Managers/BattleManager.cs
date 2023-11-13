using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> readyDiceList = new List<GameObject>();

    private GameObject selectObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!DiceManager.instance.isDiceRolling())
            {
                selectObject = GameManager.instance.SelectObject();

                if (selectObject.GetComponent<Die>() != null)
                {
                    if (readyDiceList.Contains(selectObject))
                    {
                        Debug.Log("리스트에서 뺌");
                        readyDiceList.Remove(selectObject);
                        OutLineOff(selectObject);
                    }
                    else
                    {
                        Debug.Log("리스트에 넣음");
                        readyDiceList.Add(selectObject);
                        OutLineOn(selectObject);
                    }
                }
                else if(selectObject.GetComponent<Enemy>() != null)
                {
                    Enemy enemy = selectObject.GetComponent<Enemy>();

                    if (readyDiceList.Count != 0)
                    {
                        int total = 0;
                        foreach(var die in readyDiceList)
                        {
                            OutLineOff(die);
                            total += die.GetComponent<Die>().value;
                        }
                        readyDiceList.Clear();

                        PlayerHit(enemy, total);
                    }
                    else
                    {

                    }
                }
            }
        }
    }

    private void OutLineOn(GameObject die)
    {
        die.GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", 0.15f);
    }

    private void OutLineOff(GameObject die)
    {
        die.GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", 0f);
    }
    private void PlayerHit(Enemy enemy, int damage)
    {
        enemy.currentHealth -= damage;

        enemy.GetComponentInChildren<Slider>().value = (float)enemy.currentHealth / (float)enemy.maxHealth;

        enemy.GetComponentInChildren<TextMeshProUGUI>().text = $"{enemy.currentHealth}/{enemy.maxHealth}";
    }
}

// 드래그 형식의 플레이어 공격
/*
public class BattleManager : MonoBehaviour
{

    [SerializeField] GameObject canvas;
    [SerializeField] GameObject linePrefab;

    [SerializeField] Player player;
    [SerializeField] List<Enemy> enemies;


    private RectTransform canvasRectTransform;
    private RectTransform imageRectTransform;
    private Vector2 startCanvasLocalPoint;
    private Vector2 endCanvasLocalPoint;

    private GameObject spawnedImage;

    private GameObject selectDie;
    private GameObject selectEnemy;

    private bool dieSelected;

    void Start()
    {
        canvasRectTransform = canvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        Play();
    }

    private void Play()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!DiceManager.instance.isDiceRolling())
            {
                selectDie = GameManager.instance.SelectObject();

                // 오브젝트가 주사위인지 판별
                if (selectDie.GetComponent<Die>() != null)
                {
                    // 주사위의 월드 좌표를 스크린 좌표로 변환
                    Vector3 objectScreenPoint = Camera.main.WorldToScreenPoint(selectDie.transform.position);

                    // 스크린 좌표를 캔버스의 로컬 좌표로 변환
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, objectScreenPoint, Camera.main, out startCanvasLocalPoint);

                    // 이미지 프리팹을 캔버스에 생성
                    spawnedImage = Instantiate(linePrefab, startCanvasLocalPoint, Quaternion.identity);

                    // 부모를 캔버스로 설정
                    spawnedImage.transform.SetParent(canvas.transform, false);

                    // 이미지 RectTransform 변수에 저장
                    imageRectTransform = spawnedImage.GetComponent<RectTransform>();

                    // 플래그 활성화
                    dieSelected = true;
                }
            }
        }

        if (dieSelected)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 targetScreenPoint;

                selectEnemy = GameManager.instance.SelectObject();

                if (selectEnemy.GetComponent<Enemy>() != null)
                {
                    targetScreenPoint = Camera.main.WorldToScreenPoint(selectEnemy.GetComponent<RectTransform>().position) + new Vector3(0f, selectEnemy.GetComponent<BoxCollider>().center.y, 0f);
                }
                else
                {
                    targetScreenPoint = Input.mousePosition;
                }

                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, targetScreenPoint, Camera.main, out endCanvasLocalPoint);

                // 길이 계산
                float length = Vector2.Distance(startCanvasLocalPoint, endCanvasLocalPoint);
                imageRectTransform.sizeDelta = new Vector2(30f, length);

                // 각도 계산
                float angle = Vector2.SignedAngle(Vector2.down, startCanvasLocalPoint - endCanvasLocalPoint);
                imageRectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (selectEnemy.GetComponent<Enemy>() != null)
                {
                    HitEnemy(selectEnemy.GetComponent<Enemy>(), selectDie.GetComponent<Die>());
                }

                // 플래그 해제
                dieSelected = false;

                Destroy(spawnedImage);
            }
        }
    }

    private void HitEnemy(Enemy enemy, Die die)
    {
        enemy.currentHealth -= die.value;

        enemy.GetComponentInChildren<Slider>().value = (float)enemy.currentHealth / (float)enemy.maxHealth;

        enemy.GetComponentInChildren<TextMeshProUGUI>().text = $"{enemy.currentHealth}/{enemy.maxHealth}";

        die.gameObject.SetActive(false);
    }
}
*/