using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(CharacterStats))]
public class CharacterStatsEditor : Editor
{
    private bool classes;
    private bool equipmentBonuses;
    private bool crystals;
    private bool equipment;
    private bool baseStats;
    private bool combatGraphics;

    public override void OnInspectorGUI()
    {
        CharacterStats charac = (CharacterStats)target;
        EditorGUILayout.LabelField("Missing Something? Add it to CharacterStatsEditor.cs.");

        charac.charName = EditorGUILayout.TextField("Character Name", charac.charName);

        baseStats = EditorGUILayout.BeginFoldoutHeaderGroup(baseStats, "Base Stats");
        if (baseStats)
        {
            charac.Level = EditorGUILayout.IntField("Level", charac.Level);
            charac.MaxHP = EditorGUILayout.IntField("MaxHP", charac.MaxHP);
            charac.HP = EditorGUILayout.IntField("HP", charac.HP);
            charac.MaxMana = EditorGUILayout.IntField("MaxMana", charac.MaxMana);
            charac.mana = EditorGUILayout.IntField("mana", charac.mana);
            charac.attack = EditorGUILayout.FloatField("attack", charac.attack);
            charac.defense = EditorGUILayout.FloatField("defense", charac.defense);
            charac.experience = EditorGUILayout.IntField("experience", charac.experience);
            charac.expToLevel = EditorGUILayout.IntField("expToLevel", charac.expToLevel);
            charac.currentPower =EditorGUILayout.IntField("currentPower", charac.currentPower);
            charac.powersGained = EditorGUILayout.IntField("powersGained", charac.powersGained);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        equipment = EditorGUILayout.BeginFoldoutHeaderGroup(equipment, "Equipment");
        if (equipment)
        {
            charac.weapon = (GameObject)EditorGUILayout.ObjectField("Weapon", charac.weapon, typeof(GameObject), false, null);
            charac.armor = (GameObject)EditorGUILayout.ObjectField("Armor", charac.armor, typeof(GameObject), false, null);
            charac.accessory = (GameObject)EditorGUILayout.ObjectField("Accessory", charac.accessory, typeof(GameObject), false, null);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        crystals = EditorGUILayout.BeginFoldoutHeaderGroup(crystals, "Crystals");
        if (crystals)
        {
            charac.HealthCrystalsGained = EditorGUILayout.IntField("Health Crystals Gained", charac.HealthCrystalsGained);
            charac.ManaCrystalsGained = EditorGUILayout.IntField("ManaCrystalsGained", charac.ManaCrystalsGained);
            charac.AttackCrystalsGained = EditorGUILayout.IntField("AttackCrystalsGained", charac.AttackCrystalsGained);
            charac.defenseCrystalsGained = EditorGUILayout.IntField("defenseCrystalsGained", charac.defenseCrystalsGained);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        equipmentBonuses = EditorGUILayout.BeginFoldoutHeaderGroup(equipmentBonuses, "Equipment Bonuses");
        if (equipmentBonuses)
        {
            charac.armorBonusDefense = EditorGUILayout.FloatField("armorBonusDefense", charac.armorBonusDefense);
            charac.weaponBonusAttack = EditorGUILayout.FloatField("weaponBonusAttack", charac.weaponBonusAttack);
            charac.accessoryAttack = EditorGUILayout.FloatField("accessoryAttack", charac.accessoryAttack);
            charac.accessoryDefense = EditorGUILayout.FloatField("accessoryDefense", charac.accessoryDefense);
            charac.accessoryHealth = EditorGUILayout.FloatField("accessoryHealth", charac.accessoryHealth);
            charac.accessoryMana = EditorGUILayout.FloatField("accessoryMana", charac.accessoryMana);
            charac.accessoryCritChance = EditorGUILayout.FloatField("accessoryCritChance", charac.accessoryCritChance);
            charac.accessoryExpBonus = EditorGUILayout.FloatField("accessoryExpBonus", charac.accessoryExpBonus);
            charac.accessoryIceBonus = EditorGUILayout.FloatField("accessoryIceBonus", charac.accessoryIceBonus);
            charac.accessoryEarthBonus = EditorGUILayout.FloatField("accessoryEarthBonus", charac.accessoryEarthBonus);
            charac.accessoryFireBonus = EditorGUILayout.FloatField("accessoryFireBonus", charac.accessoryFireBonus);
            charac.accessoryAirBonus = EditorGUILayout.FloatField("accessoryAirBonus", charac.accessoryAirBonus);
            charac.accessoryHPVamp = EditorGUILayout.FloatField("accessoryHPVamp", charac.accessoryHPVamp);
            charac.accessoryMPVamp = EditorGUILayout.FloatField("accessoryMPVamp", charac.accessoryMPVamp);
            charac.accessoryDodgeBonus = EditorGUILayout.FloatField("accessoryDodgeBonus", charac.accessoryDodgeBonus);
            charac.accessoryAttackPercent = EditorGUILayout.FloatField("accessoryAttackPercent", charac.accessoryAttackPercent);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        combatGraphics = EditorGUILayout.BeginFoldoutHeaderGroup(combatGraphics, "Combat Graphics");
        if (combatGraphics)
        {
            //EditorGUIUtility.LookLikeInspector();
            SerializedProperty tps = serializedObject.FindProperty("combatSprites");
            SerializedProperty tps2 = serializedObject.FindProperty("bustSprite");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(tps, true);
            EditorGUILayout.PropertyField(tps2, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            //EditorGUIUtility.LookLikeControls();
            charac.startPositionOnScreen = EditorGUILayout.Vector2Field("Start Position", charac.startPositionOnScreen);
            charac.homePositionOnScreen = EditorGUILayout.Vector2Field("Home Position", charac.homePositionOnScreen);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        classes = EditorGUILayout.BeginFoldoutHeaderGroup(classes, "Classes");
        if (classes)
        {
            charac.MageClass = EditorGUILayout.Toggle("Mage?", charac.MageClass);
            charac.FighterClass = EditorGUILayout.Toggle("Fighter?", charac.FighterClass);
            charac.SurvivorClass = EditorGUILayout.Toggle("Survivor?", charac.SurvivorClass);
            charac.ScoutClass = EditorGUILayout.Toggle("Scout?", charac.ScoutClass);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}

