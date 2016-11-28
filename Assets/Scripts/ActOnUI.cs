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
		public	Vector2		mPosition;		//Position of RB
		public	float		mRotation;		//Rotation of RB
		public	Vector2		mVelocity;			//Velocity of RB
		public	float		mAngularVelocity;	//Angular Velocity of RB
		public	MiniGO() {					//Default constructor
			mPosition=Vector2.zero;		//Sensible values
			mRotation=0f;
			mVelocity=Vector2.zero;
			mAngularVelocity=0f;
		}
	}

	[System.Serializable]           //Must include this to allow Class to save
	public class SaveGame {
		public	int				Version;		//Version of save file
		public	SaveData		Header;		//Save File Header
		public	List<MiniGO>	ListGO;		//List of Game Objects to save
		public	SaveGame() {		//Default constructor
			Version=1;				//Sensible values
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


	public void Clear() {		//Delete all tagged objects
		GameObject[]	tGOArray = GameObject.FindGameObjectsWithTag ("SaveThis");		//Find Objects Tagged as SaveThis, these are all the objects we wish to store
		foreach (GameObject tGO in tGOArray) {
			Destroy (tGO);		//Kill them
		}
	}

	public	void	SaveState() {			//Save Game state
		GameObject[]	tGOArray = GameObject.FindGameObjectsWithTag ("SaveThis");		//Find all Objects Tagged as SaveThis, these are all the objects we wish to store
		SaveGame	tSaveGame = new SaveGame ();		//Name SaveGame class
		foreach (GameObject tGO in tGOArray) {			//For all objects marked as saveable
			Rigidbody2D tRB = tGO.GetComponent<Rigidbody2D> ();		//Get RB
			MiniGO tmGO = new MiniGO ();			//Make a miniGO, with just enough data to store what we need
			tmGO.mPosition = tRB.position;			//copy variables
			tmGO.mRotation = tRB.rotation;
			tmGO.mVelocity = tRB.velocity;
			tmGO.mAngularVelocity = tRB.angularVelocity;
			tSaveGame.ListGO.Add (tmGO);		//Add to list
		}
		tSaveGame.Header.Details = InputText.text;		//Store other details in save file
		tSaveGame.Header.Score = (int)Slider.value;
		tSaveGame.Header.Count=tSaveGame.ListGO.Count;
		if (mSaveLoad.SaveClass<SaveGame> (tSaveGame, mFileName)) {		//Use save routine to save the class
			Debug.Log ("Saved");
		} else {
			Debug.Log (mSaveLoad.LastErrorMessage);
		}
	}

	public	void	LoadState() {
		SaveGame tSaveGame=mSaveLoad.LoadClass<SaveGame>(mFileName);       //Load GameObjects 
		if (tSaveGame != null) {						//If loaded successfully
			foreach(MiniGO tmGO in tSaveGame.ListGO) {		//Make new GO from prefab for each loaded item
				GameObject	tGO = Instantiate (Prefab);
				Rigidbody2D tRB = tGO.GetComponent<Rigidbody2D> ();		//Get RB
				tRB.velocity = tmGO.mVelocity;						///Copy values from save file
				tRB.angularVelocity = tmGO.mAngularVelocity;
				tRB.position = tmGO.mPosition;
				tRB.rotation = tmGO.mRotation;
			}
			InputText.text = tSaveGame.Header.Details;			//Grab header details
			Slider.value = (float)tSaveGame.Header.Score;
			Count.text = string.Format("Save count {0}", tSaveGame.Header.Count);
		}
	}

	public void	Spawn() {			//Span a new object from prefab
		GameObject	tGO = Instantiate (Prefab);
		Rigidbody2D tRB = tGO.GetComponent<Rigidbody2D> ();
		Vector2	tDirection = new Vector2 (Random.Range (-Slider.value, Slider.value), Random.Range (-Slider.value, Slider.value));	//Random accelleration
		tRB.AddForce (tDirection, ForceMode2D.Impulse);
	}
}
