using UnityEngine;

[CreateAssetMenu(fileName = "NewRiffRaff", menuName = "RiffRaffs/RiffRaffData")]
public class RiffRaffData : ScriptableObject
{
    public string riffRaffName;
    public Sprite sprite;
    public string genre;
    public string part;
    public string instrumentType;
    public GameObject overWorldSong;
    public GameObject performanceSong;
    [TextArea] public string description;
}