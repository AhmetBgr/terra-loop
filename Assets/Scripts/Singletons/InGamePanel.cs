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

    private int displayedScore = 0;
    Tween scoreTween;
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
        scoreTween.Kill();
        scoreTween = DOVirtual.Float(displayedScore, score, 0.2f * Mathf.Abs(score-displayedScore), (value) =>
        {
            scoreText.text = ((int)value).ToString();
            displayedScore = score;
        }).SetDelay(0.5f);

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
