using UnityEngine;

[CreateAssetMenu(fileName = "NewRiffRaff", menuName = "RiffRaffs/RiffRaffData")]
public class RiffRaffData : ScriptableObject
{
    public string riffRaffName;
    public Sprite sprite;
    public string instrumentType;
    //public string ganre
    //public string class (percussion, etc) 
    [TextArea] public string description;
}