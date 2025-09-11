using UnityEngine;

public class DashAbility : Ability
{
    [SerializeField] private AbilityData_t _abilityData;

    [SerializeField] private bool _isCooldown; 


    
    public override void Cancel()
    {
        return;
    }

    public override void Execute()
    {
        if (_isCooldown) return; 

        Player.Singleton.Controller.QueueDash();
        Debug.Log("Attempting to dash");

        _isCooldown = true;
        Invoke(nameof(EndCooldown), _abilityData.CooldownTimeSeconds);
    }



    private void EndCooldown()
    {
        _isCooldown = false;
    }

    public override AbilityData_t GetAbilityData()
    {
        return _abilityData;
    }


    public override bool IsHoldAbility()
    {
        return false;
    }

    public override bool IsCooldown()
    {
        return _isCooldown;
    }
}
