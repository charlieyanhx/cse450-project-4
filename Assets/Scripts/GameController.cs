using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    //outlets
    public Transform[]spawnPoints;
    public GameObject[]asteroidPrefabs;
    public GameObject explosionPrefab;
    public Text textScore;
    public Text textMoney;
    public Text missilespeedUpgradeText;
    public Text bonusUpgradeText;

    //configuration
    public float minAsteroidDelay = 0.2f;
    public float maxAsteroidDelay = 2f; 

    //state tracking
    public float timeElapsed;
    public float asteroidDelay;
    public int score;
    public int money;
    public float missileSpeed = 2f;
    public float bonusMultiplier = 1f;

    //methods
    void Awake()
    {
        instance=this;
    }

    private void Start()
    {
        StartCoroutine("AsteroidSpawnTimer");
        score = 0;
        money = 0; 
    }

    void Update()
    {
        //Increment passage of time for each frame of the game
        timeElapsed += Time.deltaTime;

        //compute asteroid delay
        float decreaseDelayOverTime = maxAsteroidDelay - ((maxAsteroidDelay - minAsteroidDelay) / 30f * timeElapsed);
        asteroidDelay = Mathf.Clamp(decreaseDelayOverTime, minAsteroidDelay, maxAsteroidDelay);

        UpdateDisplay();
    }

    void SpawnAsteroid()
    {
        //pick random spawn points and prefabs
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject randomAsteroidPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        //spawn
        Instantiate(randomAsteroidPrefab, randomSpawnPoint.position, Quaternion.identity);
    }

    public void EarnPoints(int pointAmount)
    {
        score += Mathf.RoundToInt(pointAmount*bonusMultiplier);
        money += Mathf.RoundToInt(pointAmount * bonusMultiplier);
    }

    void UpdateDisplay()
    {
        textScore.text = score.ToString();
        textMoney.text = money.ToString();
    }

    public void UpgradeMissileSpeed()
    {
        int cost = Mathf.RoundToInt(25 * missileSpeed);
        if (cost <= money)
        {
            money -= cost;
            missileSpeed += 1f;
            missilespeedUpgradeText.text = "Missile Speed $" + Mathf.RoundToInt(25 * missileSpeed).ToString();
        }
    }

    public void UpgradeBonus()
    {
        int cost = Mathf.RoundToInt(100 * bonusMultiplier);
        if (cost <= money)
        {
            money -= cost;
            bonusMultiplier += 1f;
            bonusUpgradeText.text = "Multiplier $" + Mathf.RoundToInt(25 * bonusMultiplier).ToString();
        }
    }


    IEnumerator AsteroidSpawnTimer()
    {
        //wait
        yield return new WaitForSeconds(asteroidDelay);

        //spawn
        SpawnAsteroid();

        //repeat
        StartCoroutine("AsteroidSpawnTimer");
    }
}
