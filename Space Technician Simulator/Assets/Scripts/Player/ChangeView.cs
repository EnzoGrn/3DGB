using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ChangeView : MonoBehaviour
{
    [Header("1st Person View")]
    [Tooltip("A game parent object to use for 1st Person View. It contains the main Camera and the PlayerFollowCamera")]
    public GameObject FirstPersonView;

    [Tooltip("The 1st Person Controller Script for controlling the 1st Person Camera")]
    public FirstPersonController firstPersonController;

    [Header("3rd Person View")]
    [Tooltip("A game parent object to use for 3rd Person View. It contains the main Camera and the PlayerFollowCamera")]
    public GameObject ThirdPersonView;

    [Tooltip("The 3rd Person Controller Script for controlling the 3rd Person Camera")]
    public ThirdPersonController thirdPersonController;

    private StarterAssetsInputs _input;

    public event System.Action OnCameraChanged;

    // Start is called before the first frame update
    void Start()
    {
        // set the first person view to active and the third person view to inactive
        FirstPersonView.SetActive(false);
        ThirdPersonView.SetActive(true);
        firstPersonController.enabled = false;
        thirdPersonController.enabled = true;

        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        // if keypressed is F, print
        if (_input.view)
        {
            // if the third person view is active, switch to first person view
            if (ThirdPersonView.activeSelf)
            {
                ThirdPersonView.SetActive(false);
                FirstPersonView.SetActive(true);
                firstPersonController.enabled = true;
                thirdPersonController.enabled = false;
            }
            else {
                // if the first person view is active, switch to third person view
                FirstPersonView.SetActive(false);
                ThirdPersonView.SetActive(true);
                firstPersonController.enabled = false;
                thirdPersonController.enabled = true;
            }
            // invoke the OnCameraChanged event
            OnCameraChanged?.Invoke();
            _input.view = false;
        }
    }
}
