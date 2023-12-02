using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fruitList;
    [SerializeField] private Transform spawnPoint;

    private GameObject currentFruit;


    private void Awake()
    {
        currentFruit = GetNewFruit();

        for (int i = 0; i < fruitList.Length; i++)
        {
            fruitList[i].GetComponent<Fruit>().fruitIndex = i;
        }
    }

    private void Update()
    {
        if (currentFruit != null)
        {
            currentFruit.transform.position = spawnPoint.position;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropFruit();
            }

        }
    }


    public void DropFruit()
    {
        currentFruit.GetComponent<Rigidbody>().isKinematic = false;
        currentFruit.GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentFruit.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        currentFruit = null;
        Invoke("GetNewFruit", 1f);

    }

    private GameObject GetNewFruit()
    {
        int index = UnityEngine.Random.Range(0, 4);

        GameObject fruit = ObjectPoolManager.SpawnObject(fruitList[index], spawnPoint.position, quaternion.identity, ObjectPoolManager.PoolType.GameObject);


        currentFruit = fruit;

        return fruit;
    }

    public void SpawnFruit(int size, Vector3 location)
    {
        ObjectPoolManager.SpawnObject(fruitList[size], location, quaternion.identity, ObjectPoolManager.PoolType.GameObject);
    }

}
