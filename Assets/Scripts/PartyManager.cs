using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;

    [Header("References")]
    public Transform player;
    public GameObject riffRaffFollowerPrefab;

    [HideInInspector] public List<RiffRaffData> partyMembers = new();

    private readonly List<GameObject> activeFollowers = new();

    // Preloaded song instances
    private readonly Dictionary<RiffRaffData, GameObject> preloadedOverworldSongs = new();
    private readonly Dictionary<RiffRaffData, GameObject> preloadedPerformanceSongs = new();

    private bool isInPerformance = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        PreloadAllSongs();
    }

    private void PreloadAllSongs()
    {
        // You can replace this with your own riffraff registry
        var allRiffs = Resources.LoadAll<RiffRaffData>("RiffRaffs");

        foreach (var riff in allRiffs)
        {
            // --- Overworld Song ---
            if (riff.overWorldSong != null && !preloadedOverworldSongs.ContainsKey(riff))
            {
                var overworldObj = Instantiate(riff.overWorldSong, transform);
                overworldObj.name = $"{riff.riffRaffName}_OverworldSong";
                overworldObj.SetActive(false);
                preloadedOverworldSongs[riff] = overworldObj;
            }

            // --- Performance Song ---
            if (riff.performanceSong != null && !preloadedPerformanceSongs.ContainsKey(riff))
            {
                var performanceObj = Instantiate(riff.performanceSong, transform);
                performanceObj.name = $"{riff.riffRaffName}_PerformanceSong";
                performanceObj.SetActive(false);
                preloadedPerformanceSongs[riff] = performanceObj;
            }
        }
    }

    public void SetParty(List<RiffRaffData> newParty)
    {
        // Stop previous follower spawners
        foreach (var follower in activeFollowers)
        {
            var spawner = follower.GetComponent<MusicNoteSpawner>();
            if (spawner != null) spawner.StopSpawning();
        }

        partyMembers = newParty;
        RefreshPartyFollowers();

        // Start spawners for new followers if in overworld
        if (!isInPerformance)
        {
            foreach (var follower in activeFollowers)
            {
                var spawner = follower.GetComponent<MusicNoteSpawner>();
                if (spawner != null) spawner.StartSpawning();
            }
        }

        // Refresh songs
        if (isInPerformance)
            RefreshPerformanceSongs();
        else
            RefreshOverworldSongs();
    }

    public void RefreshPartyFollowers()
    {
        foreach (var follower in activeFollowers)
            Destroy(follower);
        activeFollowers.Clear();

        Transform followTarget = player;

        foreach (var riffRaff in partyMembers)
        {
            GameObject followerObj = Instantiate(riffRaffFollowerPrefab, player.position, Quaternion.identity);
            followerObj.name = riffRaff.riffRaffName;

            var sr = followerObj.GetComponent<SpriteRenderer>();
            if (sr != null && riffRaff.sprite != null)
                sr.sprite = riffRaff.sprite;

            var follower = followerObj.GetComponent<PartyFollower>();
            if (follower != null)
                follower.followTarget = followTarget;

            followTarget = followerObj.transform;
            activeFollowers.Add(followerObj);
        }
    }

    public List<GameObject> GetActiveFollowers()
    {
        return activeFollowers;
    }

    // --- SONG HANDLING ---

    public void SwitchToPerformanceMode(bool isPerformance)
    {
        isInPerformance = isPerformance;

        if (isPerformance)
        {
            StopAllOverworldSongs();
            RefreshPerformanceSongs();
        }
        else
        {
            StopAllPerformanceSongs();
            RefreshOverworldSongs();

            // Stop all spawners if returning to overworld
            foreach (var follower in activeFollowers)
            {
                var spawner = follower.GetComponent<MusicNoteSpawner>();
                //if (spawner != null) spawner.StopSpawning();
            }
        }
    }

    private void StopAllOverworldSongs()
    {
        foreach (var song in preloadedOverworldSongs.Values)
        {
            var audio = song.GetComponent<AudioSource>();
            if (audio != null) audio.Stop();
            song.SetActive(false);
        }

    }

    private void StopAllPerformanceSongs()
    {
        foreach (var song in preloadedPerformanceSongs.Values)
        {
            var audio = song.GetComponent<AudioSource>();
            if (audio != null) audio.Stop();
            song.SetActive(false);
        }
    }

    private void RefreshOverworldSongs()
    {
        StopAllOverworldSongs();

        foreach (var riff in partyMembers)
        {
            if (!preloadedOverworldSongs.ContainsKey(riff))
                continue;

            var songObj = preloadedOverworldSongs[riff];
            songObj.SetActive(true);

            var audio = songObj.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.time = 0f;
                audio.loop = true;
                audio.Play();
            }
        }
        // Start note spawners for visual flair in overworld
        foreach (var follower in activeFollowers)
        {
            var spawner = follower.GetComponent<MusicNoteSpawner>();
            if (spawner != null) spawner.StartSpawning();
        }
    }

    private void RefreshPerformanceSongs()
    {
        StopAllPerformanceSongs();

        foreach (var riff in partyMembers)
        {
            if (!preloadedPerformanceSongs.ContainsKey(riff))
                continue;

            var songObj = preloadedPerformanceSongs[riff];
            songObj.SetActive(true);

            var audio = songObj.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.time = 0f;
                audio.loop = true;
                audio.Play();
            }
        }
        // Start performance note spawners
        foreach (var follower in activeFollowers)
        {
            var spawner = follower.GetComponent<MusicNoteSpawner>();
            if (spawner != null)
            {
                spawner.StartSpawning();
            }
        }
    }
}
