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

    //������������� �������
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
        LookAtTarget(); //������������ ��������� � ������� 
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
        for (int i = 0; i < collision.contacts.Length; i++) //��������� ��� ����� ���������������
        {
            if (collision.contacts[i].point.y < transform.position.y)   //���� ����� ��������������� ��������� ���� ������ ������ ���������
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

        //���� ������� ��� ���������������� �������, ������ ������ 1, 
        //��������� ������ ������ �������� 1,41... (���������� ������ �� ����).
        //�� ���� �������� ����� ������� ������ �� ���������, ��� ������ �� ����� �� ����.
        //����� ����� �� ����, ����������� �������������� ������ (��������� ��� ����� ������ 1):
        resultXZForce.Normalize();

        resultXZForce = resultXZForce * movingForce;

        _rigidbody.AddForce(resultXZForce);
    }

    private void LookAtTarget()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        float distanse; ;
        //������� ������� ������, � � �� ������� �������� ���, ������ �� ������ � �� ����� ������������, ������� ��������� ��� �������� ����.
        // Input.mousePosition - ������� ��������� ������� � ������������ ������ (������ ����� ���� - 0, 0; ������� ������ ���� - ������ ����, ������ ����)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (plane.Raycast(ray, out distanse))  //���� ��� ����� � �����-�� ���������, ����� ���������� true, � ������� ��������� ������������ � ���������� hit (�������� ����� out)
        {
            Vector3 position = ray.GetPoint(distanse);  //������� �� ���� �����, ����������� �� �������� ���������� �� ������ ����. ��� ���������� ����� �� ���������� ������������ - ���������� hit. 
            //position.y = transform.position.y;  //��������� ����� ������������ ����� ���������� ���� ��� ���� ���������, ������������ �� ���������� Y � ���������� Y ���������.
            transform.LookAt(position);
        }
    }

    private void Destroyed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddEnemy(Enemy newEnemy)
    {
        enemies.Add(newEnemy); // ��������� ������ ����� � ������
        Debug.Log("New enemy added to enemies list");
    }

    // ����� ��� �������� ����� �� ������
    public void RemoveEnemy(Enemy enemyToRemove)
    {
        enemies.Remove(enemyToRemove); // ������� ����� �� ������
        Debug.Log("Enemy removed from enemies list");
    }

    // ����� ��� ������ ���������� �����
    public Enemy FindNearestEnemy()
    {
        if (enemies.Count == 0) return null; // ���������, ���� �� ������ �����

        Enemy nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null) // ���������, �� ��� �� ��������� ���� ����
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
        if (enemies.Count == 0) return; // ���������, ���� �� ������ �����

        // ������� ���������� �����
        Enemy nearestEnemy = FindNearestEnemy();

        // ���������� ���
        if (nearestEnemy != null)
        {
            RemoveEnemy(nearestEnemy); // ������� ����� �� ������
            Destroy(nearestEnemy.gameObject); // ���������� ������ �����
        }
    }
    
}