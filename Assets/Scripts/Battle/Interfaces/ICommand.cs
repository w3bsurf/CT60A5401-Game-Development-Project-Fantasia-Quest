using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for different battle commands to inherit from
public interface ICommand
{
    bool IsFinished { get; }
    // Interface function for executing a command in battle
    IEnumerator Co_Execute(Battle battle);
}
