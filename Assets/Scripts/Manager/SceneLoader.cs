using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

[assembly: InternalsVisibleTo("Tests")]

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneWhenReady(string sceneName, Action onSceneLoaded = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, onSceneLoaded));
    }

    internal IEnumerator LoadSceneAsync(string sceneName, Action onSceneLoaded)
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

        // Szene fertig geladen
        onSceneLoaded?.Invoke();
    }
}
