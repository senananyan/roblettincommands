using System;
using System.IO;
using System.Linq;
using TaleWorlds.Core;

namespace RoblettinCommands
{

public static class TagManager
{
    private static string BannedTagsPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bannedtags.txt");

    // Load banned tags from file
    private static string[] LoadBannedTags()
    {
        if (!File.Exists(BannedTagsPath)) return Array.Empty<string>();
        return File.ReadAllLines(BannedTagsPath).Select(tag => tag.Trim()).ToArray();
    }

    // Check if a player's name contains any banned tag
    public static bool HasBannedTag(VirtualPlayer player)
    {
        string playerName = player.UserName;
        string[] bannedTags = LoadBannedTags();

        foreach (string tag in bannedTags)
        {
            string pattern = $"[{tag}]";
            if (playerName.Contains(pattern))
            {
                return true;
            }
        }
        return false;
    }
}
}