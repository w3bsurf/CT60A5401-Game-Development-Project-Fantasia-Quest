using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, and changing methods
 */

// Class the damage popup ui element in battles
public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damage;

    Transform damageTransform;

    private void Awake()
    {
        damageTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        StartCoroutine(Co_Disappear());
    }

    // Coroutine to hide the UI element
    private IEnumerator Co_Disappear()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    // Sets the damage text value accordingly
    public void SetDamage(string damageText)
    {
        damage.text = damageText;
    }
}
