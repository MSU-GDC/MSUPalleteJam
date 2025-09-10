using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AbilityTracker.cs 
/// John F. 9/9/25
/// 
/// Summary: 
/// Tracks what abilities the player has access to, including any they have equipped. 
/// Also exposes callback servers relating to the adding and switching of abilities.
/// 
/// 
/// 
/// </summary>
public class AbilityTracker : MonoBehaviour
{
    private uint _unlockedAbilities; /// ability bitmask

    private AbilityID_e _currentAbility;

    public AbilityTracker Singleton;

    private Dictionary<AbilityID_e, AbilityData_t> _abilityDatabase;


    public delegate void AbilityCallback();


    public static AbilityCallback AbilitySwitchCallback; // callback called whenever abilities are switched
    public static AbilityCallback AbilityAddedCallback;  // callback called whenever a new ability is added



    


    private void Awake()
    {
        if(Singleton != null)
        {
            Debug.LogWarning("Warning! Multiple instances of AbilityTracker Detected, deleting most recent instance...");
            Destroy(gameObject);
        }
        else Singleton = this;
    }


    private void Start()
    {
        _unlockedAbilities = 0;
    }

    public void UnlockAbility(AbilityID_e abilityID)
    {
        if (IsAbilityUnlocked(abilityID)) return; 


        _unlockedAbilities = (uint)abilityID | _unlockedAbilities;

        AbilityAddedCallback.Invoke(); 

        if (_currentAbility == 0) EquipAbility(abilityID);
    }

    public void RemoveAbility(AbilityID_e abilityID)
    {
        _unlockedAbilities = _unlockedAbilities ^ (uint)abilityID;
    }


    public bool IsAbilityUnlocked(AbilityID_e abilityID)
    {
        return ((uint)abilityID & _unlockedAbilities) != 0; 
    }


    public bool EquipAbility(AbilityID_e abilityID)
    {
        if (!IsAbilityUnlocked(abilityID)) return false;

        _currentAbility = abilityID;
        return true;
    }
}

public enum AbilityID_e
{
    NOID = 0x0000_0000,
    DASH = 0x0000_0001,
    GRAVFLIP = 0x0000_0010,
    WALLBLAST = 0x0000_0100,
    ROCKTHROW = 0x0000_1000
}



[System.Serializable]
public struct AbilityData_t
{
    public AbilityID_e AbilityID;
    public Color AssociatedColor; 
}





