using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitIndex;
    private bool collided = false;
    private FruitManager fruitManager;

    private void Awake()
    {
        fruitManager = GameObject.Find("FruitManager").GetComponent<FruitManager>();
    }


    private void OnCollisionEnter(Collision other)
    {
        // if (collided) { return; }

        Fruit otherFruit = other.gameObject.GetComponent<Fruit>();
        if (otherFruit != null)
        {
            if (fruitIndex == otherFruit.fruitIndex)
            {
                // collided = true;
                ObjectPoolManager.ReturnObjectToPool(gameObject);
                ObjectPoolManager.ReturnObjectToPool(other.gameObject);

                Invoke("DoSomething", 1);
                fruitManager.SpawnFruit(fruitIndex + 1, (transform.position + other.transform.position) / 2);
                // Debug.Log("spawned");


            }
        }

    }

    private void DoSomething()
    {

    }






}
