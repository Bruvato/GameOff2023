using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LoseTrigger : MonoBehaviour
{
    private float elapsedTime;
    [SerializeField] private Material newMaterial;
    private Material currentMaterial;
    private void OnTriggerEnter(Collider other)
    {
        currentMaterial = other.gameObject.GetComponent<Renderer>().sharedMaterial;

    }
    private void OnTriggerStay(Collider other)
    {

        elapsedTime += Time.deltaTime;
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        if (seconds > 2)
        {
        }

        float lerp = Mathf.PingPong(elapsedTime, 2) / 2;
        other.gameObject.GetComponent<Renderer>().sharedMaterial.Lerp(currentMaterial, newMaterial, lerp);


    }
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<Renderer>().sharedMaterial = currentMaterial;
        elapsedTime = 0;
    }
}
