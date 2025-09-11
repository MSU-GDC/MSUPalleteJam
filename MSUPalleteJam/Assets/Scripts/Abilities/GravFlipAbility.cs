using UnityEngine;

public class GravFlipAbility : Ability
{
    [SerializeField] private AbilityData_t _abilityData; 

    public override void Execute()
    {
        if (AbilityTracker.Singleton.IsAbilityUnlocked(_abilityData.AbilityID)){
            Player.Singleton.Controller.SetGravityDirection(-1); 
        } 
    }

    public override void Cancel()
    {
        if (AbilityTracker.Singleton.IsAbilityUnlocked(_abilityData.AbilityID))
        {
            Player.Singleton.Controller.SetGravityDirection(1);
        }
    }

    public override AbilityData_t GetAbilityData()
    {
        return _abilityData;
    }

    public override bool IsToggleAbility()
    {
        return true;
    }

    public override bool CanToggle()
    {
        return Player.Singleton.Controller.IsGrounded() && Player.Singleton.Controller.IsPlayerYMovement() == false;
    }
}
