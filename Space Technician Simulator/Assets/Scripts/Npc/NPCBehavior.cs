using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.AI;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[System.Serializable]
public class PointData
{
    public Transform point;
    public string animationName;
}

public class NPCBehavior : MonoBehaviour
{
    public enum MovementType
    {
        Loop,
        PingPong
    }

    #region MOVEMENT SETTINGS
    public bool canMove;
    [SerializeField, HideInInspector] private List<PointData> movementPoints = null;
    [SerializeField, HideInInspector] public MovementType movementType = MovementType.Loop;
    public bool moveActive = true;

    private int currentPointIndex = 0;
    private bool isReversing = false; // For ping-pong behavior
    private Animator _animator;
    private bool isWaiting = false; // For handling wait time

    public bool showFootstepClips = false;
    public bool showMovementPoints = false;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    public float waitTime = 1f; // Time to wait at each point
    public float moveSpeed = 3f;
    private NavMeshAgent _Agent;

    #endregion

    #region DIALOGUE SETTINGS
    public bool hasDialogue;
    [SerializeField, HideInInspector] private DialogueSO currentDialogue;
    public TMP_Text interactionText;
    public TMP_Text npcNameText;
    public bool lookAtPlayer;
    public int actorIndex;
    public ActorSO actorsInfo;
    public Camera dialogueCamera;

    public bool useDynamicDialogue;
    public DialogueSO defaultDialogue;

    private bool isDialogueActive = false;
    private bool isPlayerDetected = false;

    public Func<DialogueSO> onGenerateNewDialogue;
    #endregion

    void Start()
    {
        _animator = GetComponent<Animator>();
        _Agent = GetComponent<NavMeshAgent>();

        if (_Agent != null)
        {
            _Agent.speed = moveSpeed;
            _Agent.stoppingDistance = 0.1f;
            _Agent.autoBraking = false;
            _Agent.angularSpeed = 840f;
        }

        if (canMove && movementPoints.Count > 0)
        {
            _Agent.SetDestination(movementPoints[currentPointIndex].point.position);
        }

        interactionText.gameObject.SetActive(false);
        interactionText.text = "Press 'Enter' to talk";

        RegisterEvent();
    }


