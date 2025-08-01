using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGamePanel : Singleton<InGamePanel>
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public CanvasGroup canvasGroup;

    void Start()
    {
        Controller.GameEnded += OnGameEnded;
    }
    private void OnDestroy()
    {
        Controller.GameEnded -= OnGameEnded;
    }

    public void UpdateScore(int score, int curScore)
    {
        DOVirtual.Float(curScore, score, 0.25f * Mathf.Abs(score-curScore), (value) =>
        {
            scoreText.text = ((int)value).ToString();
        }).SetDelay(1f);

        //scoreText.text = score.ToString();
    }
    public void UpdateTimer(float time)
    {
        timerText.text = ((int)time).ToString();
    }

    private void OnGameEnded()
    {
        canvasGroup.DOFade(0f, 1f).SetEase(Ease.InOutCubic);
    }
}
