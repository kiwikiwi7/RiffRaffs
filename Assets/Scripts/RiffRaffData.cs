using UnityEngine;

[CreateAssetMenu(fileName = "NewRiffRaff", menuName = "RiffRaffs/RiffRaffData")]
public class RiffRaffData : ScriptableObject
{
    public string riffRaffName;
    public Sprite sprite;
    public string instrumentType;
    [TextArea] public string description;
}