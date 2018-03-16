using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class StepStateExtensions  {

    // Use this for initialization
    public static void SetStepState(this PhotonPlayer player, bool isFinish)
    {
        Hashtable score = new Hashtable();  // using PUN's implementation of Hashtable
        score["StepState"] = isFinish;

        player.SetCustomProperties(score);  // this locally sets the score and will sync it in-game asap.
    }

    public static bool GetStepState(this PhotonPlayer player)
    {
        object isFinish;
        if (player.CustomProperties.TryGetValue("StepState", out isFinish))
        {
            return (bool)isFinish;
        }
        return false;
    }
}
