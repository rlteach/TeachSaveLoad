using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStatus : MonoBehaviour {

    Text mText;

	// Use this for initialization
	void Start () {
        mText = GetComponent<Text>();
        Debug.Assert(mText, "No Text found");

        mText.text = string.Format("{0:s}\nPlayCount:{1:d}",GameManager.PlayTime,GameManager.PlayCount);
	}
}
