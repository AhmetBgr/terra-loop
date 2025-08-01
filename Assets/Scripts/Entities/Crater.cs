using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crater : Entity
{
    protected override IEnumerator Start()
    {
        yield return base.Start();

        isPlaced = true;
        parent.localPosition = Vector3.zero;
    }
}
