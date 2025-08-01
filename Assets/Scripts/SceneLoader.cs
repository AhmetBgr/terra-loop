using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    public CanvasGroup fadePanel;
    public bool fadeOnStart = true; 
    bool isTransitioning = false;
    public bool loadTargetSceneImmediatly = false;
    public string targetScene = "SampleScene";
    float fadeDuration = 0.3f;

    // Start is called before the first frame update
    /*void Start()
    {
        fadePanel.alpha = 1.0f;
        fadePanel.DOFade(0f, 0.3f);

    }*/
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        if (fadeOnStart) { 
            fadePanel.alpha = 1.0f;
            StartCoroutine(Fade(0f));

        }

        yield return null;

        if (loadTargetSceneImmediatly)
        {
            TransitionToScene(targetScene);
        }
    }

    public void TransitionToScene(string sceneName)
    {
        if (!isTransitioning)
            StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        isTransitioning = true;

        // Fade to black
        yield return StartCoroutine(Fade(1f));

        // Load the new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Fade back in
        yield return StartCoroutine(Fade(0f));

        isTransitioning = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        fadePanel.blocksRaycasts = true;

        float startAlpha = fadePanel.alpha;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            fadePanel.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadePanel.alpha = targetAlpha;
        //fadePanel.blocksRaycasts = targetAlpha != 0;
    }

    public void ReloadCurrentScene()
    {
        SoundController.instance.Reset();
        InGamePanel.instance.Reset();
        InputController.instance.Reset();
        ScoreBubbleSpawner.instance.Reset();
        Earth.instance.Reset();
        Controller.instance.Reset();

        Scene currentScene = SceneManager.GetActiveScene();

        TransitionToScene(currentScene.name);

        /*fadePanel.DOFade(1f, 0.3f).OnComplete(() => {

            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        });*/


        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
