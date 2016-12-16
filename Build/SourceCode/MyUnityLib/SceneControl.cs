using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour {

    private void OnGUI()
    {
        GUI.Button(new Rect(10, 70, 100, 30), "Press R Restart");
        GUI.Button(new Rect(10, 100, 100, 30), "Press Q Quit");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        }
    }
}
