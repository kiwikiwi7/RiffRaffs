using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RiffRaffMultiplierManager : MonoBehaviour
{
    public static RiffRaffMultiplierManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Calculates the total score multiplier based on the current party's Parts and Genres.
    /// </summary>
    public float GetPartyMultiplier()
    {
        var party = PartyManager.Instance?.partyMembers;
        if (party == null || party.Count == 0)
            return 1f; // no party = no multiplier

        // ---- PART MULTIPLIER ----
        HashSet<string> uniqueParts = new HashSet<string>();
        foreach (var riff in party)
        {
            if (!string.IsNullOrEmpty(riff.part))
                uniqueParts.Add(riff.part);
        }
        float partMultiplier = uniqueParts.Count; // x1 per unique part (up to 4x)

        // ---- GENRE MULTIPLIER ----
        Dictionary<string, int> genreCounts = new Dictionary<string, int>();
        foreach (var riff in party)
        {
            if (string.IsNullOrEmpty(riff.genre))
                continue;

            if (!genreCounts.ContainsKey(riff.genre))
                genreCounts[riff.genre] = 0;

            genreCounts[riff.genre]++;
        }

        // +0.5 for each *duplicate* genre (every extra riffraff with the same genre)
        float genreMultiplier = 1f;
        foreach (var count in genreCounts.Values)
        {
            if (count > 1)
                genreMultiplier += 0.5f * (count - 1);
        }

        // ---- FINAL MULTIPLIER ----
        float totalMultiplier = partMultiplier * genreMultiplier;
        return Mathf.Max(1f, totalMultiplier);
    }
}
