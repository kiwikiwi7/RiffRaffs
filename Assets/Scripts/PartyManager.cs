using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;

    [Header("References")]
    public Transform player;                      // The player to follow
    public GameObject riffRaffFollowerPrefab;     // Prefab for visual followers

    [HideInInspector] public List<RiffRaffData> partyMembers = new();
    private readonly List<GameObject> activeFollowers = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void SetParty(List<RiffRaffData> newParty)
    {
        partyMembers = newParty;
        RefreshPartyFollowers();
    }

    public void RefreshPartyFollowers()
    {
        // Clear old followers
        foreach (var follower in activeFollowers)
            Destroy(follower);
        activeFollowers.Clear();

        Transform followTarget = player;

        // Spawn each follower behind the last
        foreach (var riffRaff in partyMembers)
        {
            GameObject followerObj = Instantiate(riffRaffFollowerPrefab, player.position, Quaternion.identity);
            followerObj.name = riffRaff.riffRaffName;

            // Set sprite
            var sr = followerObj.GetComponent<SpriteRenderer>();
            if (sr != null && riffRaff.sprite != null)
                sr.sprite = riffRaff.sprite;

            // Set follow target
            var follower = followerObj.GetComponent<PartyFollower>();
            if (follower != null)
                follower.followTarget = followTarget;

            followTarget = followerObj.transform;
            activeFollowers.Add(followerObj);
        }
    }
}
