using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : Character
{
    private Rigidbody _rigidbody;
    [Header("Character Settings")]
    [SerializeField]
    private float movingForce = 20.0f;
    [SerializeField]
    private float maxSlope = 30f;
    [SerializeField]
    private float maxSpeed = 40.0f;

    //[SerializeField] private Weapon weapon; // Поле для хранения ссылки на оружие

    private bool onGround = false;
    private float damping = 0.3f;

    //private Enemy[] enemies;
    private List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
       
    }

    private void OnCollisionStay(Collision collision)
    {
        onGround = CheckIsOnGround(collision);
    }

    private void Update()
    {
        LookAtTarget(); // Поворачиваем персонажа к курсору 
        CheckSpaceBarPressed();
        Shoot();

    }

    private void FixedUpdate()
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
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.point.y < transform.position.y && Vector3.Angle(contact.normal, Vector3.up) < maxSlope)
            {
                return true;
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

        //Если сложить два перпендикулярных вектора, каждый длиной 1, 
        //получится вектор длиной примерно 1,41... (квадратный корень из двух).
        //То есть персонаж будет быстрее бегать по диагонали, чем строго по одной из осей.
        //Чтобы этого не было, нормализуем результирующий вектор (установим его длину равной 1):
        resultXZForce.Normalize();

        resultXZForce = resultXZForce * movingForce;

        _rigidbody.AddForce(resultXZForce);
    }

    private void LookAtTarget()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        float distanse; ;
        //Находим главную камеру, и с ее помощью получаем луч, идущий из камеры в ту точку пространства, которая находится под курсором мыши.
        // Input.mousePosition - текущее положение курсора в пространстве экрана (нижний левый угол - 0, 0; верхний правый угол - ширина окна, высота окна)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (plane.Raycast(ray, out distanse))  //если луч попал в какой-то коллайдер, метод возвращает true, и выводит параметры столкновения в переменную hit (ключевое слово out)
        {
            Vector3 position = ray.GetPoint(distanse);  //Находим на луче точку, находящуюся на заданном расстоянии от начала луча. Это расстояние берем из параметров столкновения - переменной hit. 
            //position.y = transform.position.y;  //Поскольку точка столкновения может находиться выше или ниже персонажа, приравниваем ее координату Y к координате Y персонажа.
            transform.LookAt(position);
        }
    }
    public void AddEnemy(Enemy newEnemy)
    {
        enemies.Add(newEnemy); 
        Debug.Log("New enemy added to enemies list");
    }

    public void RemoveEnemy(Enemy enemyToRemove)
    {
        enemies.Remove(enemyToRemove); 
        Debug.Log("Enemy removed from enemies list");
    }

    // Метод для поиска ближайшего врага
    public Enemy FindNearestEnemy()
    {
        if (enemies.Count == 0) return null;

        Enemy nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null) // Проверяем, не был ли уничтожен этот враг
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
        if (enemies.Count == 0) return;

        Enemy nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            RemoveEnemy(nearestEnemy);
            Destroy(nearestEnemy.gameObject);
        }
    }

    private void Destroyed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

