using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBubbleSpawner : Singleton<ScoreBubbleSpawner>
{
    public GameObject scoreBubblePrefab;

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

    public void SpawnScoreBubble(Entity entity)
    {
        if (scoreBubblePrefab == null) return;

        Vector3 spawnPosition = entity.scoreBubbleSpawnTransform.position;
        spawnPosition.z = 0;
        Transform bubble = Instantiate(scoreBubblePrefab, spawnPosition, Quaternion.identity).transform;
        SpriteRenderer bubbleSprite = bubble.GetComponent<SpriteRenderer>();
        Color color =  entity.color;

        /*if(entity.entityType == EntityType.Human)
        {
            color = Color.white;
        }
        else if (entity.entityType == EntityType.Building)
        {
            color = Color.gray;
        }
        else if (entity.entityType == EntityType.Tree)
        {
            color = Color.green;
        }*/

        bubbleSprite.color = color;

        // Get the screen position of the UI element
        /*Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, targetTransform.position); // null because Screen Space - Overlay

        // Convert screen position to world point
        Vector3 worldTargetPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane + 5f)); // Adjust Z depth

        worldTargetPos.z = 0; // Optional: Lock to 2D plane*/

        /*Debug.Log($"Target world position for bubble: {targetTransform.position}");
        bubble.DOMove(targetTransform.position, 1f) // Adjust timing here
            .SetEase(Ease.InCubic)
            .OnComplete(() => Destroy(bubble.gameObject));*/

        MoveInArc2(bubble, targetTransform.position);
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

    void MoveInArc2(Transform item, Vector3 endPos)
    {
        float arcHeight = 0.2f;
        float duration = 1f;

        Vector3 startPos = item.transform.position;
        Vector3 midPos = (startPos + endPos) / 2 + Vector3.up * arcHeight;

        Ease xEase = item.transform.position.x > 0 ? Ease.OutExpo : Ease.InExpo;
        Ease yEase = item.transform.position.x > 0 ? Ease.InExpo : Ease.OutExpo;

        Sequence arcSeq = DOTween.Sequence();
        arcSeq.Append(item.DOMoveY(endPos.y, duration).SetEase(Ease.OutSine));
        arcSeq.Join(item.DOMoveX(endPos.x, duration).SetEase(Ease.InCubic));
        //arcSeq.Append(item.DOMoveY(endPos.y, duration / 2).SetEase(Ease.InQuad));

        arcSeq/*.SetEase(Ease.InCubic)*/
            .OnComplete(() => Destroy(item.gameObject));
    }
}
