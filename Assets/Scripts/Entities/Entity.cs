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
    public Transform view;
    public Rigidbody2D rb;
    public Transform rangeTransform;
    public Transform scoreBubbleSpawnTransform;
    public SoundEffect fallSFX;
    public SoundEffect destroyedSFX;

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

        if(Controller.instance.gameState == GameState.Ended)
        {
            OnGameEnded();

        }
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

        }
        else
        {
            CheckForDestroy(other);
        }
    }
    protected void OnGameEnded()
    {
        if (!isPlaced)
        {
            parent.DOScale(Vector3.zero, 0.5f)
                .SetEase(Ease.InBack)
                .OnComplete(() => {
                    Destroy(parent.gameObject);
                    //DestroyEntity();
                });
        }
    }
    public virtual void DestroyEntity()
    {
        SoundController.instance.PlayAudio(destroyedSFX);

        Earth.instance.entities.Remove(this);

        ParticleSpawner.instance.SpawnParticle(ParticleSpawner.instance.destoryPrefab1, transform.position);

        Destroy(parent.gameObject);
    }
    public virtual void CheckForDestroy(Collider2D other)
    {
        DestroyEntity();
    }

    public void MoveDown(Vector3 position, float speed)
    {
        transform.DOMove(position, 0.2f).OnComplete(() => {

            parent.DOKill();
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            Vector3 pos = transform.position;
            parent.SetParent(Earth.instance.entityHolder);
            parent.localPosition = Vector3.zero;
            transform.position = pos;
            Earth.instance.AddEntity(this);
            OnReachedEarth(entityType);
        });
        SoundController.instance.PlayAudio(fallSFX); 
        //rb.AddForce(Vector2.down * speed, ForceMode2D.Force);
        col.enabled = true;

    }

    public virtual void OnReachedEarth(EntityType type)
    {
        isPlaced = true;
        ReachedEarth?.Invoke(entityType);
        PlayRangeEffect();
        SoundController.instance.PlayAudio(SoundController.instance.entityPlaced);

    }
    public void MoveTowards(Transform entityParent, float delay = 0f,UnityAction onComplete = null)
    {
        col.enabled = false;
        isActive = false;


        parent.DOLocalRotate(entityParent.localRotation.eulerAngles, 20f)
            .OnKill( () => {
                col.enabled = true;
                isActive = true;
            })
            .SetDelay(delay)
            .SetSpeedBased()
            .OnComplete(() => {
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
