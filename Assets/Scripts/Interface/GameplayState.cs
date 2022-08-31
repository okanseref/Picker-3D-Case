using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GameplayState 
{
    //Objects that have pause functions
    void SetGameplayState(bool active);
}
