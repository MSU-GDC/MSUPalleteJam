using UnityEngine;
using UnityEngine.Rendering;

public class BombAbility : Ability
{
    [SerializeField] private AbilityData_t _data;

    [SerializeField] private bool _isCooldown;


    [SerializeField] private GameObject _bombPrefab; 

    public override void Cancel()
    {
        return; 
    }

    public override void Execute()
    {
        if (!_isCooldown && AbilityTracker.Singleton.IsAbilityUnlocked(_data.AbilityID))
        {
            GameObject.Instantiate(_bombPrefab, Player.Singleton.transform.position, Quaternion.identity);
            _isCooldown = true;

            Player.Singleton.SoundController.PlayAbilitySound(SoundID_e.BombPlace,false,true); 

            Invoke(nameof(EndCooldown), _data.CooldownTimeSeconds);
        }

    }

    public override AbilityData_t GetAbilityData()
    {
        return _data; 
    }

    public override bool IsCooldown()
    {
        return _isCooldown; 
    }


    private void EndCooldown()
    {
        _isCooldown = false;
    }
}
