using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityQueue : MonoBehaviour
{
    public List<Rigidbody2D> entities;

    public GameObject humanPrefab;

    public GameObject[] otherEntityPrefabs;



    // Start is called before the first frame update
    void Start()
    {
        //UpdateEntityPositions();

        for (int i = 0; i < 10; i++)
        {
            SpawnEntity();
        }
    }


    public Entity GetNextEntity()
    {
        if (entities.Count == 0) return null;

        var entity = entities[0].GetComponentInChildren<Entity>();
        //entity.parent.localScale = Vector3.one;
        entity.parent.DOKill();
        entity.parent.localPosition = Vector3.zero;
        entities.RemoveAt(0);

        SpawnEntity(); // Spawn a new entity to keep the queue full
        //UpdateEntityPositions();
        return entity;
    }

    private void UpdateEntityPositions()
    {
        if(entities.Count == 0) return; 

        for (int i = 0; i < entities.Count; i++)
        {
            var entityParent = entities[i];

            Vector3 pos = Vector3.left * i * 0.2f;
            pos += i != 0 ?  Vector3.left * 0.3f : Vector3.zero;
            entityParent.transform.DOKill();
            entityParent.transform.DOLocalMove(pos, 0.25f);
        }

        //var entity2 = entities[0];
        //entity2.transform.localScale = Vector3.one * 1.5f;
    }

    public void SpawnEntity()
    {
        int humanCheck = Random.Range(0, 11);

        GameObject entityPrefab = humanPrefab;
        if (humanCheck <= 3)
        {
            entityPrefab = otherEntityPrefabs[Random.Range(0, otherEntityPrefabs.Length)];

        }

        GameObject entityGO = Instantiate(entityPrefab, transform);
        Vector3 pos = Vector3.left * entities.Count * 0.2f;
        pos += entities.Count != 0 ? Vector3.left * 0.3f : Vector3.zero;
        entityGO.transform.localPosition = pos;
        entities.Add(entityGO.GetComponent<Rigidbody2D>());

        UpdateEntityPositions();
    }
}
