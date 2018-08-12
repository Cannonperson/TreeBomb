using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject tutorial;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            tutorial.SetActive(false);
        }
    }

    public void OpenTutorial()
    {
        tutorial.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
