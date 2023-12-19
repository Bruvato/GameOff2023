using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clearer : MonoBehaviour
{
    private FruitManager fruitManager;
    private void Awake()
    {
        fruitManager = GameObject.Find("FruitManager").GetComponent<FruitManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Fruit otherFruit = other.gameObject.GetComponent<Fruit>();
        if (otherFruit != null)
        {
            ObjectPooler.Instance.ReturnToPool("Fruit", other.gameObject);
            fruitManager.SpawnEffects(other.gameObject.transform.position);

        }
    }
}
