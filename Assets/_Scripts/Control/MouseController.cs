using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour {
    // Start is called before the first frame update
    void Start () {
        // init guns list
        Guns = new List<FireableGun> ();

        // get all controlled guns
        GameObject[] lst = GameObject.FindGameObjectsWithTag ("ControlledGun");
        foreach (GameObject go in lst) {
            // add debuff guns to the list
            AddGun (go);
        }

        // init selection border
        SelectionBorder = Instantiate (Resources.Load ("Prefabs/SelectionBorder") as GameObject, transform.position + SelectionBorderDisplacement, Quaternion.identity);
        if (Guns.Count > 0) {
            SelectSlot (Guns[0].gameObject.GetComponentInParent<SlotController> ());
        }
        // 

    }

    private void SelectSlot (SlotController slot) {
        SelectionBorder.transform.position = slot.transform.position;
        SelectionBorder.transform.SetParent (slot.transform);
        SelectionBorder.transform.localScale = new Vector3 (1.1f, 1.1f, 1);
        SelectedSlot = slot;
    }

    public GameObject SelectionObject;

    private void AddGun (GameObject go) {
        FireableGun debuffController = go.GetComponent<FireableGun> ();

        if (debuffController) {
            Guns.Add (debuffController);
        }
    }

    private List<FireableGun> Guns;
    private SlotController SelectedSlot;
    private GameObject SelectionBorder;

    private Vector3 SelectionBorderDisplacement = new Vector3 (0, 0, 1);

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown (0)) {
            RaycastHit hit;
            if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100.0f)) {
                Debug.Log ("You selected the " + hit.transform.name); // ensure you picked right object

                EnemyController enemy = hit.transform.GetComponent<EnemyController> ();
                SlotController slot = hit.transform.GetComponent<SlotController> ();

                // if we clicked on an enemy, try to fire the selected gun
                if (enemy != null) {
                    TryFireSelection (enemy);
                } else if (slot != null) {
                    // if we clicked on a gun, select it as the weapon to fire
                    SelectSlot (slot);
                }
            }
        }
    }

    private void TryFireSelection (EnemyController enemy) {
        if (SelectedSlot != null) {
            FireableGun gun = SelectedSlot.GetComponentInChildren<FireableGun> ();
            if (gun != null) {
                gun.Fire (enemy);
            }
        }
    }

    private void fireDebuffGuns (EnemyController enemy) {
        foreach (DebuffGunController debuffGun in Guns) {
            debuffGun.Fire (enemy);
        }
    }
}