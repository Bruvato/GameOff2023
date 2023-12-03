using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LoseTrigger : MonoBehaviour
{
    private float elapsedTime;
    private Color currentColor = new Color(1, 0, 0, 0f);
    [SerializeField] private float duration;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerStay(Collider other)
    {

        elapsedTime += Time.deltaTime;
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        if (seconds > 6)
        {
            gameManager.Restart();
            elapsedTime = 0;
        }
        else if (seconds > 1)
        {
            float lerp = Mathf.PingPong(elapsedTime, duration) / duration;
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(currentColor, new Color(1, 0, 0, 0.5f), lerp);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<Renderer>().material.color = currentColor;
        elapsedTime = 0;
    }

}
