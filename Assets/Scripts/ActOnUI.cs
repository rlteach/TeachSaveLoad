using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActOnUI : MonoBehaviour {

    public InputField InputText;
    public Slider Slider;
    public Text Count;
    public GameObject Prefab;


    private string mFileName = "FileSave.gm";       //Use this filenane to save, it will pick save path automatically



    public void	Spawn() {			//Span a new object from prefab
		GameObject	tGO = Instantiate (Prefab);
		Rigidbody2D tRB = tGO.GetComponent<Rigidbody2D> ();
		Vector2	tDirection = new Vector2 (Random.Range (-Slider.value, Slider.value), Random.Range (-Slider.value, Slider.value));	//Random accelleration
		tRB.AddForce (tDirection, ForceMode2D.Impulse);
	}


    public  void    Save() {
        if (!SaveLoad.singleton.SaveGame(mFileName)) {
            GameManager.MessageBox(SaveLoad.singleton.LastErrorMessage, "Error");
        }
    }

    public void     Load() {
        if(!SaveLoad.singleton.LoadGame(mFileName)) {
            GameManager.MessageBox(SaveLoad.singleton.LastErrorMessage, "Error");
        }
    }


    public void Clear() {       //Delete all tagged objects
        GameObject[] tGOArray = GameObject.FindGameObjectsWithTag("SaveThis");      //Find Objects Tagged as SaveThis, these are all the objects we wish to store
        foreach (GameObject tGO in tGOArray) {
            Destroy(tGO);       //Kill them
        }
    }

}
