using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for different interactables to inherit from
public interface IInteractable
{
    ScriptableObject Interaction { get; }
    void Interact();
}
