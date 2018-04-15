using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAEnemyDetector : MonoBehaviour
{
    [Header("Debug")] public bool drawGizmos;


    [Header("IA Settings")] public LayerMask layerEnemies;
    public float detectorRadius;


    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawWireSphere(transform.position, detectorRadius);
        }
    }
}