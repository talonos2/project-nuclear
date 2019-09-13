using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

// Define an extension method in a non-nested static class.
public static class Extensions
{

    public static int HandleAnimation(this AttackAnimation anim, float timeSinceStart, GameObject userSprite, GameObject targetSprite, Stats targetStats, Stats userStats)
    {
        switch (anim)
        {
            case AttackAnimation.HOP:
                return HandleHopAnimation(timeSinceStart, userSprite, targetSprite, targetStats, userStats);
        }
        Debug.LogWarning("Unknown Attack Animation!" + anim);
        return -1;
    }

    public static float GetAnimationLength(this AttackAnimation anim)
    {
        switch (anim)
        {
            case AttackAnimation.HOP:
                return 1;
        }
        Debug.LogWarning("Unknown Attack Animation!" + anim);
        return 1000;
    }

    public static float GetDamagePoint(this AttackAnimation anim)
    {
        switch (anim)
        {
            case AttackAnimation.HOP:
                return AttackAnimationManager.Instance.enemyKnockBackStart;
        }
        Debug.LogWarning("Unknown Attack Animation!" + anim);
        return 1000;
    }

    private static int HandleHopAnimation(float timeSinceStart, GameObject userSprite, GameObject targetSprite, Stats targetStats, Stats userStats)
    {
        AttackAnimationManager aam = AttackAnimationManager.Instance;
        int flip = (userStats.homePositionOnScreen.x < targetStats.homePositionOnScreen.x ? 1 : -1);
        if (timeSinceStart < aam.initialHopDuration)
        {
            float amountThrough = timeSinceStart /aam.initialHopDuration;
            float currentJumpHeight = -Mathf.Pow(-((amountThrough * 2 * Mathf.Pow(aam.initialHopHeight, .5f)) - Mathf.Pow(aam.initialHopHeight, .5f)),2)+aam.initialHopHeight;
            userSprite.transform.localPosition = Vector3.Lerp(userStats.homePositionOnScreen, targetStats.homePositionOnScreen, amountThrough)+new Vector3(0,currentJumpHeight);
        }
        if (timeSinceStart > aam.enemyKnockBackStart && timeSinceStart < aam.enemyKnockBackStart + aam.enemyKnockBackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemyKnockBackStart) / aam.enemyKnockBackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen, targetStats.homePositionOnScreen + new Vector2(aam.knockBackXOffset, 0)*flip, amountThrough);
        }
        if (timeSinceStart > aam.enemySpringbackStart && timeSinceStart < aam.enemySpringbackStart + aam.enemySpringbackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemySpringbackStart) / aam.enemySpringbackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen + new Vector2(aam.knockBackXOffset, 0) * flip, targetStats.homePositionOnScreen + new Vector2(aam.springBackXOffset, 0) * flip, amountThrough);
        }
        if (timeSinceStart > aam.enemyMoveBackStart && timeSinceStart < aam.enemyMoveBackStart + aam.enemyMoveBackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemyMoveBackStart) / aam.enemyMoveBackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen + new Vector2(aam.springBackXOffset, 0) * flip, targetStats.homePositionOnScreen, amountThrough);
        }
        if (timeSinceStart > aam.returnHopStart && timeSinceStart < aam.returnHopStart + aam.returnHopDuration)
        {
            float amountThrough = (timeSinceStart - aam.returnHopStart) / aam.returnHopDuration;
            float currentJumpHeight = -Mathf.Pow(-((amountThrough * 2 * Mathf.Pow(aam.returnHopHeight, .5f)) - Mathf.Pow(aam.returnHopHeight, .5f)), 2)+aam.returnHopHeight;
            userSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen, userStats.homePositionOnScreen, amountThrough) + new Vector3(0, currentJumpHeight);
        }

        //Which frame are we in?

        if (timeSinceStart > aam.initialHopDuration && timeSinceStart < aam.initialHopDuration + aam.defaultAttackFrameTime)
        {
            return 1;
        }
        if (timeSinceStart > aam.initialHopDuration + aam.defaultAttackFrameTime && timeSinceStart < aam.initialHopDuration + aam.defaultAttackFrameTime*2)
        {
            return 2;
        }
        if (timeSinceStart > aam.initialHopDuration + aam.defaultAttackFrameTime * 2 && timeSinceStart < aam.initialHopDuration + aam.defaultAttackFrameTime*5)
        {
            return 3;
        }
        return 0;
    }
}

public enum AttackAnimation { HOP };
