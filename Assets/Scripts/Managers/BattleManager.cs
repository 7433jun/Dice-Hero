using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] DiceManager diceManager;

    [SerializeField] Player player;
    [SerializeField] List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private List<Die> readyDiceList = new List<Die>();

    private GameObject selectObject;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        GameManager.instance.state = Enums.State.battle;

        BattleStart();
    }

    void Update()
    {
        if (GameManager.instance.state == Enums.State.playerTurn)
        {
            PlayerTurn();
        }
    }

    private void BattleStart()
    {
        Debug.Log("��������");

        StartCoroutine(StartPlayerTurn());
    }

    IEnumerator StartPlayerTurn()
    {
        diceManager.StartPlayerTurnDice();

        yield return new WaitForSeconds(1f);

        Debug.Log("�÷��̾� ��");

        GameManager.instance.state = Enums.State.playerTurn;
    }

    private void PlayerTurn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!diceManager.isDiceRolling())
            {
                selectObject = GameManager.instance.SelectObject();

                if (selectObject.GetComponent<Die>() != null)
                {
                    Die die = selectObject.GetComponent<Die>();

                    if (diceManager.activeReRoll)
                    {
                        return;
                    }

                    if (die.useable)
                    {
                        if (readyDiceList.Contains(die))
                        {
                            readyDiceList.Remove(die);
                            OutLineOff(die);
                        }
                        else
                        {
                            readyDiceList.Add(die);
                            OutLineOn(die);
                        }
                    }
                }
                else if (selectObject.GetComponent<Enemy>() != null)
                {
                    Enemy enemy = selectObject.GetComponent<Enemy>();

                    if (readyDiceList.Count != 0)
                    {
                        int total = 0;
                        foreach (var die in readyDiceList)
                        {
                            OutLineOff(die);

                            total += die.value;

                            // �׳� Color.gray�� ���� �߶�� �ѵ�
                            //DieColor(die, new Color(0.88f, 0.88f, 0.88f));
                            DieColor(die, Color.gray);

                            die.useable = false;

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

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("���ʹ� ��");

        foreach (Enemy enemy in enemies)
        {
            EnemyHit(enemy);
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine(StartPlayerTurn());
    }

    private bool CheckPlayerWin()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.currentHealth <= 0)
            {
                return true;
            }
        }

        return false;
    }

    private void OutLineOn(Die die)
    {
        die.GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", 0.15f);
    }

    private void OutLineOff(Die die)
    {
        die.GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", 0f);
    }

    private void DieColor(Die die, Color color)
    {
        die.GetComponent<Renderer>().material.color = color;
    }

    private void PlayerHit(Enemy enemy, int damage)
    {
        player.GetComponent<Animator>().SetTrigger("Attack");
        enemy.GetComponent<Animator>().SetTrigger("Hit");

        enemy.currentHealth -= damage;

        if (enemy.currentHealth < 0)
        {
            enemy.currentHealth = 0;
        }

        enemy.GetComponentInChildren<Slider>().value = (float)enemy.currentHealth / (float)enemy.maxHealth;

        enemy.GetComponentInChildren<TextMeshProUGUI>().text = $"{enemy.currentHealth}/{enemy.maxHealth}";

        if (CheckPlayerWin())
        {
            GameManager.instance.Path();
        }
    }

    private void EnemyHit(Enemy enemy)
    {
        player.GetComponent<Animator>().SetTrigger("Hit");
        enemy.GetComponent<Animator>().SetTrigger("Attack");

        player.currentHealth -= enemy.Enemydamage();

        if (player.currentHealth < 0)
        {
            player.currentHealth = 0;
        }

        player.GetComponentInChildren<Slider>().value = (float)player.currentHealth / (float)player.maxHealth;

        player.GetComponentInChildren<TextMeshProUGUI>().text = $"{player.currentHealth}/{player.maxHealth}";
    }

    public void EndPlayerTurnButton()
    {
        Debug.Log("�÷��̾� �� ����");

        GameManager.instance.state = Enums.State.enemyTurn;

        StartCoroutine(EnemyTurn());
    }
}

// �巡�� ������ �÷��̾� ����
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
*/