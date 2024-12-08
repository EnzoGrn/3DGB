using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _Target;
    [SerializeField] private MovementType _MovementType;

    private NavMeshAgent _Agent;
    private int _CurrentTarget = 0;
    private bool _IsWaiting = false;

    // PingPong
    private bool _ReversedMovement = false;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _Target.Length; i++)
        {
            if (i < _Target.Length - 1)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_Target[i].position, _Target[i + 1].position);
            }
            else if (_MovementType == MovementType.Loop)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_Target[i].position, _Target[0].position);
            }

            // Add a sphere to the waypoints
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_Target[i].position, 0.5f);
        }
    }

    private void Start()
    {
        _Agent = GetComponent<NavMeshAgent>();
        _Agent.SetDestination(_Target[_CurrentTarget].position);
    }

    private void Update()
    {
        if (_Agent.remainingDistance < 0.1f && !_IsWaiting)
        {
            StartCoroutine(NextTarget());
        }
    }

    private void PingPongNextTarget()
    {
        if (!_ReversedMovement)
        {
            if (_CurrentTarget + 1 > _Target.Length - 1)
            {
                _ReversedMovement = true;
            }
            else
            {
                _CurrentTarget++;
            }
        }
        else
        {
            if (_CurrentTarget - 1 < 0)
            {
                _ReversedMovement = false;
            }
            else
            {
                _CurrentTarget--;
            }
        }
    }

    private void LoopNextTarget()
    {
        if (_CurrentTarget + 1 > _Target.Length - 1)
        {
            _CurrentTarget = 0;
        }
        else
        {
            _CurrentTarget++;
        }
    }

    private IEnumerator NextTarget()
    {
        _IsWaiting = true;

        if (_MovementType == MovementType.Loop)
        {
            LoopNextTarget();
        }
        else if (_MovementType == MovementType.PingPong)
        {
            PingPongNextTarget();
        }

        yield return new WaitForSeconds(4f);
        _Agent.SetDestination(_Target[_CurrentTarget].position);
        _IsWaiting = false;
    }
}

[System.Serializable]
public enum MovementType
{
    Loop,
    PingPong
}
