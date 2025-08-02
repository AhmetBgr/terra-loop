using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        //restartButton.onClick.AddListener(() => SceneLoader.Instance.TransitionToScene("Transition"));
        restartButton.onClick.AddListener(() => SceneLoader.Instance.ReloadCurrentScene());

    }
    private void Update()
    {
        scoreText.text = Controller.instance.score.ToString();
        highScoreText.text = "Highest \n" + Controller.instance.GetHighScore().ToString();

    }
    private void OnEnable()
    {
        restartButton.interactable = false;
        scoreText.text = Controller.instance.score.ToString();
        highScoreText.text = "Highest \n" + Controller.instance.GetHighScore().ToString();

        DOVirtual.DelayedCall(2f, () => { restartButton.interactable = true; });
    }
}
