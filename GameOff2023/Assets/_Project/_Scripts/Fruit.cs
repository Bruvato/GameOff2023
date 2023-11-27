using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
