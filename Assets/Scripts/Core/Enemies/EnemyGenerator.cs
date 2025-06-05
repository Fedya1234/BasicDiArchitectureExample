using System.Collections;
using UnityEngine;
using UniversalPool;
using Random = UnityEngine.Random;

namespace Core.Enemies
{
  public class EnemyGenerator : MonoBehaviour
  {
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private int _enemiesPoolSize = 20;
    [SerializeField] private int _enemiesMaxCount = 20;
    private void OnEnable()
    {
      StartCoroutine(SpawnCoroutine());
    }

    private void OnDisable()
    {
      StopAllCoroutines();
    }

    private IEnumerator SpawnCoroutine()
    {
      PoolFactory.FillPool(_enemyPrefab, _enemiesPoolSize, _enemiesMaxCount);
      
      while (true)
      {
        yield return new WaitForSeconds(_spawnInterval);
        
        var spawnPosition = _spawnPoint.position + Random.insideUnitSphere * _spawnRadius;
        spawnPosition.y = _spawnPoint.position.y;
        
        if (PoolFactory.TryGetInstance(out var enemy, _enemyPrefab, spawnPosition) == true)
        {
          // All settings can be set in the prefab
        }
        
      }
    }
  }
}