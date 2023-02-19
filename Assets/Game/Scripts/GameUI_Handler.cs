using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_Handler : MonoBehaviour
{
    public Slider _healthSlider;
    public Slider _manaSlider;
    public GameObject Player;
    private Character _character;
    private void Awake()
    {
        _character = Player.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        var healthpercent = _character.GetHealthPercent();
        var manapercent = _character.GetManaPercent();
        _healthSlider.value = healthpercent;
        _manaSlider.value = manapercent;
    }
}
