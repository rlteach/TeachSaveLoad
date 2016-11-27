using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActOnUI : MonoBehaviour {

    public InputField InputText;
    public Slider Slider;
    public Text Count;


    [System.Serializable]           //Must include this to allow Class to save
    public class SaveData {             //Only simple types allowed
        public int Score;        //Score to save
        public string Details;      //When it was saved
        public int Count;        //Save Count
        public SaveData() {        //Set Defaults
            Score = 0;
            Details = "New";
            Count = 0;
        }
    }

    private SaveLoad mSaveLoad;     //Link to SaveLoad Script

    private SaveData mSaveData;

    private string mFileName = "FileSave.gm";       //Use this filenane to save, it will pick save path automatically

    // Use this for initialization
    void Start() {
        mSaveLoad = GetComponent<SaveLoad>();     //Get Access to SaveLoad Code
        mSaveData = new SaveData();             //Create Save Load class to save and load into
        Count.text = string.Format("Save count {0}", mSaveData.Count);
    }


    public void Save() {                           //Copy UI elements to Save Load function and save it
        mSaveData.Details = InputText.text;
        mSaveData.Score = (int)Slider.value;
        mSaveData.Count++;
        if (!mSaveLoad.SaveClass<SaveData>(mSaveData, mFileName)) {
            Debug.Log(mSaveLoad.LastErrorMessage);
        }

    }

    public void Load() {
        mSaveData = mSaveLoad.LoadClass<SaveData>(mFileName);       //Load UI Elements into Save Load function
        if (mSaveData != null) {
            InputText.text = mSaveData.Details;
            Slider.value = (float)mSaveData.Score;
            Count.text = string.Format("Save count {0}", mSaveData.Count);
        } else {
            Debug.Log(mSaveLoad.LastErrorMessage);
        }
    }
}
