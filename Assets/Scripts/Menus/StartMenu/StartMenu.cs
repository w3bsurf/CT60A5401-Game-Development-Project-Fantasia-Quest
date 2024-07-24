using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// A class for the StartMenu UI
public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject mapTransition;
    [SerializeField] private StartMenuSelector selector;
    [SerializeField] private AudioSource menuAccept;

    private void Start()
    {
        selector.gameObject.SetActive(true);
        selector.IsActive = true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChooseMenuOption();
        }
    }

    // Function to choose an option in the start menu
    private void ChooseMenuOption()
    {
        switch (selector.Index)
        {
            case 0:
                menuAccept.Play();
                StartGame();
                break;
            case 1:
                menuAccept.Play();
                QuitGame();
                break;
        }
    }

    // Function to start the game from the start menu
    private void StartGame()
    {
        SceneLoader.LoadGameStart();
    }

    // Function to quit the game
    private void QuitGame()
    {
        Application.Quit();
    }


}
