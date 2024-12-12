using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitVitalQuest : MonoBehaviour {

    [Header("Circuit Vital Quest")]

    /// <summary>
    /// The nième number of the circuit vital quest generated
    /// Can be the reference to the circuit script link to a step
    /// So if the number it's 1, the step is 0.
    /// </summary>
    public int CircuitNumber = 0;

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

            // -- Script of the first step (step 0) --
            if (CircuitNumber == 1) {
                _QuestManagerUI.OnDialogueEnded.AddListener(InitializationDialogueEnded);

                _Fuse = Object.FindAnyObjectByType<FuseController>();

                if (_Fuse) {
                    _Fuse.InitWaypoint();
                    _Fuse.OnFusePickup += OnFusePickup;
                }
            }

            // -- Script of the third step (step 2) --
            if (CircuitNumber == 3) {
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

            // -- Script of the fourth step (step 3) --
            if (CircuitNumber == 4) {
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

            if (!FusePickedUp)
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

        // -- Step 3, waypoint --
        if (CircuitNumber == 3 && _Quest.CurrentStepIndex == 2 && _WaypointScript) {
            _WaypointScript.isDisabled = false;
        } else if (CircuitNumber == 3 && _WaypointScript) {
            _WaypointScript.isDisabled = true;
        }

        // -- Step 4, waypoint --
        if (CircuitNumber == 4 && _Quest.CurrentStepIndex == 3 && _WaypointScript) {
            _WaypointScript.isDisabled = false;
        } else if (CircuitNumber == 4 && _WaypointScript) {
            _WaypointScript.isDisabled = true;
        }

        // If the quest is completed, destroy the quest and everything related to it
        if (_Quest.CurrentStepIndex == _Quest.QuestInfo.questSteps.Count) {

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

    private void OnTriggerEnter(Collider other)
    {
        if (CircuitNumber == 4 && _Quest.CurrentStepIndex == 3) {
            if (other.CompareTag("Player"))
                _Quest.AdvanceStep();
        }
    }
}
