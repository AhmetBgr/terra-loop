using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : Singleton<Earth>
{
    public List<Entity> entities = new List<Entity>();
    private float rotationSpeed = 30f; // 360 degrees in 10 seconds
    private float defRotationSpeed = 30f; // 360 degrees in 10 seconds
    public Transform entityHolder;

    private float speedupRotationSpeed = 36f; // 360 degrees in 10 seconds

    protected override void Awake()
    {
        base.Awake();

    }
    void Start()
    {
        /*transform.DORotate(new Vector3(0, 0, 360), 10f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);*/
        speedupRotationSpeed = defRotationSpeed * 5; // Double the speed for speedup
        //defRotationSpeed = rotationSpeed; // Store the default speed

        Controller.GameEnded += OnGameEnded;
    }
    private void OnDestroy()
    {
        Controller.GameEnded -= OnGameEnded;
    }
    void Update()
    {
        if (Controller.instance.gameState == GameState.Ended /*|| Controller.instance.gameState == GameState.WaitingToStart*/)
        {
            rotationSpeed = defRotationSpeed / 5;
        }
       /* else
        {
            rotationSpeed = defRotationSpeed;
        }
       */
        if (Input.GetMouseButtonDown(1))
        {
            rotationSpeed = speedupRotationSpeed;

        }
        if (Input.GetMouseButtonUp(1))
        {
            rotationSpeed = defRotationSpeed;
        }
        
        // Rotate around Z axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);


    }
    private void OnGameEnded()
    {
        transform.DOLocalMove(Vector3.zero, 3f);
        transform.DOScale(Vector3.one * 1.5f, 3f);

    }
    public void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }   

    public List<Entity> GetNearbyEntities(Vector3 position, float range, EntityType type)
    {
        var nearbyEntities = new List<Entity>();
        foreach (var entity in entities)
        {
            if (entity == null) continue; // Skip null entities

            if (!entity.isActive) continue;

            if (type == entity.entityType && Vector2.Distance(entity.transform.position, position) <= range)
            {
                nearbyEntities.Add(entity);
            }
        }
        return nearbyEntities;
    }
}
