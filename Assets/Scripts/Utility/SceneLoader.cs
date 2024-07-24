using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified with added comments and additional methods and variables such as load game start and load start menu, load credits, checks for boss battle.
 */

// Class for loading different scenes such as the battle scene and restoring to the previous scene.
public static class SceneLoader
{
    private static int savedSceneBuildIndex;
    private static Vector2 savedPlayerLocation;
    private static Vector2 beforeBossLocation = new Vector2(32, -4.71f);

    private const int startMenuBuildIndex = 0;
    private const int startSceneBuildIndex = 1;
    private const int endCreditsBuildIndex = 6;
    private const string creditsTransition = "CreditsTransition";

    // Loads the game start scene after selecting start the game from start menu
    public static void LoadGameStart()
    {
        SceneManager.LoadScene(startSceneBuildIndex);
    }

    // Loads the start menu
    public static void LoadStartMenu()
    {
        if (Game.Manager != null)
        {
            GameObject.Destroy(Game.Manager.Player.gameObject);
            GameObject.Destroy(Game.Manager.Map.gameObject);
            GameObject.Destroy(Game.Manager.gameObject);
        }

        SceneManager.sceneLoaded -= DisablePlayerObject;
        SceneManager.sceneLoaded -= RestoreMapAndPlayer;
        SceneManager.LoadScene(startMenuBuildIndex);
    }

    public static void LoadStartMenuFromBattle()
    {
        if (Game.Manager != null)
        {
            Game.Manager.Player.gameObject.SetActive(true);
            GameObject.Destroy(Game.Manager.Player.gameObject);
            GameObject.Destroy(Game.Manager.Map.gameObject);
            GameObject.Destroy(Game.Manager.gameObject);
        }

        SceneManager.sceneLoaded -= DisablePlayerObject;
        SceneManager.sceneLoaded -= RestoreMapAndPlayer;
        SceneManager.LoadScene(startMenuBuildIndex);
    }
    
    // Loads battle scene and saves current player location while making the current player character inactive to stop movement
    public static void LoadBattleScene(int battleSceneBuildIndex, bool bossBattle)
    {
        if (!bossBattle)
        {
            GameObject.DontDestroyOnLoad(Game.Manager.Map);
        } else
        {
            GameObject.Destroy(Game.Manager.Map.gameObject);
            if (Game.Manager.EndMap == null) Game.Manager.EndMap = GameObject.Instantiate(Game.Manager.EndingMap);
            Game.Manager.EndMap.gameObject.SetActive(false);
            Game.Manager.Map = GameObject.Instantiate(Game.Manager.BossMap);
            GameObject.DontDestroyOnLoad(Game.Manager.Map);
            GameObject.DontDestroyOnLoad(Game.Manager.EndMap);
        }
        savedSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        savedPlayerLocation = Game.Manager.Player.CurrentCell.Center2D();
        Game.Manager.Player.gameObject.SetActive(false);
        Game.Manager.Map.gameObject.SetActive(false);
        SceneManager.LoadScene(battleSceneBuildIndex);
    }

    // Loads the end credits scene
    public static void LoadCreditsScene()
    {
        if (Game.Manager != null)
        {
            Game.Manager.Player.gameObject.SetActive(false);
            GameObject.Destroy(Game.Manager.Player.gameObject);
            GameObject.Destroy(Game.Manager.Map.gameObject);
            GameObject.Destroy(Game.Manager.gameObject);
        }

        SceneManager.sceneLoaded -= DisablePlayerObject;
        SceneManager.sceneLoaded -= RestoreMapAndPlayer;
        SceneManager.LoadScene(endCreditsBuildIndex);
    }

    // Coroutine to start the credits scene
    public static IEnumerator Co_StartCredits()
    {
        GameObject transition = GameObject.Instantiate(Resources.Load<GameObject>(creditsTransition), Game.Manager.Player.transform.position, Quaternion.identity);
        Animator animator = transition.GetComponent<Animator>();
        while (animator.IsAnimating())
        {
            yield return null;
        }
        LoadCreditsScene();
    }

    // Reloads the previous scene after a battle
    public static void ReloadSavedSceneAfterBattle(bool bossBattle)
    {
        if (!bossBattle)
        {
            SceneManager.sceneLoaded += RestoreMapAndPlayer;
        } else
        {
            SceneManager.sceneLoaded += RestoreMapAndPlayerAtTheEnd;
        }
        if (savedSceneBuildIndex == 1)
        {
            savedSceneBuildIndex++;
        }
        SceneManager.LoadScene(savedSceneBuildIndex);
    }

    // Reloads the previous scene after a defeat in boss battle
    public static void ReloadSavedSceneAfterBossBattle()
    {
        SceneManager.sceneLoaded += RestoreMapAndPlayerAfterBossBattle;
        if (savedSceneBuildIndex == 1)
        {
            savedSceneBuildIndex++;
        }
        SceneManager.LoadScene(savedSceneBuildIndex);
    }

    // Function to restore both the map and the player after a battle scene
    public static void RestoreMapAndPlayer(Scene scene, LoadSceneMode mode)
    {
        Game.Manager.Map.gameObject.SetActive(true);
        Game.Manager.Player.transform.position = savedPlayerLocation;
        Game.Manager.Player.gameObject.SetActive(true);
        SceneManager.sceneLoaded -= RestoreMapAndPlayer;
    }

    // Function to restore both the map and the player after winning a boss battle
    public static void RestoreMapAndPlayerAtTheEnd(Scene scene, LoadSceneMode mode)
    {
        Game.Manager.Map = Game.Manager.EndMap;
        Game.Manager.Map.gameObject.SetActive(true);
        Game.Manager.Player.transform.position = savedPlayerLocation;
        Game.Manager.Player.gameObject.SetActive(true);
        SceneManager.sceneLoaded -= RestoreMapAndPlayerAtTheEnd;
    }

    // Function to restore both the map and the player after a battle scene
    public static void RestoreMapAndPlayerAfterBossBattle(Scene scene, LoadSceneMode mode)
    {
        Game.Manager.Map.gameObject.SetActive(true);
        Game.Manager.Player.transform.position = beforeBossLocation;
        Game.Manager.Player.gameObject.SetActive(true);
        SceneManager.sceneLoaded -= RestoreMapAndPlayerAfterBossBattle;
    }

    // Function to disable the player object during when loading a scene such as a battle scene
    private static void DisablePlayerObject(Scene scene, LoadSceneMode mode)
    {
        Game.Manager.Player.gameObject.SetActive(false);
    }
}
