using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{

    [Header("UIElements")]
    [SerializeField] private GameObject _abilityIndicator;
    [SerializeField] private Image _abilityIcon;
    [SerializeField] private TMP_Text _abilityNameText;

    private void Start()
    {
        _abilityIndicator.SetActive(false);
        AbilityTracker.AbilitySwitchCallback += UpdateAbilityUI;
        
    }



    private void UpdateAbilityUI()
    {
        Ability ability = AbilityTracker.Singleton.CurrentAbility();

        Debug.Log("Callback invoked");

        if (ability != null) {
            _abilityIcon.sprite = ability.GetAbilityData().Icon;
            _abilityNameText.text = ability.GetAbilityData().AbilityName;
            if (!_abilityIndicator.activeSelf) _abilityIndicator.SetActive(true);

        }

    }
}
