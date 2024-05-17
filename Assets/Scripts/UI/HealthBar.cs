using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField]
    private Destructable owner;

    private Image _healthBar;
    bool _rotateBar = true;

   
    void Start()
    {
        _healthBar = gameObject.GetComponent<Image>();
        if (owner.gameObject.GetComponent<CharacterController2>() != null)
        {
            _rotateBar = false;
        }
    }

    void Update()
    {
        _healthBar.fillAmount = Mathf.InverseLerp(0.0f, owner.HitPoints, owner.HitPointsCurrent);

        //if (_rotateBar)
        //{
        //    transform.forward = Camera.main.transform.forward;

        //}
    }
}
