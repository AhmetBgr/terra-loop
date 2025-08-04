using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Entity
{
    override protected IEnumerator Start()
    {
        yield return base.Start();
    }
    protected override void OnDestroy()
    {

        base.OnDestroy();

        if (!isPlaced)
            return;

        Controller.instance.AddScore(-1, this);
    }

    public override void DestroyEntity()
    {
        base.DestroyEntity();
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {

        base.OnTriggerEnter2D(other);
    }
    public override void OnReachedEarth(EntityType type)
    {
        base.OnReachedEarth(type);

        Controller.instance.AddScore(1, this);

        var nearbyEntities = Earth.instance.GetNearbyEntities(transform.position, Controller.instance.buildingRange, EntityType.Building);

        Entity closestbuilding = null;
        foreach (var entity in nearbyEntities)
        {

            if ((entity as Building).windows.Count == 0) continue;
                 
            if (closestbuilding == null || Vector2.Distance(entity.transform.position, transform.position) < Vector2.Distance(closestbuilding.transform.position, transform.position))
            {
                closestbuilding = entity;
            }
        }

        if(closestbuilding != null)
        {
            ((Building)closestbuilding).AddHuman(this);
            //MoveTowards(closestbuilding.parent);
            //isActive = false;

        }
    }

}
