using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteRunnerManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] chunkList;
    public GameObject[] pillarList;
    public GameObject currentDistanceText;
    public GameObject currentCoinsText;
    public GameObject distanceText;
    public GameObject speedText;
    public GameObject coinsText;
    public GameObject totalScoreText;
    public GameObject highScoreText;
    public int spawnDistrance = 8;
    public int pointsPerChunk = 5;
    private float TimeInChunk = 0;
    private int chunksCompleted = 0;
    private int nextChunk = 1;
    private int offsetX = 0;
    private int lastInstantiated = 0;
    private int distanceScore = 0;
    private int speedBonus = 0;
    private int coinCount = 0;
    private int coinsScore = 0;
    private bool scored = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeInChunk += Time.deltaTime;
        if (player.transform.position.z > chunksCompleted * 50 + 50 && player.GetComponent<BallControl>().alive)
        {
            chunksCompleted++;
            if(TimeInChunk > 0)
            {
                distanceScore += pointsPerChunk;
                speedBonus += Mathf.Clamp((int)(Mathf.Ceil((TimeInChunk * 220) / -100.0f + 5) * 5), 0, 10);
                //(PointManager.Instance ?? FindObjectOfType<PointManager>())?.AddPoints(pointsPerChunk + tempSpeedBonus);
                distanceText.GetComponent<TMP_Text>().SetText(distanceScore + " points");
                speedText.GetComponent<TMP_Text>().SetText(speedBonus + " points");
                coinsText.GetComponent<TMP_Text>().SetText(coinCount + " = " + coinsScore + " points");
                totalScoreText.GetComponent<TMP_Text>().SetText(distanceScore + speedBonus + coinsScore + " points");
            }
            currentDistanceText.GetComponent<TMP_Text>().SetText("Distance: " + distanceScore);
            TimeInChunk = 0;
        }
        if (nextChunk < chunksCompleted + spawnDistrance)
        {
            spawnChunk();
            nextChunk++;
        }

        if (player.GetComponent<BallControl>().restartActive && !scored)
        {
            int finalScore = distanceScore + speedBonus + coinsScore;
            (PointManager.Instance ?? FindObjectOfType<PointManager>())?.AddPoints(finalScore);
            if (finalScore > PlayerPrefs.GetInt("InfiniteRunnerHighScore"))
            {
                PlayerPrefs.SetInt("InfiniteRunnerHighScore", finalScore);
                totalScoreText.GetComponent<TMP_Text>().SetText(finalScore + " points\nNew High Score!");
            }
            highScoreText.GetComponent<TMP_Text>().SetText("High Score: " + PlayerPrefs.GetInt("InfiniteRunnerHighScore"));
            scored = true;
        }
    }

    public void CoinGet(int score)
    {
        coinCount++;
        coinsScore += score;
        currentCoinsText.GetComponent<TMP_Text>().SetText("Coins: " + coinCount);
    }

    private void spawnChunk()
    {
        int nextInstantiated = UnityEngine.Random.Range(0, chunkList.Length);
        if (nextInstantiated == lastInstantiated)
            nextInstantiated++;
        if(nextInstantiated >= chunkList.Length)
            nextInstantiated = 0;
        Instantiate(chunkList[nextInstantiated], new Vector3(offsetX, nextChunk * -2, nextChunk * 50 + 25), new Quaternion(0, 0, 0, 0));
        Instantiate(pillarList[UnityEngine.Random.Range(0, pillarList.Length)], new Vector3(offsetX, nextChunk * -2, nextChunk * 50 + 25), new Quaternion(0, 0, 0, 0));
        offsetX += UnityEngine.Random.Range(-2, 3);
        lastInstantiated = nextInstantiated;
    }
}
