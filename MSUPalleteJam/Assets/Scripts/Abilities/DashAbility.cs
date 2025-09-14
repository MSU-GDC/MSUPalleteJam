using UnityEngine;

public class DashAbility : Ability
{
    [SerializeField] private AbilityData_t _abilityData;
    [SerializeField] private bool _isCooldown;
    [SerializeField] public ParticleSystem _dashEffect;
    [SerializeField] private AbilityTracker _abilityTracker;

    private void Awake()
    {
        // Only assign if not set in the inspector
        if (_dashEffect == null)
        {
            GameObject dashParticleObj = GameObject.Find("dashParticle");
            if (dashParticleObj != null)
            {
                _dashEffect = dashParticleObj.GetComponent<ParticleSystem>();
            }
            else
            {
                Debug.LogWarning("DashAbility: No GameObject named 'dashParticle' found in the scene.");
            }
        }
    }

    public override void Cancel()
    {
        return;
    }

    public override void Execute()
    {
        if (_isCooldown) return; 

        if (_dashEffect != null)
        {
            _dashEffect.Play();
        }
        else
        {
            Debug.LogWarning("DashAbility: _dashEffect is not assigned.");
        }

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
