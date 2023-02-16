using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCaster : MonoBehaviour
{
    private Collider _damageCasterCollider;
    public int Damage = 30;
    private List<Collider> _damagedTargetList;

    private void Awake()
    {
        _damageCasterCollider = GetComponent<Collider>();
        _damageCasterCollider.enabled = false;
        _damagedTargetList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (!_damagedTargetList.Contains(other))
        {
            var targetCC = other.GetComponent<Character>(); //TODO: replace with Enemy

            if (targetCC != null)
            {
                targetCC.ApplyDamage(Damage);
            }

            _damagedTargetList.Add(other);
        }
    }

    public void EnableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = true;
    }

    public void DisableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = false;
    }
}
