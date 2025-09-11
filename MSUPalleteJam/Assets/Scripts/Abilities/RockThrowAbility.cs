using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class RockThrowAbility : Ability
{
    [SerializeField] private AbilityData_t _abilityData;
    [SerializeField] private GameObject _rockPrefab;


    [SerializeField] private float _baseProjectileForce = 20f;

    [SerializeField] private float _maxProjectileForce = 40f;


    [SerializeField] private float _chargeTimeSeconds = 1.5f;

    [SerializeField] private bool _isCooldown;


    private float _cChargeTime; 


    private bool _hasChargedRock;

    private float _cProjForce;

    public override void Cancel()
    {
        // THROW DA ROCK
        if (!_isCooldown && !_hasChargedRock)
        {
            _isCooldown = true;
            Invoke(nameof(EndCooldown), _abilityData.CooldownTimeSeconds);
        }
        if (!_hasChargedRock) return;





        Vector2 mousePosInWorld = Player.Singleton.MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector3 forceDir = (mousePosInWorld - (Vector2)Player.Singleton.ProjectileSpawnPt.position).normalized;


        GameObject rock = GameObject.Instantiate(_rockPrefab, Player.Singleton.ProjectileSpawnPt.position, Quaternion.identity);

        Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();

        rb.AddForce(forceDir * _cProjForce, ForceMode2D.Impulse);

        _hasChargedRock = false;

        _cProjForce = 0;

        _cChargeTime = 0;




    }

    public override void Execute()
    {
        // CHARGE DA ROCK

        if (_isCooldown) return; 

        _hasChargedRock = true;

        _cChargeTime = Mathf.Clamp(_cChargeTime + Time.deltaTime,0, _chargeTimeSeconds);


        _cProjForce = _baseProjectileForce + _cChargeTime * (_maxProjectileForce - _baseProjectileForce) / _chargeTimeSeconds;

        _cProjForce = Mathf.Clamp(_cProjForce, _baseProjectileForce, _maxProjectileForce);



        Debug.Log($"{_cProjForce}"); 
        if (_cChargeTime >= _chargeTimeSeconds) Debug.Log("ROCK IS FULLY CHARGED!!!");

    }

    public override AbilityData_t GetAbilityData()
    {
        return _abilityData;
    }


    private void OnDrawGizmos()
    {
        if (Player.Singleton == null) return;
        Vector2 mousePosInWorld = Player.Singleton.MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Gizmos.DrawWireSphere((mousePosInWorld), 1.0f);
    }

    public override bool IsHoldAbility()
    {
        return true;
    }

    public override bool IsCooldown()
    {
        return _isCooldown;
    }

    private void EndCooldown() => _isCooldown = false;
}
