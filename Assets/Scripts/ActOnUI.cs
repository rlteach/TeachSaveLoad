using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActOnUI : MonoBehaviour {

    public InputField InputText;
    public Slider Slider;
    public Text Count;

	public	GameObject Prefab;

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

	[System.Serializable]           //Must include this to allow Class to save
	public	class MiniGO {
		public	Vector2		mPosition;
		public	float		mRotation;
		public	Vector2		mVelocity;
		public	float		mAngularVelocity;
		public	MiniGO() {
			mPosition=Vector2.zero;
			mRotation=0f;
			mVelocity=Vector2.zero;
			mAngularVelocity=0f;
		}
	}

	[System.Serializable]           //Must include this to allow Class to save
	public class SaveGame {
		public	int				Version;
		public	SaveData		Header;
		public	List<MiniGO>	ListGO;
		public	SaveGame() {
			Version=1;
			Header=new SaveData();
			ListGO=new List<MiniGO>();
		}
	}

    private SaveLoad mSaveLoad;     //Link to SaveLoad Script

    private string mFileName = "FileSave.gm";       //Use this filenane to save, it will pick save path automatically

    // Use this for initialization
    void Start() {
        mSaveLoad = GetComponent<SaveLoad>();     //Get Access to SaveLoad Code
        Count.text = "Type here";
    }


	public void Clear() {
		GameObject[]	tGOArray = GameObject.FindGameObjectsWithTag ("SaveThis");
		foreach (GameObject tGO in tGOArray) {
			Destroy (tGO);
		}
	}

	public	void	SaveState() {
		GameObject[]	tGOArray = GameObject.FindGameObjectsWithTag ("SaveThis");
		SaveGame	tSaveGame = new SaveGame ();
		foreach (GameObject tGO in tGOArray) {
			Rigidbody2D tRB = tGO.GetComponent<Rigidbody2D> ();
			MiniGO tmGO = new MiniGO ();
			tmGO.mPosition = tRB.position;
			tmGO.mRotation = tRB.rotation;
			tmGO.mVelocity = tRB.velocity;
			tmGO.mAngularVelocity = tRB.angularVelocity;
			tSaveGame.ListGO.Add (tmGO);
		}
		tSaveGame.Header.Details = InputText.text;
		tSaveGame.Header.Score = (int)Slider.value;
		tSaveGame.Header.Count=tSaveGame.ListGO.Count;
		if (mSaveLoad.SaveClass<SaveGame> (tSaveGame, mFileName)) {
			Debug.Log ("Saved");
		} else {
			Debug.Log (mSaveLoad.LastErrorMessage);
		}
	}

	public	void	LoadState() {
		SaveGame tSaveGame=mSaveLoad.LoadClass<SaveGame>(mFileName);       //Load GameObjects 
		if (tSaveGame != null) {
			foreach(MiniGO tmGO in tSaveGame.ListGO) {
				GameObject	tGO = Instantiate (Prefab);
				Rigidbody2D tRB = tGO.GetComponent<Rigidbody2D> ();
				tRB.velocity = tmGO.mVelocity;
				tRB.angularVelocity = tmGO.mAngularVelocity;
				tRB.position = tmGO.mPosition;
				tRB.rotation = tmGO.mRotation;
			}
			InputText.text = tSaveGame.Header.Details;
			Slider.value = (float)tSaveGame.Header.Score;
			Count.text = string.Format("Save count {0}", tSaveGame.Header.Count);
		}
	}

	public void	Spawn() {
		GameObject	tGO = Instantiate (Prefab);
		Rigidbody2D tRB = tGO.GetComponent<Rigidbody2D> ();
		Vector2	tDirection = new Vector2 (Random.Range (-Slider.value, Slider.value), Random.Range (-Slider.value, Slider.value));
		tRB.AddForce (tDirection, ForceMode2D.Impulse);
	}
}
