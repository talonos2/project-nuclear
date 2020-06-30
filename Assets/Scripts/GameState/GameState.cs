using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool isInBattle = false; //Pauses most functions except the combat part of a game
    public static bool fullPause = false; //pauses all parts of the game except menus. Used for 'escape to main menu' options



    internal static bool endRunFlag;

}
