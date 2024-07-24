using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Added comments.
 */

// An abstract interface for different cutscene commands
public interface CutsceneCommand
{
    IEnumerator Co_Execute();
    bool IsFinished { get; }
}
