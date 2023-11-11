using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

                // ������Ʈ�� �ֻ������� �Ǻ�
                if (selectDie.GetComponent<Die>() != null)
                {
                    // �ֻ����� ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
                    Vector3 objectScreenPoint = Camera.main.WorldToScreenPoint(selectDie.transform.position);

                    // ��ũ�� ��ǥ�� ĵ������ ���� ��ǥ�� ��ȯ
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, objectScreenPoint, Camera.main, out startCanvasLocalPoint);

                    // �̹��� �������� ĵ������ ����
                    spawnedImage = Instantiate(linePrefab, startCanvasLocalPoint, Quaternion.identity);

                    // �θ� ĵ������ ����
                    spawnedImage.transform.SetParent(canvas.transform, false);

                    // �̹��� RectTransform ������ ����
                    imageRectTransform = spawnedImage.GetComponent<RectTransform>();

                    // �÷��� Ȱ��ȭ
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

                // ���� ���
                float length = Vector2.Distance(startCanvasLocalPoint, endCanvasLocalPoint);
                imageRectTransform.sizeDelta = new Vector2(30f, length);

                // ���� ���
                float angle = Vector2.SignedAngle(Vector2.down, startCanvasLocalPoint - endCanvasLocalPoint);
                imageRectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (selectEnemy.GetComponent<Enemy>() != null)
                {
                    HitEnemy(selectEnemy.GetComponent<Enemy>(), selectDie.GetComponent<Die>());
                }

                // �÷��� ����
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
