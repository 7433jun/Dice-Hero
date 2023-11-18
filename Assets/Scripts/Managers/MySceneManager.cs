using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    public GameObject loadingScreen;  // �ε� ȭ���� ǥ���� ���� ������Ʈ

    private void Start()
    {
        // �ʱ⿡�� �ε� ȭ���� ��Ȱ��ȭ
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    // �� ��ȯ �Լ�
    public void SwitchSceneWithLoading(string nextSceneName)
    {
        StartCoroutine(LoadSceneAsync(nextSceneName));
    }

    IEnumerator LoadSceneAsync(string nextSceneName)
    {
        // �ε� ȭ���� Ȱ��ȭ
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        Slider slider = loadingScreen.GetComponentInChildren<Slider>();

        // �񵿱������� ���� ���� ��ε�
        //AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //yield return unloadOperation;

        // ���� ���� �񵿱������� �ε�
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(nextSceneName);
        loadOperation.allowSceneActivation = false;  // ���� �ٷ� Ȱ��ȭ���� ����

        // �ε��� 90%���� �Ϸ�Ǹ� Ȱ��ȭ
        while (!loadOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            // ���⿡�� �ε� �� ���� ������Ʈ�� �� �ֽ��ϴ�.
            slider.value = progress;

            // �ε��� 90% �̻��� �� Ȱ��ȭ
            if (progress >= 0.9f)
            {
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // �ε� ȭ���� ��Ȱ��ȭ
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }
}
