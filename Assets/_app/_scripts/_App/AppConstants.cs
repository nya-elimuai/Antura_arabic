﻿namespace EA4S
{
    /// <summary>
    /// Container for application-wide static constants.
    /// </summary>
    public static class AppConstants
    {
        public const string AppVersion = "0.12.12x";
        public const string DbSchemeVersion = "1";
        public const bool DebugPanelEnabled = true;
        public const bool UseUnityAnalytics = false;
        public static bool VerboseLogging = true;
        public static bool DebugLogInserts = true;

        // Application details (used by Teacher and maybe other)
        public const float minimumAge = 7;  // refactor: code convention for variables?
        public const float maximumAge = 15;

        public const float minimumStage = 1;
        public const float maximumStage = 6;

        public const float minimumMiniGameScore = -1;
        public const float maximumMiniGameScore = 1;

        public const int minimumMoodValue = 1;
        public const int maximumMoodValue = 5;

        // Resource Paths
        public const string AvatarsResourcesDir = "Images/Avatars/";

    #if UNITY_EDITOR
        public const string UrlGithubRepository = "https://github.com/vgwb/EA4S_Antura_U3D";
        public const string UrlTrello = "https://trello.com/b/ltLndaQI/ea4s-beta";
    #endif
    }
}