using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] public bool _CanMove = true;
    [SerializeField] private Transform[] _Target;
    [SerializeField] private MovementType _MovementType;

    // Fall timeout variables
    private float _fallTimeoutDelta;

    // Agent stuff
    private NavMeshAgent _Agent;
    private int _CurrentTarget = 0;
    private bool _IsWaiting = false;

    // PingPong
    private bool _ReversedMovement = false;

    // Animation IDs
    private Animator _Animator;
    private bool _HasAnimator;
    private int _AnimIDSpeed;
    private int _AnimIDGrounded;
    private int _AnimIDJump;
    private int _AnimIDFreeFall;
    private int _AnimIDMotionSpeed;

    // Audio
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    private GameObject _Player;

    private void Awake()
    {
        // Find the player
        _Player = GameObject.FindGameObjectWithTag("Player");
    }

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
        _HasAnimator = TryGetComponent(out _Animator);
        _Agent = GetComponent<NavMeshAgent>();
        _Agent.SetDestination(_Target[_CurrentTarget].position);

        AssignAnimationIDs();
    }

    private void Update()
    {
        _HasAnimator = TryGetComponent(out _Animator);

        HandleAnimation();

        SwitchAgent();

        if (!_CanMove)
        {
            LookAtPlayer();
        }


        if (_Agent.remainingDistance < 0.1f && !_IsWaiting)
        {
            StartCoroutine(NextTarget());
        }
    }

    public void LookAtPlayer()
    {
        // Rotate the NPC to face the player
        Vector3 direction = _Player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

    }

    private void SwitchAgent()
    {
        if (!_CanMove && !_Agent.isStopped)
        {
            // Pause the agent
            _Agent.isStopped = true;
        }
        else if (_CanMove && _Agent.isStopped)
        {
            _Agent.isStopped = false;
        }
    }

    private void HandleAnimation()
    {
        if (_HasAnimator)
        {
            // Debug.Log("Magnitude Velocity -> " + _Agent.velocity.magnitude);
            _Animator.SetFloat(_AnimIDSpeed, _Agent.velocity.magnitude);
            // Motion speed
            _Animator.SetFloat(_AnimIDMotionSpeed, _Agent.velocity.magnitude / _Agent.speed);
        }
    }

    private void AssignAnimationIDs()
    {
        _AnimIDSpeed = Animator.StringToHash("Speed");
        _AnimIDGrounded = Animator.StringToHash("Grounded");
        _AnimIDJump = Animator.StringToHash("Jump");
        _AnimIDFreeFall = Animator.StringToHash("FreeFall");
        _AnimIDMotionSpeed = Animator.StringToHash("MotionSpeed");
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

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(transform.position), FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(transform.position), FootstepAudioVolume);
        }
    }
}

[System.Serializable]
public enum MovementType
{
    Loop,
    PingPong
}
