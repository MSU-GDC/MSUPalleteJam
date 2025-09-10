using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// AbilityTracker.cs 
/// John F. 9/9/25
/// 
/// Summary: 
/// Tracks what abilities the player has access to, including any they have equipped.
/// Also exposes callback servers relating to the adding and switching of abilities.
/// Also responsible for using said abilities.
/// 
/// 
/// </summary>
public class AbilityTracker : MonoBehaviour
{
    public static AbilityTracker Singleton;


    private uint _unlockedAbilities; /// ability bitmask

    public delegate void AbilityCallback();


    public static AbilityCallback AbilitySwitchCallback; // callback called whenever abilities are switched
    public static AbilityCallback AbilityAddedCallback;  // callback called whenever a new ability is added

    [Header("Settings / References")]
    [SerializeField] private InputActionAsset _controls;
    [SerializeField] private List<Ability> _abilityList;


    [Header("Debug / Testing")]
    [SerializeField] private List<AbilityID_e> _preUnlockedAbilities;  // fill me with whatever abilities you want to have before game start

    [SerializeReference] private Ability _equippedAbility;


    private Dictionary<AbilityID_e, Ability> _abilityDatabase;

    private InputAction _useAbilityAction;

    private void Awake()
    {
        if(Singleton != this && Singleton != null)
        {
            Debug.LogWarning("Warning! Multiple instances of AbilityTracker Detected, deactivating most recent instance...");
            gameObject.SetActive(false);

            return;
        
        }
        else Singleton = this;


        _useAbilityAction = _controls.FindActionMap("Player").FindAction("UseAbility"); 


    }


    private void Start()
    {
        _unlockedAbilities = 0;

        _abilityDatabase = new Dictionary<AbilityID_e, Ability>();

        foreach (Ability ability in _abilityList)
        {
            _abilityDatabase.Add(ability.GetAbilityData().AbilityID, ability);
        }

#if UNITY_EDITOR
        if (_preUnlockedAbilities == null || _preUnlockedAbilities.Count == 0) return;
        foreach(AbilityID_e ability in _preUnlockedAbilities)
        {
            Debug.Log($"Pre-Unlocking ability: {ability}");
            UnlockAbility(ability); 
        }

#endif // UNITY_EDITOR
    }



    private void Update()
    {
        if (_useAbilityAction.WasPressedThisFrame() && _equippedAbility != null)
        {
            _equippedAbility.Execute();
        }

        if(_useAbilityAction.WasReleasedThisFrame() && _equippedAbility != null)
        {
            _equippedAbility.Cancel();
        }
    }


    public void UnlockAbility(AbilityID_e abilityID)
    {
        if (IsAbilityUnlocked(abilityID)) return; 


        _unlockedAbilities = (uint)abilityID | _unlockedAbilities;

        if(AbilityAddedCallback != null) AbilityAddedCallback.Invoke(); 

        if (_equippedAbility == null) EquipAbility(abilityID);
    }

    public void RemoveAbility(AbilityID_e abilityID)
    {
        _unlockedAbilities = _unlockedAbilities ^ (uint)abilityID;

        if(_equippedAbility.GetAbilityData().AbilityID == abilityID)
        {
            _equippedAbility.Cancel();
            _equippedAbility = null;
        }
    }


    public bool IsAbilityUnlocked(AbilityID_e abilityID)
    {
        return ((uint)abilityID & _unlockedAbilities) != 0; 
    }


    public bool EquipAbility(AbilityID_e abilityID)
    {
        if (!IsAbilityUnlocked(abilityID)) return false;
        if (!IsAbilityInDatabase(abilityID))
        {
            Debug.LogWarning("WARNING, PLAYER ATTEMPTED TO EQUIP A ABILITY THAT IS NOT IN THE DATABASE");
            return false;
        }

        _equippedAbility = _abilityDatabase[abilityID]; 

        if(AbilitySwitchCallback != null) AbilitySwitchCallback.Invoke();

        return true;
    }

    private bool IsAbilityInDatabase(AbilityID_e abilityID)
    {
        return _abilityDatabase.ContainsKey(abilityID);
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
    public string AbilityName; 
}





