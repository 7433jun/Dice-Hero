using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] List<GameObject> diceList = new List<GameObject>();
    [SerializeField] Text reRollButtonText;
    [SerializeField] GameObject grayMask;

    public int reRollCount;
    bool activeReRoll;
    bool isButtonPressed;

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

    public void ReRoll()
    {
        GameObject selectedObject = SelectObject();

        if (selectedObject.GetComponent<Die>() != null)
        {
            Roll(selectedObject);
            reRollCount--;
            reRollButtonText.text = $"리롤 {reRollCount}회";
        }

        activeReRoll = false;
        grayMask.SetActive(false);
    }

    public void ReRollButton()
    {
        if(reRollCount > 0)
        {
            if (activeReRoll == false)
            {
                activeReRoll = true;
                grayMask.SetActive(true);
            }
            else
            {
                activeReRoll = false;
                grayMask.SetActive(false);
            }
        }
    }



    private void Start()
    {
        activeReRoll = false;
        reRollButtonText.text = $"리롤 {reRollCount}회";
        RollAll();
    }

    private void Update()
    {
        if (activeReRoll)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isButtonPressed = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isButtonPressed)
                {
                    ReRoll();
                    isButtonPressed = false;
                }
            }
        }

        
    }
}
