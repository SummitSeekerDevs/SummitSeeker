using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneWhenReady(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        // Hier potentiell Ladebildschirm
        while (!asyncLoad.isDone)
        {
            // Fortschritt anzeigen
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Loading: " + (progress * 100f) + "%");

            // Unity lädt bis 0.9 - dann wartet es auf die Aktivierung
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log("Scene loaded, Switching ...");
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
