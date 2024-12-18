using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CircuitVitalQuest : MonoBehaviour
{

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
    private DialogueSO[] _AlreadyRepearFuse;

    [SerializeField]
    private DialogueSO[] _NoFuseDialog;

    [Header("Step #4")]
    [SerializeField]
    private MachineRoomManager _MachineRoom;

    [SerializeField]
    private ARoom[] _Rooms;

    [Header("Step #5")]

    [SerializeField]
    private bool _CanInteract = false;

    [SerializeField]
    private GameObject _InteractTxt;

    [SerializeField]
    private Camera _CameraCinematic;

    public GameObject WaypointPrefab;

    [SerializeField]
    private GameObject _WaypointParent;

    [SerializeField]
    public GameObject _Waypoint;

    private Waypoint _WaypointScript;

    [Header("Audio")]

    [SerializeField]
    private AudioClip _Clip;

    /// <summary>
    /// Initialize the quest
    /// If the QuestManager initalize some quest at the same time, another dialogue can be ended and the quest can be advanced
    /// So that will enter in conflict with the current quest
    /// </summary
    public void Initialize(Quest quest)
    {
        _Quest = quest;
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
                //NPCBehavior npc = GetComponent<NPCBehavior>();

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

                // -- Bind the event OnRoom for trigger open the door --
                _Rooms = FindObjectsOfType<ARoom>();

                _MachineRoom = Object.FindFirstObjectByType<MachineRoomManager>();

                if (_Rooms.Length == 0 || !_MachineRoom) {
                    Debug.LogError("Error in the quest initialization: MachineRoomManager not found");

                    return;
                }

                for (int i = 0; i < _Rooms.Length; i++)
                    _Rooms[i].ElectricityOff();
            }

            // -- Script of the fifth step (step 4) --
            if (CircuitNumber == 5) {
                _WaypointScript = GetComponent<Waypoint>();

                _WaypointParent = GameObject.FindGameObjectWithTag("WaypointsParent");

                if (_WaypointParent && WaypointPrefab)
                    _Waypoint = Instantiate(WaypointPrefab, _WaypointParent.transform);
                _WaypointScript.SetWaypointUI(_Waypoint);
                _WaypointScript.isDisabled = true;
            }

            // -- Script of the sixth step (step 5) --
            if (CircuitNumber == 6) {
                _WaypointScript = GetComponent<Waypoint>();

                _WaypointParent = GameObject.FindGameObjectWithTag("WaypointsParent");

                if (_WaypointParent && WaypointPrefab)
                    _Waypoint = Instantiate(WaypointPrefab, _WaypointParent.transform);
                _WaypointScript.SetWaypointUI(_Waypoint);
                _WaypointScript.isDisabled = true;

                // -- Bind the event OnRoom for trigger open the door --
                _Rooms = FindObjectsOfType<ARoom>();
                _MachineRoom = Object.FindFirstObjectByType<MachineRoomManager>();

                if (_Rooms.Length == 0 || !_MachineRoom) {
                    Debug.LogError("Error in the quest initialization: MachineRoomManager not found");

                    return;
                }

                QuestManager.Instance.OnQuestRemoved += OnQuestFinished;
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

        // -- Step 5, waypoint --
        if (CircuitNumber == 5 && _Quest.CurrentStepIndex == 4 && _WaypointScript) {
            _WaypointScript.isDisabled = false;
        } else if (CircuitNumber == 5 && _WaypointScript) {
            _WaypointScript.isDisabled = true;
        }

        // -- Step 6, waypoint --
        if (CircuitNumber == 6 && _Quest.CurrentStepIndex == 5 && _WaypointScript) {
            _WaypointScript.isDisabled = false;
        } else if (CircuitNumber == 6 && _WaypointScript) {
            _WaypointScript.isDisabled = true;
        }
    }

    private void Update()
    {
        // -- Show text to interact --
        if (_CanInteract) {
            if (Input.GetKeyDown(KeyCode.E)) {
                if (CircuitNumber == 5) {
                    AudioManager.Instance.PlaySFX(_Clip, transform);

                    _Quest.AdvanceStep();

                    CanInteract(false);
                } else if (CircuitNumber == 6) {
                    ReactiveMachineRoom();
                }
            }
        }
    }

    private DialogueSO GetDialogueMark()
    {
        if (FusePickedUp) {
            if (_Quest.CurrentStepIndex > 2)
                return _AlreadyRepearFuse[Random.Range(0, _AlreadyRepearFuse.Length)];
            else if (_Quest.CurrentStepIndex == 2)
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
                InMachineRoom();
        } else if ((CircuitNumber == 5 && _Quest.CurrentStepIndex == 4) || (CircuitNumber == 6 && _Quest.CurrentStepIndex == 5)) {
            if (other.CompareTag("Player"))
                CanInteract(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _InteractTxt)
            CanInteract(false);
    }

    private void CanInteract(bool canInteract)
    {
        _CanInteract = canInteract;

        if (_CanInteract) {
            _InteractTxt.SetActive(true);
        } else {
            _InteractTxt.SetActive(false);
        }
    }

    private void InMachineRoom()
    {
        _Quest.AdvanceStep();

        _MachineRoom.OpenDoor();
    }

    private void ReactiveMachineRoom()
    {
        AudioManager.Instance.PlaySFX(_Clip, transform);

        for (int i = 0; i < _Rooms.Length; i++)
            _Rooms[i].ElectricityOn();

        if (_InteractTxt)
            _InteractTxt.SetActive(false);

        _Quest.AdvanceStep();

        StartCoroutine(Cinematic());
    }

    private IEnumerator Cinematic()
    {
        GameObject hud = GameObject.FindGameObjectWithTag("HUD");

        if (!hud) {
            Debug.LogError("HUD not found");

            yield return null;
        } else {
            hud.SetActive(false);

            Camera currentCamera = Camera.main;

            _CameraCinematic.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);

            currentCamera.gameObject.SetActive(true);

            hud.SetActive(true);
        }
    }

    private void OnQuestFinished(Quest quest)
    {
        QuestManager.Instance.OnQuestRemoved -= OnQuestFinished;

        CircuitVitalQuest[] circuitVitalQuest = FindObjectsOfType<CircuitVitalQuest>();

        CircuitVitalQuest cinematic = null;

        foreach (var steps in circuitVitalQuest) {
            if (steps.CircuitNumber == 6) {
                cinematic = steps;

                continue;
            }

            if (steps._Waypoint)
                Destroy(steps._Waypoint);
            if (steps.CircuitNumber == 3)
                Destroy(steps);
            else
                Destroy(steps.gameObject);
        }

        if (cinematic)
            StartCoroutine(Wait(cinematic));
    }

    private IEnumerator Wait(CircuitVitalQuest cinematic)
    {
        yield return new WaitForSeconds(3f);

        Destroy(cinematic.gameObject);
    }
}
