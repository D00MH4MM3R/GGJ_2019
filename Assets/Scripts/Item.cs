using UnityEngine;

public enum ItemType
{
    None,
    Key,
    Knife,
    Shotgun,
    Crowbar,
    BaseballBat
}
    
public class Item : MonoBehaviour
{
    public ItemType m_type;
}
