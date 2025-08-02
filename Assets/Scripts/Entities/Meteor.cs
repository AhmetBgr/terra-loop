using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Meteor : Entity
{
    public SoundEffect meteorImpact;
    public GameObject craterPrefab;
    Tween moveTween;
    protected override IEnumerator Start()
    {
        yield return base.Start();
        col.enabled = true;

    }
    /*protected override void OnTriggerEnter2D(Collider2D other)
    {
        //CheckForDestroy(other);
    }*/
    protected override void OnTriggerEnter2D(Collider2D other)
    {

        if (!isPlaced && other.TryGetComponent(out Earth earth))
        {
            moveTween.Kill();
            parent.DOKill(other);
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;

            parent.LookAt(Earth.instance.transform.position);
            float angle = -parent.localRotation.eulerAngles.x;

            parent.localRotation = Quaternion.Euler(0f, 0f, angle);


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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("EarthGF"))
        {
            Vector2 dir = (Earth.instance.transform.position - transform.position).normalized; 
            rb.AddForce(dir * 0.25f);
        }

    }
    public override void OnReachedEarth(EntityType type)
    {


        base.OnReachedEarth(type);

        DOVirtual.DelayedCall(0.002f, () =>
        {

            //crater.transform.localRotation = parent.localRotation;

        });


        //crater.transform.localRotation = parent.localRotation; // Quaternion.Euler(0f, 0f, parent.localRotation.eulerAngles.z -0);
        //Debug.LogWarning("meteor parent rotation: " + parent.localRotation.eulerAngles);
        //Debug.LogWarning("crater parent rotation: " + crater.transform.localRotation.eulerAngles);
        var crater = Instantiate(craterPrefab, Earth.instance.entityHolder);
        crater.transform.localPosition = Vector3.zero;

        /*crater.transform.LookAt(transform.position);

        float angle = -crater.transform.localRotation.eulerAngles.x;
        if (transform.position.x < 0)
        {
            //angle = 90 - angle;
        }

        crater.transform.localRotation = Quaternion.Euler(0f, 0f, angle);*/

        Vector2 direction = transform.position - crater.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        crater.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        DestroyEntity();
    }
    public override void DestroyEntity()
    {
        SoundController.instance.PlayAudio(destroyedSFX);
        SoundController.instance.PlayAudio(meteorImpact);

        Earth.instance.entities.Remove(this);

        var particle = ParticleSpawner.instance.SpawnParticle(ParticleSpawner.instance.destoryPrefab2, transform.position);

        particle.transform.parent = Earth.instance.transform;

        Destroy(parent.gameObject);
    }
    public void Move(Vector3 targetPos)
    {
        var scale = view.localScale;
        view.localScale = Vector3.zero;
        view.DOScale(scale, 1f);

        parent = transform.parent;

        rb.AddForce((targetPos - transform.position).normalized * Random.Range(10, 30));
        moveTween = DOVirtual.DelayedCall(40f, () => { Destroy(parent.gameObject); });
        //moveTween = parent.DOMove(targetPos, 10f)/*.OnComplete(() => DestroyEntity())*/;
    }
    public override void CheckForDestroy(Collider2D other)
    {
        /*if (other.TryGetComponent(out Earth earth))
        {
            Debug.Log("Meteor reached Earth, destroying itself.");
            rb.DOKill();
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            DestroyEntity();
        }*/
    }
}
