using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : Singleton<ParticleSpawner>
{
    public GameObject destoryPrefab1;
    public GameObject destoryPrefab2;

    public GameObject SpawnParticle(GameObject particlePrefab, Vector3 pos)
    {
        GameObject particle = Instantiate(particlePrefab, pos, Quaternion.identity);
        return particle;
    }
}
