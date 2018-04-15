using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAEnemyDetector : MonoBehaviour
{
    [Header("Debug")] public bool drawGizmos;


    [Header("IA Settings")] public LayerMask layerEnemies;
    public float detectorRadius;

    private List<Enemy> _enemiesDetected = new List<Enemy>();

    void FixedUpdate()
    {
        DetectEnemies();
    }
    
    public List<Enemy> EnemiesDetected
    {
        get { return _enemiesDetected; }
    }

    private void DetectEnemies()
    {
        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, detectorRadius, layerEnemies);
        _enemiesDetected.Clear();
        if (enemiesColliders != null)
        {
            foreach (Collider enemyCollider in enemiesColliders)
            {
                _enemiesDetected.Add(enemyCollider.gameObject.GetComponentInParent<Enemy>());
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawWireSphere(transform.position, detectorRadius);
        }
    }
}