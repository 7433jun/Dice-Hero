using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] List<GameObject> diceList = new List<GameObject>();
    [SerializeField] Text reRollButtonText;

    public int reRollCount;

    public void RollAll()
    {
        foreach(var die in diceList)
        {
            Roll(die);
        }
    }

    public void Roll(GameObject die)
    {
        // 주사위가 튀어오를 방향
        Vector3 force = new Vector3(-3 + 6 * Random.value, 8 + 10 * Random.value, -3 + 6 * Random.value);
        die.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

        // 주사위가 회전하기 전 회전값 랜덤 초기화
        Quaternion randomRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        die.transform.rotation = randomRotation;
        // 주사위 회전
        die.GetComponent<Rigidbody>().AddTorque(new Vector3(200 * Random.value -200 * Random.value, 200 * Random.value - 200 * Random.value), ForceMode.Impulse);

    }

    public void GetValue()
    {
        string str = "";
        int sum = 0;

        foreach (var die in diceList)
        {
            str += die.GetComponent<Die>().value + " ";
            sum += die.GetComponent<Die>().value;
        }

        str += "= " + sum;
        Debug.Log(str);
    }

    public void SelectDie()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            GameObject selectedObject = hit.collider.gameObject;

            if(selectedObject.GetComponent<Die>() != null)
            {
                Roll(selectedObject);
            }
        }
    }
    private void Start()
    {
        reRollButtonText.text = $"리롤 {reRollCount}회";
        RollAll();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(reRollCount > 0)
            {
                SelectDie();
                reRollCount--;
                reRollButtonText.text = $"리롤 {reRollCount}회";
            }
        }
    }
}
