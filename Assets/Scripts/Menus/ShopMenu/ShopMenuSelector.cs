using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified with comments, changed for the shop menu, adjusted variable names, adjusted Update method so that menu selector can be looped from top to bottom or bottom to top
 */

// Class for Shop Menu selectable options
public class ShopMenuSelector : MonoBehaviour
{
    private ShopMenu menu;
    private RectTransform rectTransform;
    private List<RectTransform> selectables = new List<RectTransform>();

    [SerializeField] private AudioSource menuHover;

    public bool IsActive { get; set; } = false;
    public int Index { get; set; } = 0;

    private void OnEnable()
    {
        menu = GetComponentInParent<ShopMenu>();
        rectTransform = GetComponent<RectTransform>();
        selectables.Clear();

        for (int i = 0; i < rectTransform.parent.childCount; i++)
        {
            if (rectTransform.parent.GetChild(i).CompareTag("Selectable") && rectTransform.parent.GetChild(i).GetComponent<RectTransform>().gameObject.activeSelf)
            {
                selectables.Add(rectTransform.parent.GetChild(i).GetComponent<RectTransform>());
            }
        }
    }

    private void Update()
    {
        if (!IsActive)
        {
            return;
        }
        if (rectTransform.anchoredPosition != selectables[Index].anchoredPosition)
        {
            MoveToSelectedOption();
        }
        if (Game.Manager.State == GameState.ShopMenu)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && Index > 0)
            {
                menuHover.Play();
                Index--;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && (Index == 0))
            {
                menuHover.Play();
                Index = selectables.Count - 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && (Index < selectables.Count - 1))
            {
                menuHover.Play();
                Index++;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && (Index == selectables.Count - 1) && (Index != 0))
            {
                menuHover.Play();
                Index = 0;
            }
        }
    }

    // Moves the selector ui element to the selected menu option
    private void MoveToSelectedOption()
    {
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, selectables[Index].anchoredPosition, 24f);
    }
}
