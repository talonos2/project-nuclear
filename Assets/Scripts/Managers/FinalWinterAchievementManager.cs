using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
At Any Cost                - Kill one of your own villagers
No Respect for the Fallen  - Kill Every Villager
May you make it further than I - Unlock A Shortcut
Paving the Way             - Unlock Every Shortcut
Clutch Return              - Send a piece of equipment home in your last 3 seconds
Treasures of the Cave      - Get a Rare Item
The Hero Has Fallen        - Lose Douglass
A Toast to Next Winter     - Beat the Game
Final Winter               - Lose the Game
Village Savior             - Beat the Game without killing Any Corrupted Villagers
I Am the Last              - Beat the Game as the Elder
Dark Lord Rising           - Beat the Game as Todd.
Heart of Winter            - Gain the Ice Power
One with the Cave          - Gain the Earth Power
Legs of Smoke              - Gain the Fire Power
Soul of Wind               - Gain the Wind Power
Slayer                     - Defeat any enemy in the last five levels with a single blow.
Attire of Heroes           - Equip the [best weapon], the [best armor], or the [best crown]
Magic of the Ancients      - Get [so many] crystals (About 1.25x average for beating the game)
Hero of Humble Origins     - Achieve level X (About 1.25x average for beating the game)
Chronicler                 - Beat the game with ten different people
Historian                  - Beat the game with 25 different people
Spirit of the People       - Talk to everybody in town on all 28 days.
I have seen it all...      - See both the good and the bad ending for 28 different people.
Fratricide                 - Kill Tedd as Todd, or vise versa
Fallen from Grace          - As Sir Meyster the Paladin, kill your own squire.
Children of the Conquerors - Kill [x] monsters (10x what's needed for a typical playthrough.) 
*/

public enum FWBoolAchievement { KILL_VILLAGER, UNLOCK_SHORTCUT, RETURN_ITEM_ON_TIME, FIND_RARE_ITEM, LOSE_DOUGLAS,
WIN_GAME, LOSE_GAME, WIN_GAME_NO_VILLAGE_KILLS, WIN_GAME_AS_ELDER, WIN_GAME_AS_TODD, GET_ICE, GET_EARTH, GET_FIRE, GET_AIR, KILL_ZONE_5_IN_ONE_SHOT,
EQUIP_BEST_RARE_ITEM, KILL_TODD_AS_TEDD, KILL_PENDLETON_AS_MEYSTER, COMPLETE_LEVEL_FAST, HAS_SWEATER}

public enum FWStatAchievement
{
    UNLOCK_ALL_SHORTCUTS, GET_LOTS_OF_CRYSTALS, GET_LOTS_OF_LEVELS, WIN_WITH_10_PEOPLE, WIN_WITH_25_PEOPLE, READ_ALL_ENDINGS, READ_ALL_DIALOGUE,
    KILL_LOTS_OF_MONSTERS, REACH_LEVEL_4_NO_PICKUPS, REACH_LEVEL_16_NO_PICKUPS, REACH_LEVEL_16_FEW_MONSTERS, COLLECT_12_KNIVES
}

public static class AchievementExtensions
{
    public static string GetName (this FWBoolAchievement achievement)
    {
        switch (achievement) {
            case FWBoolAchievement.KILL_VILLAGER:
                return "KILL_VILLAGER";
            case FWBoolAchievement.UNLOCK_SHORTCUT:
                return "UNLOCK_SHORTCUT";
            case FWBoolAchievement.RETURN_ITEM_ON_TIME:
                return "RETURN_ITEM_ON_TIME";
            case FWBoolAchievement.FIND_RARE_ITEM:
                return "FIND_RARE_ITEM";
            case FWBoolAchievement.LOSE_DOUGLAS:
                return "LOSE_DOUGLAS";
            case FWBoolAchievement.WIN_GAME:
                return "WIN_GAME";
            case FWBoolAchievement.LOSE_GAME:
                return "LOSE_GAME";
            case FWBoolAchievement.WIN_GAME_NO_VILLAGE_KILLS:
                return "WIN_GAME_NO_VILLAGE_KILLS";
            case FWBoolAchievement.WIN_GAME_AS_ELDER:
                return "WIN_GAME_AS_ELDER";
            case FWBoolAchievement.WIN_GAME_AS_TODD:
                return "WIN_GAME_AS_TODD";
            case FWBoolAchievement.GET_ICE:
                return "GET_ICE";
            case FWBoolAchievement.GET_EARTH:
                return "GET_EARTH";
            case FWBoolAchievement.GET_FIRE:
                return "GET_FIRE";
            case FWBoolAchievement.GET_AIR:
                return "GET_AIR";
            case FWBoolAchievement.KILL_ZONE_5_IN_ONE_SHOT:
                return "KILL_ZONE_5_IN_ONE_SHOT";
            case FWBoolAchievement.EQUIP_BEST_RARE_ITEM:
                return "EQUIP_BEST_RARE_ITEM";
            case FWBoolAchievement.KILL_TODD_AS_TEDD:
                return "KILL_TODD_AS_TEDD";
            case FWBoolAchievement.KILL_PENDLETON_AS_MEYSTER:
                return "KILL_PENDLETON_AS_MEYSTER";
            case FWBoolAchievement.COMPLETE_LEVEL_FAST:
                return "COMPLETE_LEVEL_FAST";
            case FWBoolAchievement.HAS_SWEATER:
                return "HAS_SWEATER";
            default:
                throw new UnityException("Bad Achievement String.");
        }
    }

    public static string GetName(this FWStatAchievement achievement)
    {
        switch (achievement)
        {
            case FWStatAchievement.UNLOCK_ALL_SHORTCUTS:
                return "UNLOCK_ALL_SHORTCUTSz";
            case FWStatAchievement.GET_LOTS_OF_CRYSTALS:
                return "GET_LOTS_OF_CRYSTALSz";
            case FWStatAchievement.GET_LOTS_OF_LEVELS:
                return "GET_LOTS_OF_LEVELSz";
            case FWStatAchievement.WIN_WITH_10_PEOPLE:
                return "WIN_WITH_10_PEOPLEz";
            case FWStatAchievement.WIN_WITH_25_PEOPLE:
                return "WIN_WITH_25_PEOPLEz";
            case FWStatAchievement.READ_ALL_ENDINGS:
                return "READ_ALL_ENDINGSz";
            case FWStatAchievement.READ_ALL_DIALOGUE:
                return "READ_ALL_DIALOGUEz";
            case FWStatAchievement.KILL_LOTS_OF_MONSTERS:
                return "KILL_LOTS_OF_MONSTERSz";
            case FWStatAchievement.REACH_LEVEL_4_NO_PICKUPS:
                return "REACH_LEVEL_4_NO_PICKUPSz";
            case FWStatAchievement.REACH_LEVEL_16_NO_PICKUPS:
                return "REACH_LEVEL_16_NO_PICKUPSz";
            case FWStatAchievement.REACH_LEVEL_16_FEW_MONSTERS:
                return "REACH_LEVEL_16_FEW_MONSTERSz";
            case FWStatAchievement.COLLECT_12_KNIVES:
                return "COLLECT_12_KNIVESz";
            default:
                throw new UnityException("Bad Achievement String.");
        }
    }
}

public class FinalWinterAchievementManager : Singleton<FinalWinterAchievementManager>
{
    public void GiveAchievement(FWBoolAchievement achievement)
    {
        try
        {
            Steamworks.SteamUserStats.GetAchievement(achievement.GetName(), out bool alreadyComplete);
            if (!alreadyComplete)
            {
                Steamworks.SteamUserStats.SetAchievement(achievement.GetName());
            }
        }
        catch (System.InvalidOperationException e)
        {
            Debug.LogWarning("Steamworks Unreachable.");
        }
    }

    public void SetStatAndGiveAchievement(FWStatAchievement achievement, int currentStat)
    {
        try
        {
            Steamworks.SteamUserStats.GetStat(achievement.GetName(), out int oldStat);
            if (oldStat < currentStat)
            {
                Steamworks.SteamUserStats.SetStat(achievement.GetName(), currentStat);
            }
        }
        catch (System.InvalidOperationException e)
        {
            Debug.LogWarning("Steamworks Unreachable.");
        }
    }
}