// Copyright (c) 2024 Synty Studios Limited. All rights reserved.
//
// Use of this software is subject to the terms and conditions of the Synty Studios End User Licence Agreement (EULA)
// available at: https://syntystore.com/pages/end-user-licence-agreement
//
// Sample scripts are included only as examples and are not intended as production-ready.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Synty.Interface.SciFiSoldierHUD.Samples
{
    [System.Serializable]
    public class SceneReference
    {
        public string SceneName;
    }

    public class MainMenuManager : MonoBehaviour
    {
        [Header("References")]
        public Animator animator;

        [Header("Parameters")]
        public bool showCursor;
        [SerializeField] private SceneAsset sceneAsset;
        [SerializeField, HideInInspector] private string sceneName;

        private void OnValidate()
        {
            if (sceneAsset != null)
            {
                sceneName = sceneAsset.name;
            }
        }

        private void OnEnable()
        {
            if (animator)
            {
                animator.gameObject.SetActive(true);
                animator.SetBool("Active", false);
            }

            if (showCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void QuitApplication()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }


        public void SwitchScene()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            StartCoroutine(C_SwitchScene());
        }

        private IEnumerator C_SwitchScene()
        {
            if (animator)
            {
                animator.gameObject.SetActive(true);
                animator.SetBool("Active", true);
                yield return new WaitForSeconds(0.5f);
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}
