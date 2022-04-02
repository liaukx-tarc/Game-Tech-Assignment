using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInput : MonoBehaviour
{
    public GameObject Grid;
    // Start is called before the first frame update
    public void SetWidth(string value)
    {
        Grid.GetComponent<CaveGenerator>().width = int.Parse(value);
    }

    public void SetHeight(string value)
    {
        Grid.GetComponent<CaveGenerator>().height = int.Parse(value);
    }

    public void SetGenerateChance(string value)
    {
        Grid.GetComponent<CaveGenerator>().generateChance = int.Parse(value);
    }

    public void SetBirthLimit(string value)
    {
        Grid.GetComponent<CaveGenerator>().birthLimit = int.Parse(value);
    }

    public void SetDeathLimit(string value)
    {
        Grid.GetComponent<CaveGenerator>().deathLimit = int.Parse(value);
    }

    public void SetRepeatTimes(string value)
    {
        Grid.GetComponent<CaveGenerator>().repeatTimes = int.Parse(value);
    }

    public void SetCavePercentage(string value)
    {
        Grid.GetComponent<CaveGenerator>().cavePercentage = int.Parse(value);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
