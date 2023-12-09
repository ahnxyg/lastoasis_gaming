using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate int InsufficientBalance(string t);

public interface ActionsButton
{
    public InsufficientBalance[] summonFunctions{  get; set; }
}