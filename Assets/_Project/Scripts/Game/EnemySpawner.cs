using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour 
{
    public GameObject activePool;

    public List<EnemyWave> waves = new();

    public EnemyWave currentWave;
    private int currentWaveIndex = 0;
    public bool ended;

    public static EnemySpawner instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError($"There is more than one {this.GetType().Name} in the scene");
        }
        instance = this;

        foreach (EnemyWave wave in waves) 
        {
            wave.Init(activePool);
        }
    }

    public void Begin()
    {
        currentWave = waves[0];
        currentWave.Start();
    }

    public void Stop()
    {
        currentWave.Stop();
    }

    private void Update()
    {
        if (currentWaveIndex == waves.Count && currentWave.ended) ended = true;

        if (currentWaveIndex == waves.Count || !currentWave.initialized) return;

        currentWave.Update();

        if (currentWave.ended)
        {
            currentWaveIndex++;

            if (currentWaveIndex == waves.Count) return;

            currentWave = waves[currentWaveIndex];
            currentWave.Start();
        }
    }
}

[Serializable]
public class EnemyWave 
{
    public float waitAfterStart;
    public float waitAfterEnd;
    [Space]
    public List<Transform> waypoints;
    public List<SpawnSequence> spawns = new();

    public bool ended = false;
    public bool initialized = false;

    private GameObject activePool;
    private Timer timer_start;
    private Timer timer_end;
    public SpawnSequence currentSpawn;
    private int currentSpawnIndex = 0;

    public void Start() 
    {
        timer_start.playing = true;
    }

    public void Stop() 
    {
        if (!initialized) return;

        timer_start.playing = false;
        timer_end.playing = false;

        currentSpawn.Stop();
    }

    public void Update() 
    {
        timer_start.Update();
        timer_end.Update();

        if (currentSpawnIndex == spawns.Count && currentSpawn.ended && !IsAnyoneAlive()) timer_end.playing = true;

        if (currentSpawnIndex == spawns.Count || !currentSpawn.initialized) return;

        currentSpawn.Update();
        if (currentSpawn.ended)
        {
            currentSpawnIndex++;

            if (currentSpawnIndex == spawns.Count) return;

            currentSpawn = spawns[currentSpawnIndex];
            currentSpawn.Start();
        }
    }

    public void Init(GameObject activePool) 
    {
        this.activePool = activePool;

        List<Vector3> waypointsPoses = new();
        foreach (Transform point in waypoints) 
        {
            waypointsPoses.Add(point.position);
        }

        foreach (SpawnSequence spawn in spawns) 
        {
            spawn.Init(activePool, waypointsPoses);
        }

        this.timer_start = new Timer(waitAfterStart, () =>
        {
            timer_start.playing = false;
            currentSpawn = spawns[0];
            currentSpawn.Start();
        }, false);

        this.timer_end = new Timer(waitAfterEnd, () =>
        {
            timer_end.playing = false;
            ended = true;
        }, false);

        initialized = true;
    }

    private bool IsAnyoneAlive()
    {
        foreach (Transform enemy in activePool.transform)
        {
            if (enemy.gameObject.activeSelf) return true;
        }
        return false;
    }
}

[Serializable]
public class SpawnSequence 
{
    public GameObject enemyPool;
    public int enemyCount;
    public float waitAfterEnd;
    public float interval;

    public bool ended = false;
    public bool initialized = false;
    public int enemiesLeft { get; private set; }

    private GameObject activePool;

    private Timer timer_end;
    private Timer timer_spawn;

    private List<Vector3> waypoints = new();

    public void Init(GameObject activePool, List<Vector3> newWaypoints) 
    {
        this.activePool = activePool;

        waypoints = newWaypoints;

        foreach (Enemy enemy in enemyPool.transform.GetComponentsInChildren<Enemy>()) 
        {
            enemy.onDeath.AddListener(() =>
            {
                enemy.transform.SetParent(enemyPool.transform);
                enemy.transform.position = enemyPool.transform.position;
            });
        }

        timer_end = new Timer(waitAfterEnd, () =>
        {
            timer_end.playing = false;
            ended = true;
        }, false, waitAfterEnd);

        timer_spawn = new Timer(interval, () =>
        {
            Spawn();

            if (enemiesLeft <= 0)
            {
                timer_spawn.playing = false;
                timer_end.playing = true;
            }
        }, false);

        initialized = true;
    }

    public void Update() 
    {
        timer_end.Update();
        timer_spawn.Update();
    }

    public void Start() 
    {
        enemiesLeft = enemyCount;
        timer_spawn.playing = true;
    }

    public void Stop() 
    {
        if (!initialized) return;

        timer_end.playing = false;
        timer_spawn.playing = false;
    }

    public void Spawn()
    {
        GameObject enemy = enemyPool.transform.GetChild(0).gameObject;
        enemiesLeft--;
        enemy.SetActive(true);
        enemy.transform.SetParent(activePool.transform);
        enemy.GetComponent<Enemy>().Init(waypoints);
    }
}