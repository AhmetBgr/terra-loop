using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crater : Entity
{
    public GameObject meteorite;

    public int sacrifedHumanCount = 0;

    protected override IEnumerator Start()
    {
        col = GetComponent<Collider2D>();
        parent = transform.parent;

        col.enabled = true;

        Controller.GameEnded += OnGameEnded;

        isPlaced = true;

        parent.localPosition = Vector3.zero;

        yield return new WaitForSeconds(1);


    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.TryGetComponent(out Human human))
        {
            AddSacrifition();
            human.isActive = false;
            human.DestroyEntity();
        }

    }

    public void AddSacrifition()
    {
        sacrifedHumanCount++;

        rangeTransform.DOScaleX(1f * sacrifedHumanCount * 0.2f, 0.5f);

        if(sacrifedHumanCount >= 3)
        {
            rangeTransform.gameObject.SetActive(false);
            Controller.instance.AddScore(50, this);
            meteorite.SetActive(false);
            isActive = false;
        }
    }
}
