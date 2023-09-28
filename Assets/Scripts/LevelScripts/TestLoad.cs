using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestLoad : MonoBehaviour
{
    public Button loadButton;
    public SceneReference newGameScene;
    void Start()
    {
        Button btn = loadButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        SceneManager.LoadScene(newGameScene);
        SceneManager.LoadScene("AlwaysLoad", LoadSceneMode.Additive);
    }
}
