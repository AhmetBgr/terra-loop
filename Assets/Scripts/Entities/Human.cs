using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
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
    public override void OnReachedEarth(EntityType type)
    {
        base.OnReachedEarth(type);

        Controller.instance.AddScore(1, this);

        //var nearbyTrees = Earth.instance.GetNearbyEntities(transform.position, GameData.buildingRange, EntityType.Tree);
        //foreach (var entity in nearbyTrees)
        //{
        //    Controller.instance.AddScore(1);
        //}

        var nearbyEntities = Earth.instance.GetNearbyEntities(transform.position, GameData.buildingRange, EntityType.Building);

        Entity closestbuilding = null;
        foreach (var entity in nearbyEntities)
        {
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
