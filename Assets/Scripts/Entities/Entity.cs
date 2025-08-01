using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum EntityType
{
    Human,
    Building,
    Tree,
    Meteor
}

public class Entity : MonoBehaviour
{
    public EntityType entityType;
    public Rigidbody2D rb;
    public Transform rangeTransform;
    public Transform scoreBubbleSpawnTransform;
    public Color color;
    public bool isActive = true;
    public Transform parent;
    private Transform moveTowardsTarget = null;
    public Collider2D col;

    public int ScoreAdded = 0;
    protected bool isPlaced = false;

    public static event Action<EntityType> ReachedEarth;

    protected virtual IEnumerator Start()
    {
        col = GetComponent<Collider2D>();
        parent = transform.parent;

        col.enabled = false;

        Controller.GameEnded += OnGameEnded;


        yield return new WaitForSeconds(1);
    }
    protected virtual void OnDestroy()
    {
        Controller.GameEnded -= OnGameEnded;

        //Controller.instance.AddScore(-ScoreAdded);
        DOTween.Kill(parent);
        DOTween.Kill(transform);

    }
    

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPlaced && other.TryGetComponent(out Earth earth))
        {
            parent.DOKill(other);
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            Vector3 pos = transform.position;
            parent.SetParent(earth.entityHolder);
            parent.localPosition = Vector3.zero;
            transform.position = pos;
            earth.AddEntity(this);
            OnReachedEarth(entityType);
        }
        else
        {
            CheckForDestroy(other);
        }
    }
    private void OnGameEnded()
    {
        if (!isPlaced)
        {
            parent.DOScale(Vector3.zero, 0.5f)
                .SetEase(Ease.InBack)
                .OnComplete(() => {
                    DestroyEntity();
                });
        }
    }
    public void DestroyEntity()
    {
        Earth.instance.entities.Remove(this);

        Destroy(parent.gameObject);
    }
    public virtual void CheckForDestroy(Collider2D other)
    {
        Destroy(parent.gameObject);
    }

    public void MoveDown(float speed)
    {
        rb.AddForce(Vector2.down * speed, ForceMode2D.Force);
        col.enabled = true;

    }

    public virtual void OnReachedEarth(EntityType type)
    {
        isPlaced = true;
        ReachedEarth?.Invoke(entityType);
        PlayRangeEffect();
    }
    public void MoveTowards(Transform entityParent, float delay = 0f,UnityAction onComplete = null)
    {
        parent.DOLocalRotate(entityParent.localRotation.eulerAngles, 20f)
            .SetDelay(delay)
            .SetSpeedBased()
            .OnComplete(() => {
                isActive = false; 
                onComplete?.Invoke();
            });
    }

    public void PlayRangeEffect()
    {
        rangeTransform.localScale = Vector3.zero;

        rangeTransform.DOScale(Vector3.one * 1f, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => {
                rangeTransform.DOScale(Vector3.zero, 0.5f)
                .SetEase(Ease.OutBack);
            });
    }
}
