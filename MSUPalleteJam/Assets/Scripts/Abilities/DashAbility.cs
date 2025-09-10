using UnityEngine;

public class DashAbility : Ability
{
    [SerializeField] private AbilityData_t _abilityData; 

    public override void Cancel()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {
        
    }

    public override AbilityData_t GetAbilityData()
    {
        return _abilityData;
    }


    public override bool IsHoldAbility()
    {
        return false;
    }

    
}
