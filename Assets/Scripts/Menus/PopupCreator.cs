using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, modified methods, added createhealpopup method
 */

// Class for creating popup elements such as damage numbers and heal numbers
public class PopupCreator : MonoBehaviour
{
    [SerializeField] private DamagePopup damagePopup;
    [SerializeField] private HealPopup healPopup;

    private Battle battle;

    public void Awake()
    {
        battle = GetComponentInParent<Battle>();
    }

    // Creates a damage popup when an actor deals damage in battle
    public void CreateDamagePopup(Actor target, int amount)
    {
        Transform canvas = target.GetComponentInChildren<Canvas>().transform;
        DamagePopup damagePop = Instantiate(damagePopup, canvas);

        if (amount <= 0)
        {
            damagePop.SetDamage("Miss!");
        }
        else
        {
            damagePop.SetDamage(amount.ToString());
        }
    }

    // Creates a damage popup when an actor heals in battle
    public void CreateHealPopup(Actor target, int amount)
    {
        Transform canvas = target.GetComponentInChildren<Canvas>().transform;
        HealPopup healPop =  Instantiate(healPopup, canvas);
        healPop.SetHeal(amount.ToString());
    }
}
