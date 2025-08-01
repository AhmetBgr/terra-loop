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
    private void OnEnable()
    {
        scoreText.text = Controller.instance.score.ToString();
        highScoreText.text = "Highest \n" + Controller.instance.GetHighScore().ToString();

    }
}
