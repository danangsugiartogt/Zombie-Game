using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Serializable]
    private class Wave
    {
        [Header("Normal Wave")]
        public float InitialSpawnTime;
        public float IntervalSpawnTime;
        public float MinimumSpawnDistance;
        public int LimitZombiesPerWave;
        public int ZombiesStock;
        public Enemy.Type ZombieType;
        public Transform[] SpawnPoints;
        public int BonusHp;

        public bool IsBoss;
        public int MinimumDeathZombiesToSpawn;

        [Header("Special Wave")]
        public Enemy.Type[] SWZombieTypes;
        public Transform[] SWSpawnPoints;
    }

    [Header("Zombies")]
    [SerializeField] private GameObject[] normalZombiePrefabs;
    [SerializeField] private GameObject specialZombiePrefab;
    [SerializeField] private GameObject bossZombiePrefab;

    [Header("Waves")]
    [SerializeField] private Wave[] waves;
    private Wave currentWave;
    private int waveIndex = 0;

    [Header("Player")]
    [SerializeField] private GameObject player;

    private List<GameObject> spawnnedZombieList = new List<GameObject>();

    private float remainingTimeToSpawn = 0;
    private int zombieDeathCount = 0;

    private bool isGameInitialized = false;
    private bool isOnSpecialWave = false;
    private bool isBossDefated = false;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (!isGameInitialized) return;
        if (isOnSpecialWave) return;

        if (remainingTimeToSpawn <= 0)
        {
            SpawnNewZombie();
            remainingTimeToSpawn = currentWave.IntervalSpawnTime;
        }
        else
        {
            remainingTimeToSpawn -= Time.deltaTime;
        }
    }

    private void Initialize()
    {
        InitializeNewWave();
        isGameInitialized = true;
    }

    private void CheckEndWave()
    {
        if (currentWave.IsBoss)
        {
            if(!isOnSpecialWave && zombieDeathCount == currentWave.MinimumDeathZombiesToSpawn)
                StartCoroutine(SpawnSpecialWaves());

            if (isOnSpecialWave && isBossDefated)
            {
                currentWave.ZombiesStock = 0;

                if (spawnnedZombieList.Count > 0) return;

                CheckEndGame();
            }
        }
        else
        {
            if (spawnnedZombieList.Count > 0) return;

            if (isOnSpecialWave)
            {
                CheckEndGame();
            }
            else
            {
                StartCoroutine(SpawnSpecialWaves());
            }
        }
    }

    private void CheckEndGame()
    {
        var maxWaves = waves.Length - 1;
        if (waveIndex < maxWaves)
        {
            waveIndex++;
            InitializeNewWave();
        }
        else
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.LogWarning("EndGame");
    }

    private void InitializeNewWave()
    {
        isOnSpecialWave = false;
        zombieDeathCount = 0;
        currentWave = waves[waveIndex];

        SpawnNewZombie(true);
    }

    private void SpawnNewZombie(bool isNewWave = false)
    {
        if (spawnnedZombieList.Count >= currentWave.LimitZombiesPerWave)
            return;
        
        if(currentWave.ZombiesStock > 0)
        {
            GameObject zombieObj;
            var zombieType = currentWave.ZombieType;
            switch (zombieType)
            {
                case Enemy.Type.Normal:
                    zombieObj = SpawnNormalZombie(isNewWave);
                    break;
                case Enemy.Type.Special:
                    zombieObj = SpawnSpecialZombie();
                    break;
                case Enemy.Type.Boss:
                    zombieObj = SpawnBossZombie();
                    break;
                default:
                    zombieObj = SpawnNormalZombie();
                    break;
            }

            SetOnDestroyEventZombie(zombieObj);
            currentWave.ZombiesStock--;
        }
    }

    private IEnumerator SpawnSpecialWaves()
    {
        isOnSpecialWave = true;
        for(int i = 0; i < currentWave.SWZombieTypes.Length; i++)
        {
            var index = i;
            var zombieType = currentWave.SWZombieTypes[index];

            GameObject zombieObj;
            switch (zombieType)
            {
                case Enemy.Type.Normal:
                    zombieObj = SpawnNormalZombie();
                    break;
                case Enemy.Type.Special:
                    zombieObj = SpawnSpecialZombie();
                    break;
                case Enemy.Type.Boss:
                    zombieObj = SpawnBossZombie();
                    break;
                default:
                    zombieObj = SpawnNormalZombie();
                    break;
            }

            SetOnDestroyEventZombie(zombieObj);

            yield return new WaitForSeconds(.2f);
        }
    }

    private void SetOnDestroyEventZombie(GameObject zombieObj)
    {
        var zombie = zombieObj.GetComponent<Enemy>();
        if (zombie != null)
        {
            zombie.SetOnDestroyEvent(() =>
            {
                if (zombie.EnemyType == Enemy.Type.Boss) isBossDefated = true;

                spawnnedZombieList.Remove(zombieObj);
                zombieDeathCount++;
                CheckEndWave();
            });
        }
    }

    private GameObject SpawnNormalZombie(bool isNewWave = false)
    {
        var rand = UnityEngine.Random.Range(0, normalZombiePrefabs.Length - 1);

        // special condition
        if(waveIndex == 0)
        {
            var distance = Vector3.Distance(currentWave.SpawnPoints[rand].position, player.transform.position);
            if(distance < 5)
            {
                if (rand >= 3) rand--;
                else if (rand <= 0) rand++;
                else rand++;
            }
        }

        var zombieObj = Instantiate(normalZombiePrefabs[rand], currentWave.SpawnPoints[rand].position, Quaternion.identity);
        spawnnedZombieList.Add(zombieObj);

        var zombie = zombieObj.GetComponent<NormalZombie>();
        if (zombie != null)
        {
            zombie.Initialize();

            if (isNewWave) zombie.AddHp(currentWave.BonusHp);
        }

        return zombieObj;
    }

    private GameObject SpawnSpecialZombie()
    {
        var rand = UnityEngine.Random.Range(0, currentWave.SpawnPoints.Length - 1);
        var zombieObj = Instantiate(specialZombiePrefab, currentWave.SpawnPoints[rand].position, Quaternion.identity);

        var zombie = zombieObj.GetComponent<SpecialZombie>();
        if (zombie != null) zombie.Initialize();

        return zombieObj;
    }

    private GameObject SpawnBossZombie()
    {
        var zombieObj = Instantiate(bossZombiePrefab, currentWave.SWSpawnPoints[0].position, Quaternion.identity);

        var zombie = zombieObj.GetComponent<BossZombie>();
        if (zombie != null) zombie.Initialize();

        return zombieObj;
    }

    void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle()
        {
            fontSize = 24,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };

        GUI.Label(new Rect(10, 140, 200, 20), $"Waves: {waveIndex + 1}", labelStyle);
        GUI.Label(new Rect(10, 170, 200, 20), $"Active Zombies: {spawnnedZombieList.Count}", labelStyle);
        GUI.Label(new Rect(10, 200, 200, 20), $"Total Zombie Killed: {zombieDeathCount}", labelStyle);
    }
}
