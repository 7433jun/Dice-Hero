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
        myLogQueue.Enqueue(newString); // ���ο� �α� �޽��� �߰�

        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        }

        // �α� �޽����� 10���� �ʰ��ϴ� ���, ���� ������ �α� �޽����� ����
        while (myLogQueue.Count > 10)
        {
            myLogQueue.Dequeue();
        }

        // myLog ������ ������ 10���� �α� �޽����� ����
        myLog = string.Join("\n", myLogQueue.ToArray());
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label); // ���� GUI ��Ų�� ���̺� ��Ÿ�� ����
        style.fontSize = 40; // ���ϴ� ���� ũ��� ����

        GUILayout.Label(myLog, style); // ��Ÿ���� �����Ͽ� ���̺� ǥ��
    }
}