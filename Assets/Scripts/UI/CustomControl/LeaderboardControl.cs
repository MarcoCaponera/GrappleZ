using GrappleZ_Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class LeaderboardControl : VisualElement
{
    private const string leaderboardTemplateName = "LeaderboardTemplate";
    private const string styleResource = "LeaderBoardUSS";

    public new class UxmlFactory : UxmlFactory<LeaderboardControl, UxmlTraits>
    {

    }

    private List<ScoreStruct> leaderboard;
    private ScrollView scrollView;
    private VisualTreeAsset Leaderbordtemplate;

    public List<ScoreStruct> Leaderboard
    {
        get { return leaderboard; }
        set
        {
            leaderboard = value;
            UpdateLeaderboard();
        }
    }

    public LeaderboardControl()
    {
        styleSheets.Add(Resources.Load<StyleSheet>(styleResource));
        Leaderbordtemplate = Resources.Load<VisualTreeAsset>(leaderboardTemplateName);
        var instance = Leaderbordtemplate.Instantiate();
        scrollView = instance.Q("ScoreListView").Q<ScrollView>();
        hierarchy.Add(instance);
    }


    private void UpdateLeaderboard()
    {
        scrollView.Clear();
        for (int i = 0; i < leaderboard.Count; i++)
        {
            LeaderboardEntryControl leaderboardEntry = new LeaderboardEntryControl(i + 1, leaderboard[i]);
            scrollView.Add(leaderboardEntry);
        }
    }
}
