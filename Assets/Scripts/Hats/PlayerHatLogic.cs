using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHatLogic : MonoBehaviour
{
    public GameObject[] hats;

    ControlsforPlayer controls;
    private bool switchHat;
    private int currentHatNum;
    private GameObject currentHatObject;
    private bool canSwitchHat = true;

    void Start()
    {
        //sets up all the boring stuff like controls what hat the player is using and disables all but the first hat
        controls = new ControlsforPlayer();
        controls.Enable();
        currentHatNum = 0;
        foreach(GameObject hat in hats)
        {
            hat.SetActive(false);
        }
        hats[currentHatNum].SetActive(true);
    }

    void Update()
    {
        //if the player switches their hat run the hat change corotine so the players cant spam it
        switchHat = controls.Actions.SwitchHat.IsPressed();
        if (switchHat && canSwitchHat)
        {
            canSwitchHat = false;
            StartCoroutine(ChangeHatCooldown());
        }
    }
    //changes the hat to the next listed hat. if its the last hat in the list it goes to the first hat. 
    void ChangeHat()
    {
        hats[currentHatNum].SetActive(false);
        currentHatNum++;
        if(currentHatNum == hats.Length)
        {
            currentHatNum = 0;
        }
        hats[currentHatNum].SetActive(true);
    }
    //changes hat and starts a short cooldown so hat changing is easier to use
    IEnumerator ChangeHatCooldown()
    {
        ChangeHat();
        yield return new WaitForSeconds(0.5f);
        canSwitchHat = true;
    }
}
