using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndGameHandler : MonoBehaviour
{

    public GameObject[] Disable;
    public SpriteRenderer[] fadeOutSprite;
    public CanvasGroup[] fadeOutCanvas;
    public CanvasGroup[] fadeInCanvas;


    // Start is called before the first frame update
    void Start()
    {
        Controller.GameEnded += OnGameEnded;
    }
    private void OnDestroy()
    {
        Controller.GameEnded -= OnGameEnded;
    }
    private void OnGameEnded()
    {
        foreach (var item in Disable) {     
            item.gameObject.SetActive(false);
        }

        foreach (var item in fadeOutSprite)
        {
            item.DOColor(Color.clear, 0.5f);
        }

        foreach(var item in fadeOutCanvas)
        {
            item.DOFade(0f, 0.5f);
        }
        foreach (var item in fadeInCanvas)
        {
            item.gameObject.SetActive(true);
            item.alpha = 0;
            item.DOFade(1f, 0.5f).SetDelay(2f);
        }
    }

}
