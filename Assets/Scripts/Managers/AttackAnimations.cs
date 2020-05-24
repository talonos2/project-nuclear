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
        Time.timeScale = AttackAnimationManager.Instance.timeWarp;
        switch (anim)
        {
            case AttackAnimation.HOP:
                return HandleHopAnimation(timeSinceStart, userSprite, targetSprite, targetStats, userStats);
            case AttackAnimation.BLAST:
                return HandleBlastAnimation(timeSinceStart, userSprite, targetSprite, targetStats, userStats);
            case AttackAnimation.PLAYER_HOP:
                return HandleHopAnimation(timeSinceStart, userSprite, targetSprite, targetStats, userStats, true);
            case AttackAnimation.FIVE_FRAME_HOP:
                return HandleHopAnimation(timeSinceStart, userSprite, targetSprite, targetStats, userStats, true, true);
            case AttackAnimation.FIVE_FRAME_HOP_WITH_ANTICIPATE:
                return HandleHopAnimation(timeSinceStart, userSprite, targetSprite, targetStats, userStats, true, true, true);
            case AttackAnimation.STATIONARY_THRUST:
                return HandleStationaryThrustAnimation(timeSinceStart, userSprite, targetSprite, targetStats, userStats);
        }
        Debug.LogWarning("Unknown Attack Animation!" + anim);
        return -1;
    }

    private static int HandleBlastAnimation(float timeSinceStart, GameObject userSprite, GameObject targetSprite, Stats targetStats, Stats userStats)
    {
        AttackAnimationManager aam = AttackAnimationManager.Instance;
        int flip = (userStats.homePositionOnScreen.x < targetStats.homePositionOnScreen.x ? 1 : -1);

        //User Recoil back.
        if (timeSinceStart > aam.initialBlastImpactTime && timeSinceStart < aam.blastRecoilFinishTime)
        {
            float amountThrough = (timeSinceStart-aam.initialBlastImpactTime) / (aam.blastRecoilFinishTime - aam.initialBlastImpactTime);
            Debug.Log(amountThrough);
            amountThrough = Mathf.Pow(amountThrough, aam.blastRecoilFriction);
            Vector3 startPosit = userStats.homePositionOnScreen;
            Vector3 endPosit = userStats.homePositionOnScreen + aam.blastRecoilAmount;
            Vector3 lerpedPosit = Vector3.Lerp(startPosit, endPosit, amountThrough);
            userSprite.transform.localPosition = lerpedPosit;
        }

        //Target get knocked back.
        if (timeSinceStart > aam.enemyKnockBackStart && timeSinceStart < aam.enemyKnockBackStart + aam.enemyKnockBackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemyKnockBackStart) / aam.enemyKnockBackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen, targetStats.homePositionOnScreen + new Vector2(aam.knockBackXOffset, 0) * flip, amountThrough);
            AdjustForScale(targetSprite);
        }

        //Target rubber-band back forward
        if (timeSinceStart > aam.enemySpringbackStart && timeSinceStart < aam.enemySpringbackStart + aam.enemySpringbackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemySpringbackStart) / aam.enemySpringbackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen + new Vector2(aam.knockBackXOffset, 0) * flip, targetStats.homePositionOnScreen + new Vector2(aam.springBackXOffset, 0) * flip, amountThrough);
            AdjustForScale(targetSprite);
        }

        //Target move back to home position.
        if (timeSinceStart > aam.enemyMoveBackStart && timeSinceStart < aam.enemyMoveBackStart + aam.enemyMoveBackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemyMoveBackStart) / aam.enemyMoveBackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen + new Vector2(aam.springBackXOffset, 0) * flip, targetStats.homePositionOnScreen, amountThrough);
            AdjustForScale(targetSprite);
        }

        //User move back to home position.
        if (timeSinceStart > aam.returnRecoilStart && timeSinceStart < aam.returnRecoilStart + aam.returnRecoilReturnDuration)
        {
            float amountThrough = (timeSinceStart - aam.returnRecoilStart) / aam.returnRecoilReturnDuration;
            Vector3 endPosit = userStats.homePositionOnScreen;
            Vector3 startPosit = userStats.homePositionOnScreen + aam.blastRecoilAmount;
            userSprite.transform.localPosition = Vector3.Lerp(startPosit, endPosit, amountThrough);
        }

        //Which frame are we in?
        if (timeSinceStart > aam.initialBlastImpactTime - aam.blastWarmupDuration && timeSinceStart < aam.initialBlastImpactTime)
        {
            return 1;
        }
        if (timeSinceStart > aam.initialBlastImpactTime && timeSinceStart < (aam.initialBlastImpactTime + aam.blastFrameTime.x))
        {
            return 2;
        }
        if (timeSinceStart > aam.initialBlastImpactTime && timeSinceStart < (aam.initialBlastImpactTime + aam.blastFrameTime.x + aam.blastFrameTime.y))
        {
            return 3;
        }
        if (timeSinceStart > aam.initialBlastImpactTime && timeSinceStart < (aam.initialBlastImpactTime + aam.blastFrameTime.x + aam.blastFrameTime.y + aam.blastFrameTime.z))
        {
            return 4;
        }
        if (timeSinceStart > aam.initialBlastImpactTime && timeSinceStart < (aam.initialBlastImpactTime + aam.blastFrameTime.x + aam.blastFrameTime.y + aam.blastFrameTime.z + aam.blastFrameTime.w))
        {
            return 5;
        }
        return 0;
    }

    private static void AdjustForScale(GameObject targetSprite)
    {
        float yOffset = (1 - (6*targetSprite.transform.localScale.y)) *3 ;
        targetSprite.transform.Translate(0, yOffset, 0);
    }

    private static int HandleStationaryThrustAnimation(float timeSinceStart, GameObject userSprite, GameObject targetSprite, Stats targetStats, Stats userStats)
    {
        AttackAnimationManager aam = AttackAnimationManager.Instance;
        int flip = (userStats.homePositionOnScreen.x < targetStats.homePositionOnScreen.x ? 1 : -1);

        //Target get knocked back.
        if (timeSinceStart > aam.enemyKnockBackStart && timeSinceStart < aam.enemyKnockBackStart + aam.enemyKnockBackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemyKnockBackStart) / aam.enemyKnockBackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen, targetStats.homePositionOnScreen + new Vector2(aam.knockBackXOffset, 0) * flip, amountThrough);
            AdjustForScale(targetSprite);
        }

        //Target rubber-band back forward
        if (timeSinceStart > aam.enemySpringbackStart && timeSinceStart < aam.enemySpringbackStart + aam.enemySpringbackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemySpringbackStart) / aam.enemySpringbackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen + new Vector2(aam.knockBackXOffset, 0) * flip, targetStats.homePositionOnScreen + new Vector2(aam.springBackXOffset, 0) * flip, amountThrough);
            AdjustForScale(targetSprite);
        }

        //Target move back to home position.
        if (timeSinceStart > aam.enemyMoveBackStart && timeSinceStart < aam.enemyMoveBackStart + aam.enemyMoveBackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemyMoveBackStart) / aam.enemyMoveBackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen + new Vector2(aam.springBackXOffset, 0) * flip, targetStats.homePositionOnScreen, amountThrough);
            AdjustForScale(targetSprite);
        }

        //Which frame are we in?
        if (timeSinceStart > aam.initialThrustImpactTime - aam.thrustWarmupDuration && timeSinceStart < aam.initialThrustImpactTime)
        {
            return 1;
        }
        if (timeSinceStart > aam.initialThrustImpactTime && timeSinceStart < (aam.initialThrustImpactTime + aam.thrustFrameTime.x))
        {
            return 2;
        }
        if (timeSinceStart > aam.initialThrustImpactTime && timeSinceStart < (aam.initialThrustImpactTime + aam.thrustFrameTime.x + aam.thrustFrameTime.y))
        {
            return 3;
        }
        if (timeSinceStart > aam.initialThrustImpactTime && timeSinceStart < (aam.initialThrustImpactTime + aam.thrustFrameTime.x + aam.thrustFrameTime.y + aam.thrustFrameTime.z))
        {
            return 4;
        }
        if (timeSinceStart > aam.initialThrustImpactTime && timeSinceStart < (aam.initialThrustImpactTime + aam.thrustFrameTime.x + aam.thrustFrameTime.y + aam.thrustFrameTime.z + aam.thrustFrameTime.w))
        {
            return 5;
        }
        return 0;
    }

    public static float GetAnimationLength(this AttackAnimation anim)
    {
        switch (anim)
        {
            case AttackAnimation.HOP:
                return 1;
            case AttackAnimation.BLAST:
                return 1;
            case AttackAnimation.PLAYER_HOP:
                return 1;
            case AttackAnimation.FIVE_FRAME_HOP:
                return 1;
            case AttackAnimation.FIVE_FRAME_HOP_WITH_ANTICIPATE:
                return 1;
            case AttackAnimation.STATIONARY_THRUST:
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
            case AttackAnimation.BLAST:
                return AttackAnimationManager.Instance.enemyKnockBackStart;
            case AttackAnimation.PLAYER_HOP:
                return AttackAnimationManager.Instance.enemyKnockBackStart;
            case AttackAnimation.FIVE_FRAME_HOP:
                return AttackAnimationManager.Instance.enemyKnockBackStart;
            case AttackAnimation.FIVE_FRAME_HOP_WITH_ANTICIPATE:
                return AttackAnimationManager.Instance.enemyKnockBackStart;
            case AttackAnimation.STATIONARY_THRUST:
                return AttackAnimationManager.Instance.enemyKnockBackStart;
        }
        Debug.LogWarning("Unknown Attack Animation!" + anim);
        return 1000;
    }

    public static float GetAttackSoundPoint(this AttackAnimation anim)
    {
        switch (anim)
        {
            case AttackAnimation.HOP:
                return AttackAnimationManager.Instance.hopSwingSoundPoint;
            case AttackAnimation.BLAST:
                return AttackAnimationManager.Instance.blastSoundPoint;
            case AttackAnimation.PLAYER_HOP:
                return AttackAnimationManager.Instance.hopSwingSoundPoint;
            case AttackAnimation.FIVE_FRAME_HOP:
                return AttackAnimationManager.Instance.hopSwingSoundPoint;
            case AttackAnimation.FIVE_FRAME_HOP_WITH_ANTICIPATE:
                return AttackAnimationManager.Instance.hopSwingSoundPoint;
            case AttackAnimation.STATIONARY_THRUST:
                return AttackAnimationManager.Instance.thrustSoundPoint;
        }
        Debug.LogWarning("Unknown Attack Animation!" + anim);
        return 1000;
    }

    public static void PlaySound(this AttackAnimation anim)
    {
        switch (anim)
        {
            case AttackAnimation.HOP:
                SoundManager.Instance.PlaySound("Combat/EnemyAttackSwing", 1f);
                return;
            case AttackAnimation.BLAST:
                SoundManager.Instance.PlaySound("Combat/EnemyAttackSwing", 1f);
                return;
            case AttackAnimation.PLAYER_HOP:
                SoundManager.Instance.PlaySound("Combat/PlayerAttackSwing", 1f);
                return;
            case AttackAnimation.FIVE_FRAME_HOP:
                SoundManager.Instance.PlaySound("Combat/EnemyAttackSwing", 1f);
                return;
            case AttackAnimation.FIVE_FRAME_HOP_WITH_ANTICIPATE:
                SoundManager.Instance.PlaySound("Combat/EnemyAttackSwing", 1f);
                return;
            case AttackAnimation.STATIONARY_THRUST:
                SoundManager.Instance.PlaySound("Combat/EnemyAttackSwing", 1f);
                return;
        }
        Debug.LogWarning("Unknown Attack Animation!" + anim);
        return;
    }

    private static int HandleHopAnimation(float timeSinceStart, GameObject userSprite, GameObject targetSprite, Stats targetStats, Stats userStats, bool isPlayer = false, bool fiveFrame = false, bool anticipate = false)
    {
        AttackAnimationManager aam = AttackAnimationManager.Instance;
        int flip = (userStats.homePositionOnScreen.x < targetStats.homePositionOnScreen.x ? 1 : -1);

        //User Jump forward.
        if (timeSinceStart < aam.initialHopDuration)
        {
            float amountThrough = timeSinceStart /aam.initialHopDuration;
            float currentJumpHeight = -Mathf.Pow(-((amountThrough * 2 * Mathf.Pow(aam.initialHopHeight, .5f)) - Mathf.Pow(aam.initialHopHeight, .5f)),2)+aam.initialHopHeight;
            Vector3 startPosit = userStats.homePositionOnScreen;
            Vector3 endPosit = targetStats.homePositionOnScreen + userStats.strikingPointOffset + targetStats.gettingStruckPointOffset;
            Vector3 lerpedPosit = Vector3.Lerp(startPosit, endPosit, amountThrough) + new Vector3(0, currentJumpHeight);
            userSprite.transform.localPosition = lerpedPosit;
            if (!isPlayer) { AdjustForScale(userSprite); }
        }

        //Target get knocked back.
        if (timeSinceStart > aam.enemyKnockBackStart && timeSinceStart < aam.enemyKnockBackStart + aam.enemyKnockBackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemyKnockBackStart) / aam.enemyKnockBackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen, targetStats.homePositionOnScreen + new Vector2(aam.knockBackXOffset, 0)*flip, amountThrough);
            if (isPlayer) { AdjustForScale(targetSprite); }
        }

        //Target rubber-band back forward
        if (timeSinceStart > aam.enemySpringbackStart && timeSinceStart < aam.enemySpringbackStart + aam.enemySpringbackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemySpringbackStart) / aam.enemySpringbackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen + new Vector2(aam.knockBackXOffset, 0) * flip, targetStats.homePositionOnScreen + new Vector2(aam.springBackXOffset, 0) * flip, amountThrough);
            if (isPlayer) { AdjustForScale(targetSprite); }
        }

        //Target move back to home position.
        if (timeSinceStart > aam.enemyMoveBackStart && timeSinceStart < aam.enemyMoveBackStart + aam.enemyMoveBackDuration)
        {
            float amountThrough = (timeSinceStart - aam.enemyMoveBackStart) / aam.enemyMoveBackDuration;
            targetSprite.transform.localPosition = Vector3.Lerp(targetStats.homePositionOnScreen + new Vector2(aam.springBackXOffset, 0) * flip, targetStats.homePositionOnScreen, amountThrough);
            if (isPlayer) { AdjustForScale(targetSprite); }
        }

        //User move back to home position.
        if (timeSinceStart > aam.returnHopStart && timeSinceStart < aam.returnHopStart + aam.returnHopDuration)
        {
            float amountThrough = (timeSinceStart - aam.returnHopStart) / aam.returnHopDuration;
            float currentJumpHeight = -Mathf.Pow(-((amountThrough * 2 * Mathf.Pow(aam.returnHopHeight, .5f)) - Mathf.Pow(aam.returnHopHeight, .5f)), 2)+aam.returnHopHeight;
            Vector3 endPosit = userStats.homePositionOnScreen;
            Vector3 startPosit = targetStats.homePositionOnScreen + userStats.strikingPointOffset + targetStats.gettingStruckPointOffset;
            userSprite.transform.localPosition = Vector3.Lerp(startPosit, endPosit, amountThrough) + new Vector3(0, currentJumpHeight);
            if (!isPlayer) { AdjustForScale(userSprite); }
        }

        //Which frame are we in?
        if (anticipate && timeSinceStart > 0 && timeSinceStart < aam.initialHopDuration)
        {
            return 5;
        }
        if (timeSinceStart > aam.initialHopDuration && timeSinceStart < aam.initialHopDuration + aam.defaultAttackFrameTime)
        {
            return 1;
        }
        if (timeSinceStart > aam.initialHopDuration + aam.defaultAttackFrameTime && timeSinceStart < aam.initialHopDuration + aam.defaultAttackFrameTime*2)
        {
            return 2;
        }
        if (fiveFrame && timeSinceStart > aam.initialHopDuration + aam.defaultAttackFrameTime * 3 && timeSinceStart < aam.initialHopDuration + aam.defaultAttackFrameTime * 5)
        {
            return 4;
        }
        if (timeSinceStart > aam.initialHopDuration + aam.defaultAttackFrameTime * 2 && timeSinceStart < aam.initialHopDuration + aam.defaultAttackFrameTime*5)
        {
            return 3;
        }
        return 0;
    }
}

public enum AttackAnimation { HOP, BLAST, PLAYER_HOP, FIVE_FRAME_HOP, FIVE_FRAME_HOP_WITH_ANTICIPATE, STATIONARY_THRUST };
