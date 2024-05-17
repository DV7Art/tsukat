using UnityEngine;


[System.Serializable]
public class WeaponSettings : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private float damage = 40.0f;
    [SerializeField] private float shootPower = 1.0f;
    [SerializeField] private Transform gunTransform;

    public GameObject Prefab { get => prefab; set => prefab = value; }
    public AudioClip ShootSound { get => shootSound; set => shootSound = value; }
    public float Damage { get => damage; set => damage = value; }
    public float ShootPower { get => shootPower; set => shootPower = value; }
    public Transform GunTransform { get => gunTransform; set => gunTransform = value; }
}
