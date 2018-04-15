using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPlayerMoveController : PlayerMoveController
{
    [Header("Move Settings")] public float moveSpeed = 5;
    public float angleToPivot = 15f;

    [Header("IA Settings")] public bool enableIA; 
    public IAEnemyDetector frontEnemyDetector;
    public float chooseNewTargetElapseTime = 0.5f;

    private Enemy _targetEnemy;

    private void Start()
    {
        StartCoroutine(ChooseBestEnemyForTarget());
    }

    protected override void TurnPosition()
    {
        if (enableIA)
            TryLookAtEnemyTarget();
        else 
            base.TurnPosition();
    }

    private void TryLookAtEnemyTarget()
    {
        if (IsEnemyActiveWithHealth(_targetEnemy))
            LookAtEnemy(_targetEnemy);
    }

    private void LookAtEnemy(Enemy enemy)
    {
        Vector2 positionEnemy = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
        Vector2 positionForward = new Vector2(transform.forward.x, transform.forward.z);
        float enemyAngle = Vector2.SignedAngle(positionForward, positionEnemy);
        if (Mathf.Abs(enemyAngle) > angleToPivot)
        {
            transform.Rotate(Vector3.up * moveSpeed * -Mathf.Sign(enemyAngle) * Time.deltaTime);
        }
        else
        {
            Vector3 enemyPosition = enemy.transform.position;
            transform.LookAt(new Vector3(enemyPosition.x, 0, enemyPosition.z));    
        }
    }

    private IEnumerator ChooseBestEnemyForTarget()
    {
        if (Time.timeScale > 0 && frontEnemyDetector != null)
        {
            List<Enemy> nextEnemyCandidates = new List<Enemy>(frontEnemyDetector.EnemiesDetected);
            foreach (Enemy enemyCandidate in nextEnemyCandidates)
            {
                TryChooseEnemyForTarget(enemyCandidate);
            }
        }

        yield return new WaitForSecondsRealtime(chooseNewTargetElapseTime);
        StartCoroutine(ChooseBestEnemyForTarget());
        yield return null;
    }

    private void TryChooseEnemyForTarget(Enemy enemyCandidate)
    {
        if (!IsEnemyActiveWithHealth(_targetEnemy) ||
            (IsEnemyActiveWithHealth(enemyCandidate) && IsBestThanTarget(_targetEnemy, enemyCandidate)))
            _targetEnemy = enemyCandidate;
    }

    private bool IsBestThanTarget(Enemy targetEnemy, Enemy enemyCandidate)
    {
        // TODO compare more parameters than distance
        return Vector3.Distance(transform.position, targetEnemy.transform.position) >
               Vector3.Distance(transform.position, enemyCandidate.transform.position);
    }

    private bool IsEnemyActiveWithHealth(Enemy enemy)
    {
        return enemy != null && enemy.gameObject.activeInHierarchy && enemy.GetComponent<Health>().currentHealth > 0;
    }

    private void OnDrawGizmos()
    {
        if (_targetEnemy != null)
        {
            Gizmos.DrawSphere(new Vector3(_targetEnemy.transform.position.x, 5, _targetEnemy.transform.position.z), 1);
        }
    }
}