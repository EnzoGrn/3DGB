using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitVitalQuest : MonoBehaviour {

    [Header("Quest")]

    [SerializeField]
    private Quest _Quest;

    [Header("Step #1")]

    [SerializeField]
    private QuestUIManager _QuestManagerUI;

    [Header("Step #2")]

    [SerializeField]
    private FuseController _Fuse;

    public bool FusePickedUp = false;

    [Header("Step #3")]

    [SerializeField]
    private DialogueSO _HasFuseDialog;

    [SerializeField]
    private DialogueSO[] _NoFuseDialog;

    public GameObject WaypointPrefab;

    [SerializeField]
    private GameObject _WaypointParent;

    [SerializeField]
    private GameObject _Waypoint;

    private Waypoint _WaypointScript;

    /// <summary>
    /// Initialize the quest
    /// If the QuestManager initalize some quest at the same time, another dialogue can be ended and the quest can be advanced
    /// So that will enter in conflict with the current quest
    /// </summary
    public void Initialize(Quest quest)
    {
        _Quest          = quest;
        _QuestManagerUI = Object.FindFirstObjectByType<QuestUIManager>();

        if (_QuestManagerUI) {
            CircuitVitalQuest[] _CircuitVitalQuests = FindObjectsOfType<CircuitVitalQuest>();

            if (_CircuitVitalQuests.Length == 1) {
                _QuestManagerUI.OnDialogueEnded.AddListener(InitializationDialogueEnded);

                _Fuse = Object.FindAnyObjectByType<FuseController>();

                if (_Fuse) {
                    _Fuse.InitWaypoint();
                    _Fuse.OnFusePickup += OnFusePickup;
                }
            } else if (_CircuitVitalQuests.Length == 2) {
                NpcDialogue dialog = GetComponent<NpcDialogue>();

                if (!dialog) {
                    Debug.LogError("Error in the quest initialization: NpcDialogue not found");

                    return;
                }
                dialog.OnNewDialogue += () => GetDialogueMark();

                _WaypointScript = GetComponent<Waypoint>();

                _WaypointParent = GameObject.FindGameObjectWithTag("WaypointsParent");

                if (_WaypointParent && WaypointPrefab)
                    _Waypoint = Instantiate(WaypointPrefab, _WaypointParent.transform);
                _WaypointScript.SetWaypointUI(_Waypoint);
                _WaypointScript.isDisabled = true;
            }
        } else {
            Debug.LogError("QuestUIManager not found");

            return;
        }
    }

    /// <summary>
    /// Event called when the initialization dialogue ended
    /// It allow the player to advance to the next step
    /// </summary>
    private void InitializationDialogueEnded()
    {
        _QuestManagerUI.OnDialogueEnded.RemoveListener(InitializationDialogueEnded);

        if (_Quest.CurrentStepIndex == 0) {
            _Quest.AdvanceStep();

            _Fuse.Set();
        }
    }

    /// <summary>
    /// Function called when the player pick up the fuse
    /// Allow the player to go to the next step
    /// </summary>
    public void OnFusePickup()
    {
        CircuitVitalQuest[] _CircuitVitalQuests = FindObjectsOfType<CircuitVitalQuest>();

        foreach (var quest in _CircuitVitalQuests)
            quest.FusePickedUp = true;
        _Fuse.Delete();

        Destroy(_Fuse.gameObject);

        _Fuse = null;
    }

    private void FixedUpdate()
    {
        // If the player picked up the fuse, advance to the next step
        if (_Quest.CurrentStepIndex == 1 && FusePickedUp) {
            _Quest.AdvanceStep();

            return;
        }
        
        if (_Quest.CurrentStepIndex == 2 && _WaypointScript) {
            _WaypointScript.isDisabled = false;
        } else if (_WaypointScript) {
            _WaypointScript.isDisabled = true;
        }
    }

    private DialogueSO GetDialogueMark()
    {
        if (FusePickedUp) {
            if (_Quest.CurrentStepIndex == 2)
                DialogueManager.Instance.OnDialogueEnded += AfterTalkToMark;
            _WaypointScript.isDisabled = false;
            _WaypointScript.DestroyWaypoint();

            return _HasFuseDialog;
        } else {
            return _NoFuseDialog[Random.Range(0, _NoFuseDialog.Length)];
        }
    }

    private void AfterTalkToMark()
    {
        _Quest.AdvanceStep();

        DialogueManager.Instance.OnDialogueEnded -= AfterTalkToMark;
    }
}
