using System.Collections;
using System.Collections.Generic;
// using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] public int index;
    [SerializeField] public int mergeScore;
    private FruitManager fruitManager;
    private Rigidbody rb;
    public float currentScale;
    public float newScale;

    private void Awake()
    {
        fruitManager = GameObject.Find("FruitManager").GetComponent<FruitManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        Fruit otherFruit = other.gameObject.GetComponent<Fruit>();
        if (otherFruit != null)
        {
            if (index == otherFruit.index)
            {
                if (rb.velocity.magnitude > other.gameObject.GetComponent<Rigidbody>().velocity.magnitude)
                {
                    ObjectPoolManager.ReturnObjectToPool(gameObject);
                    fruitManager.UpgradeFruit(other.gameObject);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        currentScale = Mathf.Lerp(currentScale, newScale, Time.fixedDeltaTime * 10);
        transform.localScale = Vector3.one * currentScale;
    }





}
