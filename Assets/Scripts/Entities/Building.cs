using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Entity
{
    public List<SpriteRenderer> windows = new List<SpriteRenderer>();
    private List<SpriteRenderer> activeWindows = new List<SpriteRenderer>();

    public Color lightOn;
    public Color lightOff;

    public int activeWindowsCount => activeWindows.Count;

    public override void OnReachedEarth(EntityType type)
    {
        base.OnReachedEarth(type);

        var nearbyEntities = Earth.instance.GetNearbyEntities(transform.position, GameData.buildingRange, EntityType.Human);

        foreach (var entity in nearbyEntities)
        {
            AddHuman(entity as Human);
        }
    }
    public override void CheckForDestroy(Collider2D other)
    {
        if(other.TryGetComponent(out Building building) || other.TryGetComponent(out Meteor meteor))
        {
            DestroyEntity();
        }
    }

    public void AddHuman(Human human)
    {
        if (human == null || windows.Count == 0) return;

        human.col.enabled = false;
        var window = windows[0];
        activeWindows.Add(window);
        windows.RemoveAt(0);

        human.MoveTowards(parent, 0.5f, onComplete: () => {
            var nearbyTrees = Earth.instance.GetNearbyEntities(transform.position, GameData.buildingRange, EntityType.Tree);
            foreach (var entity in nearbyTrees)
            {
                Controller.instance.AddScore(5, entity);
            }


            Controller.instance.AddScore(4, this);
            window.color = lightOn;

            human.parent.parent = parent;
            human.parent.gameObject.SetActive(false);
        });

        human.isActive = false;


        //if(windows.Count == 0)
            //isActive = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (!isPlaced)
            return;

        var nearbyTrees = Earth.instance.GetNearbyEntities(transform.position, GameData.buildingRange, EntityType.Tree);

        Controller.instance.AddScore(-activeWindowsCount * (4 + nearbyTrees.Count), this);
    }
}
