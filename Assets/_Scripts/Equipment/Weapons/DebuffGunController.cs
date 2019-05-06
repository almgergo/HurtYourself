using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffGunController : FireableGun {
    // Start is called before the first frame update
    void Start () {

    }

    public GameObject DebuffTemplate;

    // Update is called once per frame
    void Update () {

    }

    public override void Fire (EnemyController target) {
        target.AddDebuff (DebuffTemplate);
    }
}