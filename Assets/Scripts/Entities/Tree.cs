using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Entity
{
    public override void OnReachedEarth(EntityType type)
    {
        base.OnReachedEarth(type);

        var nearbyEntities = Earth.instance.GetNearbyEntities(transform.position, GameData.buildingRange, EntityType.Building);

        foreach (var entity in nearbyEntities)
        {
            Debug.LogError($"Tree reached Earth, affecting building: {entity.name}");
            Building building = entity as Building;
            Controller.instance.AddScore(building.activeWindowsCount * 5, this);
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (!isPlaced)
            return;

        var nearbyEntities = Earth.instance.GetNearbyEntities(transform.position, GameData.buildingRange, EntityType.Building);
        Debug.Log($"Tree destroyed, affecting building: {nearbyEntities.Count}");

        foreach (var entity in nearbyEntities)
        {
            Building building = entity as Building;
            Controller.instance.AddScore(-building.activeWindowsCount * 5, this);
        }
    }

    public override void CheckForDestroy(Collider2D other)
    {
        if (!other.TryGetComponent(out Human human))
        {
            DestroyEntity();
        }
    }
}
