using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActOnUI : MonoBehaviour {

    public InputField InputText;
    public Slider Slider;
    public Text Count;


    private string mFileName = "FileSave.gm";       //Use this filenane to save, it will pick save path automatically


    public void	Spawn() {			//Span a new object from prefab
        GameObject	tGO = SaveObject.Create(SaveGame.CurrentVersion,Random.Range(0, SaveObject.PrefabCount));
		Rigidbody2D tRB = tGO.GetComponent<Rigidbody2D> ();
		Vector2	tDirection = new Vector2 (Random.Range (-Slider.value, Slider.value), Random.Range (-Slider.value, Slider.value));	//Random acceleration
		tRB.AddForce (tDirection, ForceMode2D.Impulse);
	}


    public  void    Save() {
        if (!SaveGame.singleton.Save(mFileName)) {
            GameManager.MessageBox(SaveGame.singleton.LastErrorMessage, "Error");
        }
    }

    public void     Load() {
        if(!SaveGame.singleton.Load(mFileName)) {
            GameManager.MessageBox(SaveGame.singleton.LastErrorMessage, "Error");
        }
    }


    public void Clear() {       //Delete all SaveAble objects
        SaveObject[] tSaveArray = FindObjectsOfType<SaveObject>();      //Find Objects which have Save code
        foreach (var tSB in tSaveArray) {
            Destroy(tSB.gameObject);       //Kill them
        }
        GameManager.PanelInputField = "";
    }
}
