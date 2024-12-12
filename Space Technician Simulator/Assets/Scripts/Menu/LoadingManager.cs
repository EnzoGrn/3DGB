using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEditor;

[System.Serializable]
public class SceneReference
{
    public string SceneName;
}

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private SceneAsset sceneAsset;
    [SerializeField, HideInInspector] private string sceneName;

    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }

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
                yield return new WaitForSeconds(1f);
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
