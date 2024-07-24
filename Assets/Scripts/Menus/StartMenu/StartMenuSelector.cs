using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for the start menu selector
public class StartMenuSelector : MonoBehaviour
{
    private StartMenu menu;
    private RectTransform rectTransform;
    private List<RectTransform> selectables = new List<RectTransform>();

    [SerializeField] private AudioSource menuHover;

    public bool IsActive { get; set; } = false;
    public int Index { get; set; } = 0;

    private void OnEnable()
    {
        menu = GetComponentInParent<StartMenu>();
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

    // Moves the selector ui element to the selected menu option
    private void MoveToSelectedOption()
    {
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, selectables[Index].anchoredPosition, 8f);
    }
}
