using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    float spawnDelay;
    [SerializeField]
    Enemy enemyPrefab;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
        }
        InvokeRepeating("Spawn", 0.0f, spawnDelay);
    }
    public void Spawn()
    {
        // —павним врага на текущей позиции точки спавна
        SpawnEnemy(transform.position);
    }

    public void SpawnEnemy(Vector3 position)
    {
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        Enemy newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        newEnemy.Target = player.transform;

        CharacterController characterController = FindObjectOfType<CharacterController>();
        if (characterController != null)
        {
            characterController.AddEnemy(newEnemy);
            Debug.Log("New enemy added to enemies array");
        }
        else
        {
            Debug.LogWarning("CharacterController not found!");
        }
    }


    public void SpawnEnemyAtRandomPosition()
    {
        Vector3 randomPosition = GenerateRandomPositionAroundPlayer(player.transform.position, 10f, 20f);

        if (randomPosition != Vector3.zero)
        {
            SpawnEnemy(randomPosition);
        }
        else
        {
            Debug.LogWarning("Failed to generate random position. Spawning at default position.");
            // ¬ случае неудачи спавним врага на текущей позиции точки спавна
            Spawn();
        }
    }

    
    Vector3 GenerateRandomPositionAroundPlayer(Vector3 playerPosition, float minRadius, float maxRadius)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minRadius, maxRadius);
        Vector3 randomPosition = playerPosition + new Vector3(randomDirection.x, 0f, randomDirection.y) * randomDistance;

       
        RaycastHit hit;
        if (Physics.Raycast(randomPosition, Vector3.down, out hit))
        {           
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(hit.point, out navMeshHit, maxRadius, NavMesh.AllAreas))
            {
                return navMeshHit.position;
            }
        }

        // ≈сли не удалось найти позицию на навмеше, пытаемс€ еще раз, использу€ оригинальную позицию
        NavMeshHit navMeshHitSecondAttempt;
        if (NavMesh.SamplePosition(randomPosition, out navMeshHitSecondAttempt, maxRadius, NavMesh.AllAreas))
        {
            return navMeshHitSecondAttempt.position;
        }
        
        // ≈сли и это не удалось, возвращаем позицию, которую удалось найти
        return randomPosition;
    }
}