    private void RegisterEvent()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnd;
            DialogueManager.Instance.OnNewDialogue += PlayAnimation;
        }
        else
        {
            Debug.LogError("DialogueManager.Instance is null. Ensure DialogueManager is in the scene and initialized.");
        }
    }

    private void UnregisterEvent()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnd;
        }
    }

    void Update()
    {
        SwitchAgent();

        UpdateAnimatorSpeed();

        if (canMove && moveActive && movementPoints.Count > 1 && !isWaiting)
        {
            MoveBetweenPoints();
        }
        if (isPlayerDetected && !isDialogueActive && currentDialogue.DialogueType ==  DialogueType.Manual)
        {
            interactionText.gameObject.SetActive(true);
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void SwitchAgent()
    {
        if (!moveActive && !_Agent.isStopped)
        {
            // Pause the agent
            _Agent.isStopped = true;
        }
        else if (moveActive && _Agent.isStopped)
        {
            _Agent.isStopped = false;
        }
    }

    private void MoveBetweenPoints()
    {
        Transform targetPoint = movementPoints[currentPointIndex].point;
        if (targetPoint == null) return;

        if (_Agent.remainingDistance <= _Agent.stoppingDistance)
        {
            _animator.SetFloat("Speed", 0);
            PlayAnimationForPoint();
            StartCoroutine(WaitAtPoint(waitTime));
        }
    }

    private void UpdateAnimatorSpeed()
    {
        if (_Agent == null || _animator == null) return;

        float currentSpeed = _Agent.velocity.magnitude;

        if (currentSpeed < 0.1f)
        {
            currentSpeed = 0;
        }

        _animator.SetFloat("Speed", currentSpeed);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepAudioVolume);
            }
        }
    }

    private void PlayAnimationForPoint()
    {
        string animationName = movementPoints[currentPointIndex].animationName;
        if (!string.IsNullOrEmpty(animationName) && _animator != null)
        {
            _animator.Play(animationName);
        }
    }

    private IEnumerator WaitAtPoint(float time)
    {
        isWaiting = true;
        yield return new WaitForSeconds(time);
        AdvanceToNextPoint();
        isWaiting = false;
    }

    private void AdvanceToNextPoint()
    {
        if (movementPoints.Count == 0) return;

        // Gérer l'ordre des points en fonction du type de mouvement
        if (movementType == MovementType.Loop)
        {
            currentPointIndex = (currentPointIndex + 1) % movementPoints.Count;
        }
        else if (movementType == MovementType.PingPong)
        {
            if (!isReversing)
            {
                currentPointIndex++;
                if (currentPointIndex >= movementPoints.Count - 1)
                {
                    isReversing = true;
                }
            }
            else
            {
                currentPointIndex--;
                if (currentPointIndex <= 0)
                {
                    isReversing = false;
                }
            }
        }

        // Définir la nouvelle destination
        _Agent.SetDestination(movementPoints[currentPointIndex].point.position);
    }

    private void LookAtPlayer()
    {
        Debug.Log("Looking at player?");
        if (_Agent != null) _Agent.isStopped = true;

        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null) return;

        Debug.Log("Looking at player.");

        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    public void StartDialogue()
    {
        if (isDialogueActive)
            return;

        isDialogueActive = true;

        Debug.Log("Dialogue has started.");

        if (useDynamicDialogue)
        {
            DialogueSO newDialogue = onGenerateNewDialogue?.Invoke();

            currentDialogue = newDialogue != null ? newDialogue : defaultDialogue;
        }
        if (currentDialogue == null)
            return;

        moveActive = false;

        if (currentDialogue.Messages[0].animationName != "")
        {
            _animator.Play(currentDialogue.Messages[0].animationName);
        }

        DialogueManager.Instance.StartDialogue(currentDialogue);
    }

    public void PlayAnimation(string animationName)
    {
        if (animationName == "" || _animator == null) return;
        Debug.Log("Playing animation: " + animationName);
        if (_animator != null)
        {
            _animator.Play(animationName);
        }
    }

    private void HandleDialogueEnd()
    {
        Debug.Log("Dialogue has ended.");

        isDialogueActive = false;

        if (currentDialogue == null)
            return;

        if (currentDialogue.DialogueType == DialogueType.Manual)
        {
            DialogueTrigger dialogueTrigger = GameObject.Find("DialogueTrigger")?.GetComponent<DialogueTrigger>();

            if (dialogueTrigger == null)
            {
                Debug.LogWarning("DialogueTrigger or its script not found.");
                return;
            }

            dialogueTrigger.EndDialogue();
            StartCoroutine(WaitAtPoint(1.0f));
            moveActive = true;
        }
    }



    public void NextMessageDialogue()
    {
        if (!isDialogueActive)
            return;

        DialogueManager.Instance.NextMessage(currentDialogue.DialogueType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasDialogue && currentDialogue != null)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerDetected = true;

                if (!isDialogueActive && currentDialogue.DialogueType == DialogueType.Auto)
                {
                    isDialogueActive = true;
                    DialogueManager.Instance.StartDialogue(currentDialogue);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isDialogueActive && currentDialogue.DialogueType == DialogueType.Manual && lookAtPlayer)
        {
            LookAtPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasDialogue && currentDialogue != null)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerDetected = false;

                if (isDialogueActive && currentDialogue.DialogueType == DialogueType.Auto)
                {
                    isDialogueActive = false;
                    DialogueManager.Instance.EndDialogue(currentDialogue.DialogueType);
                }
            }
        }
    }

    private void OnDestroy()
    {
        UnregisterEvent();
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(NPCBehavior))]
    public class NPCBehaviorEditor : Editor
    {
        private ReorderableList movementPointsList;
        private ReorderableList footstepAudioClipsList;

        private void OnEnable()
        {
            SerializedProperty movementPointsProp = serializedObject.FindProperty("movementPoints");
            movementPointsList = new ReorderableList(serializedObject, movementPointsProp, true, true, true, true)
            {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    SerializedProperty element = movementPointsProp.GetArrayElementAtIndex(index);
                    SerializedProperty pointProp = element.FindPropertyRelative("point");
                    SerializedProperty animationNameProp = element.FindPropertyRelative("animationName");

                    float fieldWidth = rect.width / 2 - 10;

                    rect.y += 2;
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight),
                        pointProp,
                        GUIContent.none
                    );
                    EditorGUI.PropertyField(
                        new Rect(rect.x + fieldWidth + 10, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight),
                        animationNameProp,
                        GUIContent.none
                    );
                },
                elementHeight = EditorGUIUtility.singleLineHeight + 4,
                onAddCallback = (ReorderableList list) =>
                {
                    movementPointsProp.arraySize++;
                    serializedObject.ApplyModifiedProperties();
                },
                onRemoveCallback = (ReorderableList list) =>
                {
                    movementPointsProp.DeleteArrayElementAtIndex(list.index);
                    serializedObject.ApplyModifiedProperties();
                },
                headerHeight = 0f
            };

            SerializedProperty footstepAudioClipsProp = serializedObject.FindProperty("FootstepAudioClips");
            footstepAudioClipsList = new ReorderableList(serializedObject, footstepAudioClipsProp, true, true, true, true)
            {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    SerializedProperty element = footstepAudioClipsProp.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y + 2.5f, rect.width, EditorGUIUtility.singleLineHeight),
                        element,
                        GUIContent.none
                    );
                },
                elementHeight = EditorGUIUtility.singleLineHeight + 4,
                onAddCallback = (ReorderableList list) =>
                {
                    footstepAudioClipsProp.arraySize++;
                    serializedObject.ApplyModifiedProperties();
                },
                onRemoveCallback = (ReorderableList list) =>
                {
                    footstepAudioClipsProp.DeleteArrayElementAtIndex(list.index);
                    serializedObject.ApplyModifiedProperties();
                },
                headerHeight = 0f
            };
        }

        public override void OnInspectorGUI()
        {
            NPCBehavior npc = (NPCBehavior)target;

            serializedObject.Update();

            #region MOVEMENT SETTINGS
            npc.canMove = EditorGUILayout.Toggle(new GUIContent("Can Move", "Enable or disable the NPC's movement."), npc.canMove);
            if (npc.canMove)
            {
                GUILayout.Space(5);

                EditorGUILayout.BeginHorizontal();

                npc.showMovementPoints = EditorGUILayout.Foldout(
                    npc.showMovementPoints,
                    new GUIContent("Movement Points", "Define the points the NPC will move between."),
                    true
                );

                GUILayout.FlexibleSpace();
                EditorGUILayout.IntField(movementPointsList.count, GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();

                if (npc.showMovementPoints)
                {
                    GUILayout.Space(10);
                    movementPointsList.DoLayoutList();
                }

                GUILayout.Space(5);
                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty("movementType"),
                    new GUIContent("Movement Type", "Choose whether the NPC loops or ping-pongs between points.")
                );
                GUILayout.Space(5);
                npc.waitTime = EditorGUILayout.FloatField(
                    new GUIContent("Wait Time at Points", "The duration the NPC waits at each movement point."),
                    npc.waitTime
                );
                GUILayout.Space(2.5f);
                npc.moveSpeed = EditorGUILayout.FloatField(
                    new GUIContent("Move Speed", "The speed at which the NPC moves between points."),
                    npc.moveSpeed
                );
                GUILayout.Space(2.5f);
                npc.moveActive = EditorGUILayout.Toggle(
                    new GUIContent("Move Active", "Enable or disable the NPC's movement."),
                    npc.moveActive
                );
                GUILayout.Space(5);

                EditorGUILayout.BeginHorizontal();

                npc.showFootstepClips = EditorGUILayout.Foldout(
                    npc.showFootstepClips,
                    new GUIContent("Footstep Audio Clips", "The audio clips to play when the NPC moves."),
                    true
                );

                GUILayout.FlexibleSpace();
                EditorGUILayout.IntField(footstepAudioClipsList.count, GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();
                if (npc.showFootstepClips)
                {
                    GUILayout.Space(10);
                    footstepAudioClipsList.DoLayoutList();
                }
                GUILayout.Space(5);

                npc.FootstepAudioVolume = EditorGUILayout.Slider(
                    new GUIContent("Footstep Audio Volume", "The volume of the NPC's footstep audio."),
                    npc.FootstepAudioVolume, 0f, 1f);
            }
            #endregion

            GUILayout.Space(10);

            #region DIALOGUE SETTINGS
            if (npc.hasDialogue && npc.currentDialogue == null)
            {
                EditorGUILayout.HelpBox("Dialogue is enabled, but no Dialogue Asset is assigned!", MessageType.Error);
                GUILayout.Space(5);
            }
            // Dialogue Settings
            npc.hasDialogue = EditorGUILayout.Toggle(
                new GUIContent("Has Dialogue", "Enable or disable the NPC's dialogue."),
                npc.hasDialogue
            );
            if (npc.hasDialogue)
            {
                GUILayout.Space(5);
                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty("currentDialogue"),
                    new GUIContent("Dialogue", "The dialogue to display when the player interacts with the NPC."),
                    true
                );

                GUILayout.Space(5);
                npc.interactionText = (TMP_Text)EditorGUILayout.ObjectField(
                    new GUIContent("Interaction Text", "The text to display when the player can interact with the NPC."),
                    npc.interactionText,
                    typeof(TMP_Text),
                    true
                );

                GUILayout.Space(5);
                npc.npcNameText = (TMP_Text)EditorGUILayout.ObjectField(
                    new GUIContent("NPC Name Text", "The text to display the NPC's name."),
                    npc.npcNameText,
                    typeof(TMP_Text),
                    true
                );

                GUILayout.Space(5);
                npc.lookAtPlayer = EditorGUILayout.Toggle(
                    new GUIContent("Look at Player", "Enable or disable the NPC looking at the player."),
                    npc.lookAtPlayer
                );

                GUILayout.Space(5);
                npc.actorsInfo = (ActorSO)EditorGUILayout.ObjectField(
                    new GUIContent("Actors Info", "The ActorSO containing the NPC's actor information."),
                    npc.actorsInfo,
                    typeof(ActorSO),
                    true
                );

                if (npc.actorsInfo != null
                    && npc.actorsInfo.Actors != null
                    && npc.actorIndex >= 0
                    && npc.actorIndex < npc.actorsInfo.Actors.Length
                    && npc.actorsInfo.Actors[npc.actorIndex] != null)
                {
                        npc.npcNameText.text = npc.actorsInfo.Actors[npc.actorIndex].actorName;
                }

                GUILayout.Space(5);
                npc.actorIndex = EditorGUILayout.IntField(
                    new GUIContent("Actor Index", "The index of the actor in the ActorSO."),
                    npc.actorIndex
                );

                if (npc.actorIndex < 0)
                {
                    npc.actorIndex = 0;
                }

                GUILayout.Space(5);
                npc.dialogueCamera = (Camera)EditorGUILayout.ObjectField(
                    new GUIContent("Dialogue Camera", "The camera to use for dialogue."),
                    npc.dialogueCamera,
                    typeof(Camera),
                    true
                );

                GUILayout.Space(5);
                npc.useDynamicDialogue = EditorGUILayout.Toggle(
                    new GUIContent("Use Dynamic Dialogue", "Enable or disable dynamic dialogue generation."),
                    npc.useDynamicDialogue
                );

                if (npc.useDynamicDialogue)
                {
                    GUILayout.Space(5);
                    EditorGUILayout.PropertyField(
                        serializedObject.FindProperty("defaultDialogue"),
                        new GUIContent("Default Dialogue", "The default dialogue to use if dynamic dialogue generation fails."),
                        true
                    );
                }
            }
            #endregion

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
