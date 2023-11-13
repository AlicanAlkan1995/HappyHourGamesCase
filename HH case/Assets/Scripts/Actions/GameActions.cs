using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameActions
{
    public static Action<Vector2Int> OnCharacterMoved;
    
    public static Action<Item, Vector2Int> OnItemMoved;

    public static Action<string, int> OnPlayerScoreChanged;

}
