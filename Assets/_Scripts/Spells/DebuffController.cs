using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffController : MonoBehaviour {
    // Start is called before the first frame update
    public void Init (GameObject DebuffCanvas) {
        this.debuffCanvas = DebuffCanvas;

        offsetPosition = new Vector3 (offset, 0, 0);
        durationSliderObject = Instantiate (DurationSliderTemplate, DebuffCanvas.transform.position + offsetPosition, Quaternion.identity);
        durationSliderObject.transform.SetParent (DebuffCanvas.transform);
        durationSliderObject.GetComponent<SliderController> ().SetColor (sliderColor);

        damagePerTick = totalDamage * tickPeriod / duration;
        nextHit = Time.time + tickPeriod;
        expirationTime = Time.time + duration;

        durationSlider = this.durationSliderObject.GetComponent<Slider> ();
        durationSlider.maxValue = duration;
    }

    public Color sliderColor;
    public float duration;
    public float tickPeriod;
    public float totalDamage;
    public float offset;
    public string debuffName;

    private float damagePerTick;
    private float nextHit;
    private float expirationTime = float.PositiveInfinity;

    public GameObject DurationSliderTemplate;
    private GameObject debuffCanvas;
    private GameObject durationSliderObject;
    private Slider durationSlider;
    private Vector3 offsetPosition;

    // Update is called once per frame
    void Update () {
        durationSlider.transform.position = debuffCanvas.transform.position + offsetPosition;
        durationSlider.value = expirationTime - Time.time;
    }

    void LateUpdate () {
        if (this.hasExpired ()) {
            this.Destroy ();
        }
    }

    public float DealDamage () {
        // if cooldown is finished, we return our damage / tick
        if (Time.time > nextHit) {
            nextHit = Time.time + tickPeriod;
            return this.damagePerTick;
        }
        // otherwise we don't deal damage
        else return 0;
    }

    public bool hasExpired () {
        return this.expirationTime < Time.time;
    }

    public void Reapply () {
        expirationTime = Mathf.Min (
            CombatConstants.DEBUFF_MAX_DURATION_MULTIPLIER * duration + Time.time,
            expirationTime + duration
        );
        durationSlider.maxValue = expirationTime - Time.time;

    }

    public void Destroy () {
        Destroy (durationSliderObject);
        Destroy (gameObject);
    }
}