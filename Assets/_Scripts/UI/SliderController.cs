using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {
    public GameObject fillObject;

    public void SetColor (Color color) {
        Debug.Log ("old: " + this.fillObject.GetComponent<Image> ().color + ", fill color changed: " + color);
        this.fillObject.GetComponent<Image> ().color = color;
    }
}