using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    public GameObject loadingScreen;  // 로딩 화면을 표시할 게임 오브젝트

    private void Start()
    {
        // 초기에는 로딩 화면을 비활성화
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    // 씬 전환 함수
    public void SwitchSceneWithLoading(string nextSceneName)
    {
        StartCoroutine(LoadSceneAsync(nextSceneName));
    }

    IEnumerator LoadSceneAsync(string nextSceneName)
    {
        // 로딩 화면을 활성화
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        Slider slider = loadingScreen.GetComponentInChildren<Slider>();

        // 비동기적으로 현재 씬을 언로드
        //AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //yield return unloadOperation;

        // 다음 씬을 비동기적으로 로드
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(nextSceneName);
        loadOperation.allowSceneActivation = false;  // 씬을 바로 활성화하지 않음

        // 로딩이 90%까지 완료되면 활성화
        while (!loadOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            // 여기에서 로딩 바 등을 업데이트할 수 있습니다.
            slider.value = progress;

            // 로딩이 90% 이상일 때 활성화
            if (progress >= 0.9f)
            {
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // 로딩 화면을 비활성화
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }
}
