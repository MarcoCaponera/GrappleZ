using System.Collections.Generic;
using UnityEngine;
using System;


public static class GlobalEventArgsFactory {

#if DEBUG
    private delegate string EventDebug(GlobalEventArgs message);
    private static Dictionary<GlobalEventIndex, EventDebug> methodDebugString;

    static GlobalEventArgsFactory() {
        methodDebugString = new Dictionary<GlobalEventIndex, EventDebug>();
        methodDebugString.Add(GlobalEventIndex.LockPlayer, new EventDebug(LockPlayerDebug));
        methodDebugString.Add(GlobalEventIndex.PlayerDeath, new EventDebug(PlayerDeathDebug));
        methodDebugString.Add(GlobalEventIndex.PlayerHealthUpdated, new EventDebug(PlayerHealthUpdatedDebug));
        methodDebugString.Add(GlobalEventIndex.StartDialogue, new EventDebug(StartDialogueDebug));
        methodDebugString.Add(GlobalEventIndex.DialoguePerformed, new EventDebug(DialoguePerformedDebug));
        methodDebugString.Add(GlobalEventIndex.ShakeCamera, new EventDebug(ShakeCameraDebug));
        methodDebugString.Add(GlobalEventIndex.PlayerEnergyUpdated, new EventDebug(PlayerEnergyUpdatedDebug));
    }

    public static string GetDebugString(GlobalEventIndex eventType, GlobalEventArgs message) {
        return methodDebugString[eventType](message);
    }
#endif


    #region LockPlayer
    public static EventArgs LockPlayerFactory(bool lockValue) {
        GlobalEventArgs message = new GlobalEventArgs();
        message.args = new ExtendedVariable[1];
        message.args[0] = new ExtendedVariable("LockValue", ExtendedVariableType.Bool, lockValue);
        return message;
    }

    public static void LockPlayerParser(GlobalEventArgs message, out bool lockValue) {
        lockValue = (bool)message.args[0].GetValue();
    }

    public static string LockPlayerDebug (GlobalEventArgs message) {
        return " lock value: " + (bool)message.args[0].GetValue();
    }
    #endregion

    #region PlayerDeath
    public static GlobalEventArgs PlayerDeathFactory () {
        GlobalEventArgs message = new GlobalEventArgs();
        return message;
    }

    public static void PlayerDeathParser (GlobalEventArgs message) {

    }

    public static string PlayerDeathDebug (GlobalEventArgs message) {
        return string.Empty;
    }
    #endregion

    #region PlayerHealthUpdated
    public static GlobalEventArgs PlayerHealthUpdatedFactory (float maxHP, float currentHP) {
        GlobalEventArgs message = new GlobalEventArgs();
        message.args = new ExtendedVariable[2];
        message.args[0] = new ExtendedVariable("MaxHP", ExtendedVariableType.Float, maxHP);
        message.args[1] = new ExtendedVariable("CurrenctHP", ExtendedVariableType.Float, currentHP);
        return message;
    }

    public static void PlayerHealthUpdatedParser (GlobalEventArgs message, out float maxHP, out float currentHP) {
        maxHP = (float)message.args[0].GetValue();
        currentHP = (float)message.args[1].GetValue();
    }

    public static string PlayerHealthUpdatedDebug (GlobalEventArgs message) {
        return " maxHP: " + message.args[0].GetValue().ToString() + " currentHP: " + 
            message.args[1].GetValue().ToString();
    }
    #endregion

    #region StartDialogue
    public static GlobalEventArgs StartDialogueFactory (uint dialogueID) {
        GlobalEventArgs message = new GlobalEventArgs();
        message.args = new ExtendedVariable[1];
        message.args[0] = new ExtendedVariable("DialogueID", ExtendedVariableType.UInt, dialogueID);
        return message;
    }

    public static void StartDialogueParser (GlobalEventArgs message, out uint dialogueID) {
        dialogueID = (uint)message.args[0].GetValue();
    }

    public static string StartDialogueDebug (GlobalEventArgs message) {
        return " with dialogueID: " + message.args[0].GetValue().ToString();
    }

    #endregion

    #region DialoguePerformed
    public static GlobalEventArgs DialoguePerformedFactory(uint dialogueID) {
        GlobalEventArgs message = new GlobalEventArgs();
        message.args = new ExtendedVariable[1];
        message.args[0] = new ExtendedVariable("DialogueID", ExtendedVariableType.UInt, dialogueID);
        return message;
    }

    public static void DialoguePerformedParser(GlobalEventArgs message, out uint dialogueID) {
        dialogueID = (uint)message.args[0].GetValue();
    }

    public static string DialoguePerformedDebug(GlobalEventArgs message) {
        return " with dialogueID: " + message.args[0].GetValue().ToString();
    }

    #endregion

    #region ShakeCamera
    public static GlobalEventArgs ShakeCameraFactory(float amplitude, float frequency, 
        float duration) {
        GlobalEventArgs message = new GlobalEventArgs();
        message.args = new ExtendedVariable[3];
        message.args[0] = new ExtendedVariable("Amplitude", ExtendedVariableType.Float, amplitude);
        message.args[1] = new ExtendedVariable("Frequence", ExtendedVariableType.Float, frequency);
        message.args[2] = new ExtendedVariable("Duration", ExtendedVariableType.Float, duration);
        return message;
    }

    public static void ShakeCameraParser(GlobalEventArgs message, out float amplitude, 
        out float frequency, out float duration) {
        amplitude = (float)message.args[0].GetValue();
        frequency = (float)message.args[1].GetValue();
        duration = (float)message.args[2].GetValue();
    }

    public static string ShakeCameraDebug(GlobalEventArgs message) {
        return "with amplitude: " + (float)message.args[0].GetValue() + ", frequency: " +
            (float)message.args[1].GetValue() + " and duration: " + (float)message.args[2].GetValue();
    }

    #endregion

    #region PlayerEnergyUpdated
    public static GlobalEventArgs PlayerEnergyUpdatedFactory(float maxEnergy, float currentEnergy) {
        GlobalEventArgs message = new GlobalEventArgs();
        message.args = new ExtendedVariable[2];
        message.args[0] = new ExtendedVariable("MaxEnergy", ExtendedVariableType.Float, maxEnergy);
        message.args[1] = new ExtendedVariable("CurrentEnergy", ExtendedVariableType.Float, currentEnergy);
        return message;
    }

    public static void PlayerEnergyUpdatedParser(GlobalEventArgs message, out float maxEnergy, out float currentEnergy) {
        maxEnergy = (float)message.args[0].GetValue();
        currentEnergy = (float)message.args[1].GetValue();
    }

    public static string PlayerEnergyUpdatedDebug(GlobalEventArgs message) {
        return " maxEnergy: " + message.args[0].GetValue().ToString() + " currentEnergy: " +
            message.args[1].GetValue().ToString();
    }
    #endregion

}
