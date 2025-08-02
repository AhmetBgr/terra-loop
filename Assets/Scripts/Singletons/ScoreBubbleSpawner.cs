using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreBubbleSpawner : Singleton<ScoreBubbleSpawner>
{
    public GameObject scoreBubblePrefab;
    public GameObject negativeScoreBubblePrefab;


    public Transform targetTransform;

    /*public void SpawnScoreBubble(Vector3 position)
    {
        Debug.Log($"Spawning score bubble at position: {position}");
        if (scoreBubblePrefab == null) return;

        Debug.Log($"Score bubble prefab: {scoreBubblePrefab.name}");
        Vector3 spawnPosition = position;
        spawnPosition.z = 0; // Ensure the bubble spawns in the 2D plane
        Transform bubble = Instantiate(scoreBubblePrefab, spawnPosition, Quaternion.identity).transform;
        Vector3 worldPos;

        // Get screen position of UI element
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, targetTransform.position);

        // Convert screen position to world point using main camera
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0)); // 10f = z-depth from camera

        Debug.Log($"Target position for bubble: {worldPos}");

        bubble.DOMove(worldPos, 10f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => Destroy(bubble.gameObject));
    }
    */

    public void SpawnScoreBubble(Vector3 spawnPosition, Color color, UnityAction onComplete = null)
    {
        if (scoreBubblePrefab == null) return;

        spawnPosition.z = 0;
        Transform bubble = Instantiate(scoreBubblePrefab, spawnPosition, Quaternion.identity).transform;
        SpriteRenderer bubbleSprite = bubble.GetComponent<SpriteRenderer>();

        bubbleSprite.color = color;

        MoveInArc2(bubble, targetTransform.position, onComplete);

        SoundController.instance.PlayAudio(SoundController.instance.scoreGain);

    }
    public void SpawnNegativeScoreBubble(Vector3 spawnPosition, Color color, UnityAction onComplete = null)
    {
        if (scoreBubblePrefab == null) return;

        spawnPosition.z = 0;
        Transform bubble = Instantiate(negativeScoreBubblePrefab, spawnPosition, Quaternion.identity).transform;
        SpriteRenderer bubbleSprite = bubble.GetComponent<SpriteRenderer>();

        bubbleSprite.color = color;

        MoveInArc2(bubble, targetTransform.position, onComplete);

        //SoundController.instance.PlayAudio(SoundController.instance.scoreLose);
    }


    void MoveInArc(Transform item, Vector3 endPos)
    {
        float arcHeight = 0.2f;
        float duration = 5f;

        Vector3 startPos = item.transform.position;
        Vector3 midPos = (startPos + endPos) / 2 + Vector3.up * arcHeight;

        Sequence arcSeq = DOTween.Sequence();
        arcSeq.Append(item.DOMoveY(midPos.y, duration / 2).SetEase(Ease.OutQuad));
        arcSeq.Join(item.DOMoveX(endPos.x, duration));
        arcSeq.Join(item.DOMoveZ(endPos.z, duration));
        arcSeq.Append(item.DOMoveY(endPos.y, duration / 2).SetEase(Ease.InQuad));

        arcSeq.SetEase(Ease.InCubic)
            .OnComplete(() => Destroy(item.gameObject));
    }

    void MoveInArc2(Transform item, Vector3 endPos, UnityAction onComplete = null)
    {
        float arcHeight = 0.2f;
        float duration = 1f;

        Vector3 startPos = item.transform.position;
        Vector3 midPos = (startPos + endPos) / 2 + Vector3.up * arcHeight;

        Ease xEase = item.transform.position.x > 0 ? Ease.OutExpo : Ease.InExpo;
        Ease yEase = item.transform.position.x > 0 ? Ease.InExpo : Ease.OutExpo;

        Sequence arcSeq = DOTween.Sequence();
        arcSeq.Append(item.DOMoveY(endPos.y, duration).SetEase(Ease.InCubic));
        arcSeq.Join(item.DOMoveX(endPos.x, duration).SetEase(Ease.OutSine));
        //arcSeq.Append(item.DOMoveY(endPos.y, duration / 2).SetEase(Ease.InQuad));

        arcSeq/*.SetEase(Ease.InCubic)*/
            .OnComplete(() => {
                onComplete?.Invoke();
                SoundController.instance.PlayAudio(SoundController.instance.scoreRegister);
                Destroy(item.gameObject);
            });
    }
}
