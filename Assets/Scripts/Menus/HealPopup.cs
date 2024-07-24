using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Class for the heal popup ui elements in battle
public class HealPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healing;

    Transform healTransform;

    private void Awake()
    {
        healTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        StartCoroutine(Co_Disappear());
    }

    // Hides the popup element after one second
    private IEnumerator Co_Disappear()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    // Sets the heal value accordingly
    public void SetHeal(string healText)
    {
        healing.text = healText;
    }
}
