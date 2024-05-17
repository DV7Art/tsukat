using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController2 : Character2
{
    private Rigidbody _rigidbody;
    [Header("Character Settings")]
    [SerializeField]
    private float movingForce = 20.0f;
    //[SerializeField]
    //private float jumpForce = 80f; 

    [SerializeField]
    private float maxSlope = 30f;

    [SerializeField]
    private float maxSpeed = 40.0f;

    private bool onGround = false;
    private float damping = 0.3f;

    //private Enemy[] enemies;
    private List<Enemy> enemies = new List<Enemy>();

    //»нициализаци€ объекта
    void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }
    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }


    private void OnCollisionStay(Collision collision)
    {
        onGround = CheckIsOnGround(collision);
    }


    void Update()
    {
        LookAtTarget(); //ѕоворачиваем персонажа к курсору 
        Shoot();
    }

    void FixedUpdate()
    {
        if (onGround)
        {
            ApplyMovingForce();
            CheckSpaceBarPressed();
        }
    }

    private void CheckSpaceBarPressed()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyNearestEnemy();
        }
    }

    private bool CheckIsOnGround(Collision collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++) //ѕровер€ем все точки соприкосновени€
        {
            if (collision.contacts[i].point.y < transform.position.y)   //если точка соприкосновени€ находитс€ ниже центра нашего персонажа
            {
                if (Vector3.Angle(collision.contacts[i].normal, Vector3.up) < maxSlope)
                {
                    return true;
                }
            }
        }
        return false;
    }


    private void ApplyMovingForce()
    {
        Vector3 xAxisForce = transform.right * Input.GetAxis("Horizontal");
        Vector3 zAxisForce = transform.forward * Input.GetAxis("Vertical");

        Vector3 resultXZForce = xAxisForce + zAxisForce;

        Vector3 dampedVelocity = _rigidbody.velocity * damping;
        dampedVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = dampedVelocity;

        //≈сли сложить два перпендикул€рных вектора, каждый длиной 1, 
        //получитс€ вектор длиной примерно 1,41... (квадратный корень из двух).
        //“о есть персонаж будет быстрее бегать по диагонали, чем строго по одной из осей.
        //„тобы этого не было, нормализуем результирующий вектор (установим его длину равной 1):
        resultXZForce.Normalize();

        resultXZForce = resultXZForce * movingForce;

        _rigidbody.AddForce(resultXZForce);
    }

    private void LookAtTarget()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        float distanse; ;
        //Ќаходим главную камеру, и с ее помощью получаем луч, идущий из камеры в ту точку пространства, котора€ находитс€ под курсором мыши.
        // Input.mousePosition - текущее положение курсора в пространстве экрана (нижний левый угол - 0, 0; верхний правый угол - ширина окна, высота окна)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (plane.Raycast(ray, out distanse))  //если луч попал в какой-то коллайдер, метод возвращает true, и выводит параметры столкновени€ в переменную hit (ключевое слово out)
        {
            Vector3 position = ray.GetPoint(distanse);  //Ќаходим на луче точку, наход€щуюс€ на заданном рассто€нии от начала луча. Ёто рассто€ние берем из параметров столкновени€ - переменной hit. 
            //position.y = transform.position.y;  //ѕоскольку точка столкновени€ может находитьс€ выше или ниже персонажа, приравниваем ее координату Y к координате Y персонажа.
            transform.LookAt(position);
        }
    }

    private void Destroyed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddEnemy(Enemy newEnemy)
    {
        enemies.Add(newEnemy); // ƒобавл€ем нового врага в список
        Debug.Log("New enemy added to enemies list");
    }

    // ћетод дл€ удалени€ врага из списка
    public void RemoveEnemy(Enemy enemyToRemove)
    {
        enemies.Remove(enemyToRemove); // ”дал€ем врага из списка
        Debug.Log("Enemy removed from enemies list");
    }

    // ћетод дл€ поиска ближайшего врага
    public Enemy FindNearestEnemy()
    {
        if (enemies.Count == 0) return null; // ѕровер€ем, есть ли вообще враги

        Enemy nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null) // ѕровер€ем, не был ли уничтожен этот враг
            {
                float distance = Vector3.Distance(enemy.transform.position, playerPosition);
                if (distance < minDistance)
                {
                    nearestEnemy = enemy;
                    minDistance = distance;
                }
            }
        }

        return nearestEnemy;
    }

    private void DestroyNearestEnemy()
    {
        if (enemies.Count == 0) return; // ѕровер€ем, есть ли вообще враги

        // Ќаходим ближайшего врага
        Enemy nearestEnemy = FindNearestEnemy();

        // ”ничтожаем его
        if (nearestEnemy != null)
        {
            RemoveEnemy(nearestEnemy); // ”дал€ем врага из списка
            Destroy(nearestEnemy.gameObject); // ”ничтожаем объект врага
        }
    }
    
}