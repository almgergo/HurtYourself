using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update
    void Start () {

    }
    public Text Dps;
    public float MaxHealth;
    public float MaxMana;

    private float health;
    private float mana;

    // Update is called once per frame
    void Update () {
        CalculateDps ();
    }

    private void CalculateDps () {
        float totalDps = 0;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
        foreach (GameObject enemy in enemies) {
            totalDps += enemy.GetComponent<EnemyController> ().DpsTaken ();
        }
        Dps.text = String.Format ("Dps: {0:0}", totalDps);;
    }
}