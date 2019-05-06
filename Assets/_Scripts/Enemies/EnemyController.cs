using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
    // Start is called before the first frame update
    void Start () {
        Health = MaxHealth;

        healthSlider = Instantiate (Resources.Load ("Prefabs/HealthSlider") as GameObject, transform.position + healthOffset, Quaternion.identity);
        healthSlider.GetComponent<Slider> ().maxValue = MaxHealth;
        healthSlider.GetComponent<Slider> ().value = Health;
        healthSlider.transform.SetParent (DebuffCanvas.transform);

        healthQueue = new FixedSizedList<HealthHistory> (60);
        healthQueue.Push (new HealthHistory (Health, Time.time));
    }

    public Vector3 healthOffset;
    public GameObject DebuffCanvas;
    public float MaxHealth;

    private GameObject healthSlider;
    private float Health;
    private bool toRemove;

    private FixedSizedList<HealthHistory> healthQueue;

    // Update is called once per frame
    void Update () {

        healthSlider.transform.position = transform.position + healthOffset;
        healthSlider.GetComponent<Slider> ().value = Health;

        DealDotDamage ();

    }

    private void DealDotDamage () {
        foreach (DebuffController debuff in GetDebuffs ()) {
            this.Health -= debuff.DealDamage ();
        }
    }

    void FixedUpdate () {

        if (this.Health < 0) {
            toRemove = true;

        }
        toRemove = this.Health < 0;
        healthQueue.Push (new HealthHistory (Health, Time.time));
    }

    private DebuffController[] GetDebuffs () {
        return gameObject.GetComponentsInChildren<DebuffController> ();
    }

    private DebuffController GetExistingDebuff (DebuffController debuffToCheck) {
        foreach (DebuffController debuff in GetDebuffs ()) {
            if (debuff.debuffName.Equals (debuffToCheck.debuffName)) {
                return debuff;
            }
        }
        return null;
    }

    void LateUpdate () {
        if (toRemove) {
            foreach (DebuffController debuff in GetDebuffs ()) {
                debuff.Destroy ();
            }
            Destroy (gameObject);
        }
    }

    public void AddDebuff (GameObject debuffTemplate) {
        DebuffController templateController = debuffTemplate.GetComponent<DebuffController> ();
        if (templateController != null) {
            DebuffController existingDebuff = GetExistingDebuff (templateController);

            Debug.Log ("existing debuff: " + existingDebuff);
            // if debuff already exists
            if (existingDebuff != null) {
                existingDebuff.Reapply ();
            } else {
                // create the new debuff object
                GameObject newDebuff = Instantiate (debuffTemplate, transform, true);
                DebuffController debuffController = newDebuff.GetComponent<DebuffController> ();
                // set the parent for the debuff 
                newDebuff.transform.SetParent (transform);
                // init it's controller
                debuffController.Init (DebuffCanvas);
            }
        }
    }

    public float DpsTaken () {
        return (this.healthQueue.First ().Health - this.healthQueue.Last ().Health) / (this.healthQueue.Last ().Time - this.healthQueue.First ().Time);
    }
}