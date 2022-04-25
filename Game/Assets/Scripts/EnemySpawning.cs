using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public float globalTimer;
    public float posX;
    public float posY;
    public GameObject player;
    public int PlusOrMinus;
    public GameObject E1;
    public GameObject E2;
    public GameObject E3;
    public int whichEnemy;
    PlayerHealth ph;

    public List<GameObject> enemies = new();
    public List<GameObject> enemiesToRemove = new ();
    public float maxDist;

    private float minSpawnTime = 8f;
    private float maxSpawnTime = 15f;

    private void Awake()
    {
        ph = GameObject.Find("Player").GetComponent<PlayerHealth>();
        globalTimer = 4f;
        StartCoroutine(despawn());
        StartCoroutine(decreaseSpawnTime());
    }

    private IEnumerator decreaseSpawnTime()
    {
        while(true)
        {
            if (minSpawnTime > 2.5f) minSpawnTime -= 0.125f;
            if (minSpawnTime < 2.5f) minSpawnTime = 2.5f;
            if (maxSpawnTime > 6.5f) maxSpawnTime -= 0.1f;
            if (maxSpawnTime < 6.5f) maxSpawnTime = 6.5f;
            yield return new WaitForSecondsRealtime(15);
        }
    }

    private void Update()
    {
        if (SceneManagement.paused) return;
        if (ph.dead) return;
        globalTimer -= Time.deltaTime;
        if(globalTimer <= 0)
        {
            GenerateEnemyCoords();
            globalTimer = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    public void GenerateEnemyCoords()
    {
        PlusOrMinus = Random.Range(0, 2);
        if(PlusOrMinus == 0)
        {
            posX = Random.Range(5, 9);
        } else
        {
            posX = Random.Range(-8, -4);
        }
        PlusOrMinus = Random.Range(0, 2);
        if (PlusOrMinus == 0)
        {
            posY = Random.Range(5, 9);
        }
        else
        {
            posY = Random.Range(-8, -4);
        }
        SpawnEnemy();
    }
    public void SpawnEnemy()
    {
        GameObject Enemy;
        whichEnemy = Random.Range(1, 101);
        if(whichEnemy >= 1 && whichEnemy <= 50)
        {
            Enemy = Instantiate(E1, new Vector3(player.transform.position.x + posX, player.transform.position.y + posY, 0), player.transform.rotation);
        }else if
        (whichEnemy >= 51 && whichEnemy <= 80)
        {
            Enemy = Instantiate(E2, new Vector3(player.transform.position.x + posX, player.transform.position.y + posY, 0), player.transform.rotation);
        }else
        {
            Enemy = Instantiate(E3, new Vector3(player.transform.position.x + posX, player.transform.position.y + posY, 0), player.transform.rotation);
        }
        EnemyMechanics1.EnemySaves.Add(Enemy.GetComponent<Rigidbody2D>());
        enemies.Add(Enemy);
    }

    private IEnumerator despawn()
    {
        while(true)
        {
            foreach(GameObject g in enemies)
            {
                if (g == null) enemiesToRemove.Add(g);
                else if (Vector2.Distance(player.transform.position, g.transform.position) > maxDist) Destroy(g);
            }
            foreach (GameObject g in enemiesToRemove) enemies.Remove(g);
            enemiesToRemove.Clear();

            yield return new WaitForSecondsRealtime(5);
        }
    }

}
