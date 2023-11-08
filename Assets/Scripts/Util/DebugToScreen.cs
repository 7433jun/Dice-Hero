using System.Collections;
using UnityEngine;

public class DebugToScreen : MonoBehaviour
{
    string myLog;
    Queue myLogQueue = new Queue();

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        //myLog = logString;
        //string newString = "\n [" + type + "] : " + myLog;
        //myLogQueue.Enqueue(newString);
        //if (type == LogType.Exception)
        //{
        //    newString = "\n" + stackTrace;
        //    myLogQueue.Enqueue(newString);
        //}
        //myLog = string.Empty;
        //foreach (string mylog in myLogQueue)
        //{
        //    myLog += mylog;
        //}

        string newString = "[" + type + "] : " + logString;
        myLogQueue.Enqueue(newString); // 새로운 로그 메시지 추가

        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        }

        // 로그 메시지가 10줄을 초과하는 경우, 가장 오래된 로그 메시지를 제거
        while (myLogQueue.Count > 10)
        {
            myLogQueue.Dequeue();
        }

        // myLog 변수에 마지막 10개의 로그 메시지를 저장
        myLog = string.Join("\n", myLogQueue.ToArray());
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label); // 현재 GUI 스킨의 레이블 스타일 복사
        style.fontSize = 40; // 원하는 글자 크기로 설정

        GUILayout.Label(myLog, style); // 스타일을 적용하여 레이블 표시
    }
}