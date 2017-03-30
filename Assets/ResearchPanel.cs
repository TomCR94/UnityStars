using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResearchPanel : MonoBehaviour
{
    public GameGameObject game;
    public int playerIndex;
    public Text Energy, Weapons, Propulsion, Construction, Electronics, Biotechnology;

    public Dropdown nextField;

    // Use this for initialization
    void Start ()
    {
        Energy.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getEnergy().ToString();
        Weapons.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getWeapons().ToString();
        Propulsion.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getPropulsion().ToString();
        Construction.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getConstruction().ToString();
        Electronics.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getElectronics().ToString();
        Biotechnology.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getBiotechnology().ToString();
        nextField.value = (int)game.getGame().getPlayers()[playerIndex].getNextField();
	}

    private void Update()
    {
        Energy.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getEnergy().ToString();
        Weapons.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getWeapons().ToString();
        Propulsion.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getPropulsion().ToString();
        Construction.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getConstruction().ToString();
        Electronics.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getElectronics().ToString();
        Biotechnology.text = game.getGame().getPlayers()[playerIndex].getTechLevels().getBiotechnology().ToString();
    }

    public void setNextField(int i)
    {
        game.getGame().getPlayers()[playerIndex].setNextResearchField((NextResearchField)i);
    }
}
