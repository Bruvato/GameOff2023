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

    private GameObject currentFruit;
    private GameObject previousFruit;


    private void Start()
    {
        fruitDatabase.CalculateSizes();
        fruitDatabase.CalculateColors();

        SpawnFruit(spawnPoint.position);
        SpawnEffects(spawnPoint.position);


    }


    private void Update()
    {
        if (currentFruit != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !leaderboardManager._usernameInputField.isFocused)
            {
                StartCoroutine(DropFruit());
            }

        }
    }
    private void FixedUpdate()
    {
        if (currentFruit != null)
        {
            // currentFruit.transform.position = spawnPoint.position;
            currentFruit.GetComponent<Rigidbody>().MovePosition(spawnPoint.position);
        }

    }

    public void SpawnFruit(Vector3 location)
    {
        // choose random index
        int index = UnityEngine.Random.Range(0, maxFruitIndex);

        // spawn base fruit game object
        currentFruit = ObjectPooler.Instance.SpawnFromPool("Fruit", location, transform.rotation);

        // set layer to player
        currentFruit.layer = LayerMask.NameToLayer("Player");

        // initialize fruit with properties from fruit database
        currentFruit.GetComponent<Fruit>().index = index;

        // apple new material
        Material newMaterial = new Material(fruitPrefab.GetComponent<Renderer>().sharedMaterial) { color = fruitDatabase.GetFruitObject(index).color };
        currentFruit.GetComponent<Renderer>().sharedMaterial = newMaterial;

        // update scale
        currentFruit.transform.localScale = Vector3.one * fruitDatabase.GetFruitObject(index).size;
        currentFruit.GetComponent<Fruit>().currentScale = fruitDatabase.GetFruitObject(index).size;
        currentFruit.GetComponent<Fruit>().newScale = fruitDatabase.GetFruitObject(index).size;

        // update merge score
        currentFruit.GetComponent<Fruit>().mergeScore = fruitDatabase.GetFruitObject(index).mergeScore;

        // change player radius
        characterController.radius = fruitDatabase.GetFruitObject(index).size / 2;

        // move character controller to update overlping fruit
        Collider[] hitColliders = Physics.OverlapSphere(currentFruit.transform.position, fruitDatabase.GetFruitObject(index).size / 2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<Fruit>() == null)
            {
                Vector3 moveDir = -(hitCollider.transform.position - currentFruit.transform.position).normalized;
                characterController.Move(new Vector3(moveDir.x, 0, moveDir.z));
            }
        }

        // dont fall
        currentFruit.GetComponent<Rigidbody>().isKinematic = true;
    }

    public IEnumerator DropFruit()
    {
        previousFruit = currentFruit;
        currentFruit = null;

        // change layer to fruit
        previousFruit.layer = LayerMask.NameToLayer("Fruit");

        // fall
        previousFruit.GetComponent<Rigidbody>().isKinematic = false;
        previousFruit.GetComponent<Rigidbody>().velocity = Vector3.zero;
        previousFruit.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        // spawn new fruit after delay
        yield return new WaitForSeconds(delay);
        SpawnFruit(spawnPoint.position);


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


        // fruit.transform.localScale = new Vector3(fruitDatabase.GetFruitObject(index).size, fruitDatabase.GetFruitObject(index).size, fruitDatabase.GetFruitObject(index).size);
        fruit.GetComponent<Fruit>().newScale = fruitDatabase.GetFruitObject(index).size;

        // Refresh collision
        fruit.GetComponent<Rigidbody>().isKinematic = true;
        fruit.GetComponent<Rigidbody>().isKinematic = false;

        // Update current score
        GameManager.newScore += fruit.GetComponent<Fruit>().mergeScore;

        fruit.GetComponent<Fruit>().mergeScore = fruitDatabase.GetFruitObject(index).mergeScore;

        SpawnEffects(fruit.transform.position);
    }

    public void SpawnEffects(Vector3 locaiton)
    {
        ObjectPooler.Instance.SpawnFromPool("Particle", locaiton, transform.rotation);
        CameraShaker.Invoke();
    }


}
