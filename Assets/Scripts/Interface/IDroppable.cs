using UnityEngine;

public interface IDroppable 
{
    void Construct(bool active = true);
    void Active(); 
    void MoveToPlayer(); 
}
public enum ItemType { None, Coins, Bomb }
