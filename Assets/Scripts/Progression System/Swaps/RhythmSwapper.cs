using UnityEngine;
using System.Collections.Generic;
using System;

public class RhythmSwapper : MonoBehaviour {


    public void SwapRhythm(Rhythm newRhythm){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        var noteManager = player.GetComponent<NoteManager>();
        noteManager.rhythm = newRhythm;
        noteManager.UpdateLastSpawnedIndex();
    }
}