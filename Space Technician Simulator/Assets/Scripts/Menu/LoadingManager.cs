using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneReference
{
    public string SceneName;
}

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private string sceneName;

    private void Start()
    {
        LoadGameScene();
    }

    public void LoadGameScene()
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressText.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";

            if (operation.progress >= 0.9f)
            {
                progressText.text = "Press any key to continue...";
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
