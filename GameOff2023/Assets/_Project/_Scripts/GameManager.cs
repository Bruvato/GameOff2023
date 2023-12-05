using System.Collections;
using System.Collections.Generic;
using LeaderboardCreatorDemo;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform clearer;
    [SerializeField] Transform clearStartPoint;
    [SerializeField] Transform clearEndPoint;
    [SerializeField] private bool gameOver;
    [SerializeField] LeaderboardManager leaderboardManager;
    public int Score;
    public int newScore;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        clearer.position = clearStartPoint.position;
        clearer.gameObject.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !leaderboardManager._usernameInputField.isFocused) { Restart(); }

        Score = (int)Mathf.Lerp(Score, newScore, 10 * Time.deltaTime);
        scoreText.text = Score.ToString();


        if (gameOver)
        {
            clearer.gameObject.SetActive(true);
            clearer.position = Vector3.MoveTowards(clearer.position, clearEndPoint.position, 5 * Time.deltaTime);

            if (clearer.position == clearEndPoint.position)
            {
                gameOver = false;
                clearer.position = clearStartPoint.position;
                clearer.gameObject.SetActive(false);
            }
        }
    }
    public void Restart()
    {
        gameOver = true;
        leaderboardManager.UploadEntry();
        newScore = 0;
    }

}
