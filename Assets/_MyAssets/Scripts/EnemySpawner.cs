using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _container = default;
    [SerializeField] private GameObject[] monstrePrefab;
    [SerializeField] private float spawnTime = 3.5f; // Temps entre chaque spawn
    [SerializeField] private float spawnRadius = 20f; // Le rayon de la zone de spawn autour du joueur

    private Transform playerTransform; // R�f�rence au transform du joueur

    private UIManager _uiManager;

    private bool _StopSpawning = false;
    private Coroutine enemySpawnCoroutine;

    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemySpawnCoroutine = StartCoroutine(SpawnEnemy(spawnTime));
    }

    private IEnumerator SpawnEnemy(float spawnTime)
    {
        
        while (!_StopSpawning)
        {
            yield return new WaitForSeconds(spawnTime);
            Vector3 spawnPosition = GetValidSpawnPosition();

            if (_uiManager.getScore() < 2000)
            {
                GameObject newEnemy = Instantiate(monstrePrefab[0], spawnPosition, Quaternion.identity);
                newEnemy.transform.parent = _container.transform;
            }
            else if (_uiManager.getScore() < 4000)
            {
                spawnTime = 2.5f;
                int randomEnemy = Random.Range(0, 2); // G�n�rer un nombre entre 0 et 1 inclus
                GameObject newEnemy = Instantiate(monstrePrefab[randomEnemy], spawnPosition, Quaternion.identity);
                newEnemy.transform.parent = _container.transform;
            }
            else
            {
                spawnTime = 1.5f;
                int randomEnemy = Random.Range(0, monstrePrefab.Length);
                GameObject newEnemy = Instantiate(monstrePrefab[randomEnemy], spawnPosition, Quaternion.identity);
                newEnemy.transform.parent = _container.transform;
            }
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 1f); // V�rifier les colliders autour de la position de spawn

        // R�essayer avec une nouvelle position si la position est invalide
        while (colliders.Length > 0)
        {
            randomOffset = Random.insideUnitCircle * spawnRadius;
            spawnPosition = playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            colliders = Physics2D.OverlapCircleAll(spawnPosition, 1f);
        }

        return spawnPosition;
    }

    public void OnPlayerDeath()
    {
        _StopSpawning = true;
        StopCoroutine(enemySpawnCoroutine);

        foreach (Transform child in _container.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
