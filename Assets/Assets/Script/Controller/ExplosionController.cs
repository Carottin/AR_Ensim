using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

    List<GameObject> currentCollisions = new List<GameObject>();
    private int m_damage = 50;
    private UnitController[] m_ennemies;

    private void Start()
    {
        // First get all ennemies which are not end objects
        m_ennemies = Array.FindAll(FindObjectsOfType<UnitController>(), x => x.IsPlayer && !x.IsEndObject);
        float distance = Mathf.Infinity;

        // Then check for the nearest
        foreach (UnitController u in m_ennemies)
        {
            Vector3 diff = (u.gameObject.transform.position - transform.position);
            float curDistance = diff.sqrMagnitude;

            // if the ennemy is near enough we apply damage
            if (curDistance < 0.009 )
            {
                u.life -= m_damage;
                if (u.life <= 0)
                    u.gameObject.SetActive(false);
            }
        }
    }
}
