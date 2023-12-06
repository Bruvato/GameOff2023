using System;
using System.Collections;
using System.Collections.Generic;
using LeaderboardCreatorDemo;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class FruitManager : MonoBehaviour
{
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private FruitDatabase fruitDatabase;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float delay;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private int maxFruitIndex = 4;
    [SerializeField] private GameObject ExplosionPS;
    // [SerializeField] private GameManager gameManager;
    [SerializeField] private LeaderboardManager leaderboardManager;

    public GameObject currentFruit;


    private void Awake()
    {
        fruitDatabase.CalculateSizes();
        fruitDatabase.CalculateColors();

        SpawnFruit(0, spawnPoint.position);
        SpawnEffects(spawnPoint.position);

    }


    private void Update()
    {
        if (currentFruit != null)
        {
            currentFruit.transform.position = spawnPoint.position;

            if (Input.GetKeyDown(KeyCode.Space) && !leaderboardManager._usernameInputField.isFocused)
            {
                StartCoroutine(DropFruit());
            }

        }
    }

    public void SpawnFruit(int index, Vector3 location)
    {
        // choose random index if i = 0
        if (index == 0) { index = UnityEngine.Random.Range(0, maxFruitIndex); }

        // spawn base fruit game object
        currentFruit = ObjectPoolManager.SpawnObject(fruitPrefab, location, quaternion.identity, ObjectPoolManager.PoolType.GameObject);

        // initialize fruit with properties from fruit database
        currentFruit.GetComponent<Fruit>().index = index;

        // apple new material
        Material newMaterial = new Material(fruitPrefab.GetComponent<Renderer>().sharedMaterial) { color = fruitDatabase.GetFruitObject(index).color };
        currentFruit.GetComponent<Renderer>().sharedMaterial = newMaterial;


        currentFruit.transform.localScale = new Vector3(fruitDatabase.GetFruitObject(index).size, fruitDatabase.GetFruitObject(index).size, fruitDatabase.GetFruitObject(index).size);
        currentFruit.GetComponent<Fruit>().mergeScore = fruitDatabase.GetFruitObject(index).mergeScore;

        // change player size
        characterController.radius = fruitDatabase.GetFruitObject(index).size / 2;

        currentFruit.GetComponent<Rigidbody>().isKinematic = true;
    }

    public IEnumerator DropFruit()
    {
        currentFruit.GetComponent<Collider>().enabled = true;
        currentFruit.GetComponent<Rigidbody>().isKinematic = false;
        currentFruit.GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentFruit.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        currentFruit = null;

        // spawn new fruit after drop
        yield return new WaitForSeconds(delay);
        SpawnFruit(0, spawnPoint.position);


    }

    public void UpgradeFruit(GameObject fruit)
    {
        // increment index when merge
        fruit.GetComponent<Fruit>().index += 1;
        // if index out of bounds, rest to 0
        if (fruit.GetComponent<Fruit>().index >= fruitDatabase.GetFruitsList().Length) { fruit.GetComponent<Fruit>().index = 0; }
        int index = fruit.GetComponent<Fruit>().index;

        Material newMaterial = new Material(fruitPrefab.GetComponent<Renderer>().sharedMaterial) { color = fruitDatabase.GetFruitObject(index).color };
        fruit.GetComponent<Renderer>().sharedMaterial = newMaterial;


        fruit.transform.localScale = new Vector3(fruitDatabase.GetFruitObject(index).size, fruitDatabase.GetFruitObject(index).size, fruitDatabase.GetFruitObject(index).size);
        // Vector3 newScale = new Vector3(fruitDatabase.GetFruitObject(index).size, fruitDatabase.GetFruitObject(index).size, fruitDatabase.GetFruitObject(index).size);
        // fruit.transform.localScale = Vector3.Lerp(fruit.transform.localScale, newScale, 0.01f * Time.deltaTime);

        // Refresh collision
        fruit.SetActive(false);
        fruit.SetActive(true);

        // Update current score
        GameManager.newScore += fruit.GetComponent<Fruit>().mergeScore;

        fruit.GetComponent<Fruit>().mergeScore = fruitDatabase.GetFruitObject(index).mergeScore;

        SpawnEffects(fruit.transform.position);
    }

    public void SpawnEffects(Vector3 locaiton)
    {
        ObjectPoolManager.SpawnObject(ExplosionPS, locaiton, quaternion.identity, ObjectPoolManager.PoolType.ParticleSystem);
        CameraShaker.Invoke();
    }


}
