using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{

    [Header("UIElements")]
    [SerializeField] private GameObject _abilityIndicator;

    [SerializeField] private GameObject _noAbilityVariation; 


    [SerializeField] private List<GameObject> _uiVars;
    [SerializeField] private List<GameObject> _uiIcons;

    [SerializeField] private List<AbilityID_e> _associatedIds;


    private Dictionary<AbilityID_e, GameObject> _uiVariationLookup;
    private Dictionary<AbilityID_e, GameObject> _uiIconLookup;


    private GameObject _previousVariation;




    private void Start()
    {
        //_abilityIndicator.SetActive(false);

        _uiVariationLookup = new Dictionary<AbilityID_e, GameObject>();
        _uiIconLookup = new Dictionary<AbilityID_e, GameObject>();

        for(int i = 0; i < _associatedIds.Count; i++)
        {
            _uiVariationLookup.Add(_associatedIds[i],_uiVars[i]);

            _uiVars[i].SetActive(false);
            _uiIconLookup.Add(_associatedIds[i], _uiIcons[i]);
            _uiIcons[i].SetActive(false);
            
        }

        _previousVariation = _noAbilityVariation;

        AbilityTracker.AbilitySwitchCallback += UpdateCurrentAbility;
        AbilityTracker.AbilityAddedCallback += AddAbility;
        
    }

    public void ActivateAbilityIndicator()
    {
        _abilityIndicator.SetActive(true);
    }

    private void AddAbility()
    {
        
        AbilityID_e id = AbilityTracker.Singleton.GetLastUnlockedAbility(); 

        GameObject icon = _uiIconLookup[id];

        icon.SetActive(true); 
    }

    private void UpdateCurrentAbility()
    {
        AbilityID_e id = AbilityTracker.Singleton.CurrentAbility().GetAbilityData().AbilityID;

        GameObject nextVar = _uiVariationLookup[id];

        if (GameObject.Equals(nextVar, _previousVariation)) return;
        else
        {
            _previousVariation.SetActive(false);
            nextVar.SetActive(true);

            _previousVariation = nextVar; 
        }

    }
}
