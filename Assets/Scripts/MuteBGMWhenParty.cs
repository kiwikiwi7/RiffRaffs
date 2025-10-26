using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MuteWhenPartyPresent : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (PartyManager.Instance == null)
            return;

        // Mute if there’s anyone in the party
        bool hasPartyMembers = PartyManager.Instance.partyMembers.Count > 0;
        audioSource.mute = hasPartyMembers;
    }
}
