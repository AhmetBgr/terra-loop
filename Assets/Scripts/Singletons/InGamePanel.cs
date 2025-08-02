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
    public Color negativeScoreColor;

    private int displayedScore = 0;
    Tween scoreTween;
    void Start()
    {
        Controller.GameEnded += OnGameEnded;
    }
    private void OnDestroy()
    {
        scoreText.transform.DOKill();
        scoreTween.Kill();

        Controller.GameEnded -= OnGameEnded;
    }

    public void UpdateScore(int score, int curScore)
    {
        if (scoreText == null) return;


        scoreTween.Kill();
        float dur = 0.1f + 0.1f * Mathf.Abs(score - displayedScore);
        float delay = 0f;
        if (score > curScore)
        {
            scoreText.transform.DOKill();
            scoreText.transform.DOScale(1.2f, dur).SetDelay(delay).OnComplete( () => {
                scoreText.transform.DOScale(1f, 0.2f);
                });
            scoreText.color = Color.white;

        }
        else if (score < curScore)
        {
            scoreText.DOKill();

            scoreText.transform.DOKill();
            scoreText.transform.DOScale(0.9f, dur).SetDelay(delay).OnComplete(() => {
                scoreText.transform.DOScale(1f, 0.2f);
            });

            scoreText.DOColor(negativeScoreColor, dur).SetDelay(delay).OnComplete(() => {
                scoreText.DOColor(Color.white, 0.2f);
            }); 
            //scoreText.color = negativeScoreColor;
        }

        scoreTween = DOVirtual.Float(displayedScore, score, dur, (value) =>
        {
            scoreText.text = ((int)value).ToString();
            displayedScore = score;
            //scoreText.color = Color.white;

        }).SetDelay(delay);

        //scoreText.text = score.ToString();
    }
    public void UpdateTimer(float time)
    {
        timerText.transform.DOComplete();
        timerText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);

        timerText.text = ((int)time).ToString();
    }

    private void OnGameEnded()
    {
        canvasGroup.DOFade(0f, 1f).SetEase(Ease.InOutCubic);
    }
}
