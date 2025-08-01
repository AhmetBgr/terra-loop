using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public Collider2D[] spawnBoxes;
    public Collider2D[] targetBoxes;


    // Start is called before the first frame update
    void Start()
    {
        StartSpawnLoop();
    }

    public void StartSpawnLoop()
    {
        StartCoroutine(SpawnLoop());
    }
    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));

        Transform meteor = Instantiate(meteorPrefab, GetRandomSpawnPosition(spawnBoxes), Quaternion.identity).transform; 

        Vector3 targetPos = GetRandomSpawnPosition(targetBoxes);
        //meteor.LookAt(targetPos);
        //float angle = -meteor.localRotation.eulerAngles.x;

        //meteor.localRotation = Quaternion.Euler(0f, 0f, angle);

        meteor.GetComponentInChildren<Meteor>().Move(targetPos);
        //meteor.DOMove(targetPos, 10f).OnComplete(() => Destroy(meteor.gameObject));
        //MoveInArc2(meteor, targetPos);

        StartSpawnLoop();   
    }

    private Vector3 GetRandomSpawnPosition(Collider2D[] boxes)
    {
        int index = Random.Range(0, boxes.Length);

        Collider2D spawnBox = boxes[index];
        Bounds bounds = spawnBox.bounds;
        Vector3 pos = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y), 0f
        );

        return pos;
    }

    void MoveInArc2(Transform item, Vector3 endPos)
    {
        float arcHeight = 0.2f;
        float duration = 20f;

        Vector3 startPos = item.transform.position;
        Vector3 midPos = (startPos + endPos) / 2 + Vector3.up * arcHeight;

        Ease xEase = item.transform.position.x > 0 ? Ease.OutExpo : Ease.InExpo;
        Ease yEase = item.transform.position.x > 0 ? Ease.InExpo : Ease.OutExpo;

        Sequence arcSeq = DOTween.Sequence();
        arcSeq.Append(item.DOMoveY(endPos.y, duration * Random.Range(0.2f, 1f)).SetEase(Ease.Linear));
        arcSeq.Join(item.DOMoveX(endPos.x, duration* Random.Range(0.2f, 1f)).SetEase(Ease.Linear));
        //arcSeq.Append(item.DOMoveY(endPos.y, duration / 2).SetEase(Ease.InQuad));

        arcSeq/*.SetEase(Ease.InCubic)*/
            .OnComplete(() => Destroy(item.gameObject));
    }
}
