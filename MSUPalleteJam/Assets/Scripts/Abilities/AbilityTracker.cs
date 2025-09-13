using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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
/// TODO:
/// - Add Ability Selection
/// </summary>
public class AbilityTracker : MonoBehaviour
{
    public static AbilityTracker Singleton;


    private uint _unlockedAbilities; /// ability bitmask

    public delegate void AbilityCallback();


    public static AbilityCallback AbilitySwitchCallback; // callback called whenever abilities are switched
    public static AbilityCallback AbilityAddedCallback;  // callback called whenever a new ability is added
    
    public UnityEvent OnAbilitySwitch;


    [Header("Settings / References")]
    [SerializeField] private InputActionAsset _controls;
    [SerializeField] private List<Ability> _abilityList;
    [SerializeField] private Animator _animator;



    [Header("Debug / Testing")]
    [SerializeField] private List<AbilityID_e> _preUnlockedAbilities;  // fill me with whatever abilities you want to have before game start

    [SerializeReference] private Ability _equippedAbility;


    private Dictionary<AbilityID_e, Ability> _abilityDatabase;

    private InputAction _useAbilityAction;
    private InputAction _selectAbilityAction;

    private bool _abilityToggle;  

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

        _selectAbilityAction = _controls.FindActionMap("Player").FindAction("SelectAbility");


        _selectAbilityAction.performed += SwitchAbility;
        


    }

    private IEnumerator AnimatorWatcher()
    {
        while (true)
        {
            if (CanExecuteAbility())
            {
                _animator.SetTrigger("IsAbility");
                yield return new WaitUntil(() => CanCancelAbility());
                _animator.ResetTrigger("IsAbility");
            }
            yield return new WaitForEndOfFrame(); 
        }
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
        foreach (AbilityID_e ability in _preUnlockedAbilities)
        {
            Debug.Log($"Pre-Unlocking ability: {ability}");
            UnlockAbility(ability);
        }

#endif // UNITY_EDITOR
    }


    private void SwitchAbility(InputAction.CallbackContext ctx)
    {
        float value = ctx.ReadValue<float>();

        Debug.Log($"selected ability {value}");


        switch (value)
        {
            case 1:

                EquipAbility(AbilityID_e.DASH);
                break;
            case 2:

                EquipAbility(AbilityID_e.WALLBLAST);
                break;
            case 3:

                EquipAbility(AbilityID_e.ROCKTHROW);
                break;
            case 4:
                EquipAbility(AbilityID_e.GRAVFLIP);
                break;
            default:
                Debug.LogWarning($"Warning, invalid key pressed with value {value}");
                break; 
        }
    }



    private void Update()
    {
        if (_equippedAbility == null || _equippedAbility.IsCooldown()) return; 

        if (CanExecuteAbility())
        {
            _equippedAbility.Execute();
        }
        if (CanCancelAbility())
        {
            _equippedAbility.Cancel();
        }
        

        
    }

    private IEnumerator ToggleRoutine()
    {
        Ability equipped = _equippedAbility;

        yield return new WaitUntil(() => _useAbilityAction.WasReleasedThisFrame());
        yield return new WaitForSecondsRealtime(0.125f); 
        yield return new WaitUntil(() =>  _useAbilityAction.WasPressedThisFrame());

        yield return new WaitUntil(() => equipped.CanToggle());
        _abilityToggle = false;
    }

    private bool CanExecuteAbility()
    {

        bool execute = false; 

        if (_equippedAbility.IsHoldAbility())
        {
            execute = _useAbilityAction.IsPressed();
        }
        else if (_equippedAbility.IsToggleAbility() && !_abilityToggle && _equippedAbility.CanToggle())
        {
            execute = _useAbilityAction.WasPressedThisFrame();
            _abilityToggle = execute;

            if(_abilityToggle) StartCoroutine(ToggleRoutine());
        }
        else if(!_equippedAbility.IsHoldAbility() && !_equippedAbility.IsToggleAbility())
        {
            execute = _useAbilityAction.WasPressedThisFrame();
        }


        return (_equippedAbility.IsToggleAbility() && _abilityToggle) || execute;
    }

    private bool CanCancelAbility()
    {


        if (_equippedAbility.IsToggleAbility() && !_abilityToggle)
        {
            return true; 
        }



        if (_equippedAbility.IsHoldAbility() && _useAbilityAction.IsPressed())
        {
            return false;
        }
        else if (_equippedAbility.IsHoldAbility())
        {
            return true;
        }
        else if (_equippedAbility.IsHoldAbility() == false && _useAbilityAction.WasReleasedThisFrame())   // there might be a bug here where the player is not executing
        {                                                                                                 // any ability but logically the game still thinks that they are
            return true;
        }
        
        
        
        return false; 
        

    
    }

    public void UnlockAbility(int abilityID)
    {
        UnlockAbility((AbilityID_e)abilityID);
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

        if (_equippedAbility != null && _equippedAbility.GetAbilityData().AbilityID == abilityID) return false; 
        //else if(_equippedAbility != null)
        //{
        //    if (_equippedAbility.IsToggleAbility())
        //    {
        //        _equippedAbility.CancelWithGrace();
        //        _abilityToggle = false;
        //    }
        //    _equippedAbility.Cancel();  // cancel the previous ability

        //}


        _equippedAbility = _abilityDatabase[abilityID];

        Debug.Log($"Equipped Ability {abilityID}");

        if(AbilitySwitchCallback != null) AbilitySwitchCallback.Invoke();
        if (OnAbilitySwitch != null) OnAbilitySwitch.Invoke();
        
        return true;
    }

    private bool IsAbilityInDatabase(AbilityID_e abilityID)
    {
        return _abilityDatabase.ContainsKey(abilityID);
    }


    public Ability CurrentAbility()
    {
        return _equippedAbility; 
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
    public float CooldownTimeSeconds;

    public Sprite Icon;

}





