using UnityEngine;

/// <summary>
/// Ability.cs
/// John F. 9/10/25
/// 
/// Summary: 
/// Interface for implimenting abilities. 
/// 
/// </summary>
public abstract class Ability : MonoBehaviour
{
    public abstract void Execute();


    public abstract void Cancel(); 


    public abstract AbilityData_t GetAbilityData(); 

    public virtual bool IsHoldAbility() { return false; }

    public virtual bool IsToggleAbility() { return false; }

    public virtual bool CanToggle() {  return false; }
}

