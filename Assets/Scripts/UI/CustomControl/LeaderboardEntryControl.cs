using GrappleZ_Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LeaderboardEntryControl : VisualElement
{
    private const string leaderboardEntryTemplateName = "LeaderBoardEntryTemplate";
    private const string styleResource = "LeaderBoardUSS";

    public new class UxmlFactory : UxmlFactory<LeaderboardControl, UxmlTraits>
    {

    }

    private VisualTreeAsset Leaderbordtemplate;

    public LeaderboardEntryControl(int positionnumber, ScoreStruct scoreValue)
    {
        styleSheets.Add(Resources.Load<StyleSheet>(styleResource));
        Leaderbordtemplate = Resources.Load<VisualTreeAsset>(leaderboardEntryTemplateName);
        var instance = Leaderbordtemplate.Instantiate();
        instance.Q<Label>("PositionLabel").text = positionnumber.ToString();
        instance.Q<Label>("ScoreLabel").text = scoreValue.Score.ToString();
        instance.Q<Label>("TimeLabel").text = scoreValue.Time.ToString();
        hierarchy.Add(instance);
    }


}
