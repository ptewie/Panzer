using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PowerupManager : MonoBehaviour
{
    public List<Powerup> powerups = new List<Powerup>(); //list of all powerips on object
    public List<Powerup> powerupsToRemove = new List<Powerup>(); //cue of powerups to remove from object
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DecrementPowerupTimers();
    }

    private void LateUpdate()
    {
        ApplyRemovePowerupsQueue(); //need this to remove powerups from out list after Update
    }

    // adds powerup 
    public void Add(Powerup powerupToAdd)
    {
        powerupToAdd.Apply(this);
        if (!powerupToAdd.isPermanent)
        {
            powerups.Add(powerupToAdd);
        }
    }

    // removes powerup
    public void Remove(Powerup powerupToRemove)
    {
        powerupToRemove.Remove(this);
        powerups.Remove(powerupToRemove);
    }

    public void DecrementPowerupTimers()
    {
        //  going through and decrementing the timers by putting each object into the "powerup" varaible and looping it 
        foreach (Powerup powerup in powerups)
        {
            // subtract the frame draw from the duration of the powerup
            powerup.duration -= Time.deltaTime;
            // if time's up, we remove that sucker
            if (powerup.duration <= 0)
            {
                powerupsToRemove.Add(powerup);
            }
        }
    }

    private void ApplyRemovePowerupsQueue()
    {
        // okay now remove the powerups in our list
        foreach (Powerup powerup in powerupsToRemove)
        {
            powerups.Remove(powerup);
        }
        // reset the list
        powerupsToRemove.Clear();
    }


}