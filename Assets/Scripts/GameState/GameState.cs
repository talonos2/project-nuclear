using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool isInBattle = false; //Pauses most functions except the combat part of a game
    private static bool fullPause = false; //pauses all parts of the game except menus. Used for 'escape to main menu' options
    public static bool pickingItem = false;

    public static void setFullPause(bool setPause) {
        fullPause = setPause;
    }

    internal static bool getFullPauseStatus()
    {
        return fullPause;
    }


    //internal static bool endRunFlag;

}
