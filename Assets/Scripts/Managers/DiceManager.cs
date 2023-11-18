using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceManager : MonoBehaviour
{
    [SerializeField] List<GameObject> diceList = new List<GameObject>();
    [SerializeField] TextMeshProUGUI reRollButtonText;
    [SerializeField] GameObject grayMask;
    [SerializeField] GameObject grayMaskCamera;

    public int reRollCount;
    public bool activeReRoll = false;
    bool isButtonPressed;

    static public void Roll(GameObject die)
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

    public void StartPlayerTurnDice()
    {
        foreach (var die in diceList)
        {
            die.GetComponent<Renderer>().material.color = die.GetComponent<Die>().color;
            die.GetComponent<Die>().useable = true;
            reRollCount = 1;
            ReRollButtonText();
            Roll(die);
        }
    }

    public void GetValue()
    {
        if (isDiceRolling())
        {
            Debug.Log("구르고 있는 주사위가 있음");
            return;
        }

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

    public void ReRoll()
    {

        GameObject selectedObject = GameManager.instance.SelectObject();

        if (selectedObject.GetComponent<Die>() != null)
        {
            Roll(selectedObject);
            reRollCount--;
            ReRollButtonText();
        }

        activeReRoll = false;
        grayMask.SetActive(false);
        grayMaskCamera.SetActive(false);
    }

    public void ReRollButton()
    {
        if (isDiceRolling())
        {
            Debug.Log("구르고 있는 주사위가 있음");
            return;
        }

        if(reRollCount > 0)
        {
            if (activeReRoll == false)
            {
                activeReRoll = true;
                grayMask.SetActive(true);
                grayMaskCamera.SetActive(true);
            }
            else
            {
                activeReRoll = false;
                grayMask.SetActive(false);
                grayMaskCamera.SetActive(false);
            }
        }
        else
        {
            Debug.Log("리롤 기회가 없음");
        }
    }

    public bool isDiceRolling()
    {
        foreach(var die in diceList)
        {
            if(die.GetComponent<Die>().value == 0)
            {
                return true;
            }
        }

        return false;
    }

    private void ReRollButtonText()
    {
        reRollButtonText.text = $"리롤 {reRollCount}회";
    }


    private void Start()
    {
        ReRollButtonText();
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
