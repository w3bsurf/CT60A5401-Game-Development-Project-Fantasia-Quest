using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for the scrolling credits scene
public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co_LoadStartMenuAfterCredits());
    }

    // Loads the start menu after the end credits, waits until credits are over
    private IEnumerator Co_LoadStartMenuAfterCredits()
    {
        yield return new WaitForSeconds(32f);
        SceneLoader.LoadStartMenu();
    }
}
