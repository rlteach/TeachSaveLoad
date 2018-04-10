using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


    //This uses a singleton design pattern to create a single Game Manager object
    //This object uses DontDestroyOnLoad to survive new levels being loaded
    //By doing so it will hand around allowing levels to be loaded

	[HideInInspector]					//Don't show this in IDE
	public	int	Score;					//Game Score


	public	GameObject	ErrorPrefab;    //Link to Error Box



    //Key to use for PlayPrefs
    private const string tKeyPlayCount = "PlayCount";
    public static   int    PlayCount {
    get {
            if (!PlayerPrefs.HasKey(tKeyPlayCount)) {
                PlayerPrefs.SetInt(tKeyPlayCount, 0);        //If Key Does not exist make it
            }
            return  PlayerPrefs.GetInt(tKeyPlayCount);       //Get Key
        }
        set {
            PlayerPrefs.SetInt(tKeyPlayCount, value);        //Set Key
        }
    }

    private const string tKeyFirstPlayed = "PlayTime";
    public static string PlayTime {
        get {
                if (!PlayerPrefs.HasKey(tKeyFirstPlayed)) {     //If timje not set , set it
                    PlayTime = System.DateTime.Now.ToString("H:mm d-M-yy");        //If Key Does not exist make it
                }
            return PlayerPrefs.GetString(tKeyFirstPlayed);       //Get Key
        }
        set {
            if(!PlayerPrefs.HasKey(tKeyFirstPlayed)) {      //Only set time once
                PlayerPrefs.SetString(tKeyFirstPlayed, value);        //Set Key
            }
        }
    }


    void    TestSaveLoad() {
        const string tFilename = "UserDetails";
        if (SaveGame.TestSave(tFilename,"Richard", 54)) {
            string tName;
            int tAge;
            Debug.Log("Saved UserDetail");
            if (SaveGame.TestLoad(tFilename,out tName,out tAge)) {
                Debug.LogFormat("Loaded UserDetail:{0:s} Age {1:d}", tName, tAge);
            }
        }
    }


    public static GameManager singleton;       //Using a static means we can access this without knowing the instance
    void Awake() {
        if (singleton == null) {       //Check if we already have a Game Manager
            singleton = this;          //If not make this one it
            DontDestroyOnLoad(gameObject);       //Make this Object persists when loading scenes
            Debug.Log("Game manager active");
			InitGame ();
            TestSaveLoad();
            PlayCount++;        //Update play Count
        } else if (singleton != this) {
            Destroy(gameObject);     //If we are trying to create a second one, destroy it, there must be just one
        }
    }

	void	InitGame() {
		Score = 0;
	}

    public static void LoadLevel(int vIndex) {      //Load one of the stored levels, these must be in a Resources folder
        if (singleton != null) {       //The level manager should be in the first level laoded to allow it to persist itself
            if (vIndex < SceneManager.sceneCountInBuildSettings) {
                SceneManager.LoadScene(vIndex);
                Debug.Log("Scene " + SceneManager.GetActiveScene().name + " Loaded");       //Get the scene name
            } else {
                Debug.Log("Invalid Index:" + vIndex);
            }
        } else {
            Debug.Log("Game manager not initialised, it should load in 1st Scene");       //Add this to the first level which is loaded
        }
    }

	public	static void	MessageBox(string vMessage,string vTitle="Message") {		//Display an error box in current scene, in red
		GameObject tGO=Instantiate (singleton.ErrorPrefab);
		Error tError = tGO.GetComponent<Error> ();
		tError.Message = vMessage;
		tError.Colour = Color.red;
        tError.Title = vTitle;
	}

	public	static void	MessageBox(string vMessage,Color vColour, string vTitle = "Message") {		//Display an error box in current scene, using colour
		GameObject tGO=Instantiate (singleton.ErrorPrefab);
		Error tError = tGO.GetComponent<Error> ();
		tError.Message = vMessage;
		tError.Colour = vColour;
        tError.Title = vTitle;
    }


    public  static  string PanelInputField {
        get {
            InputField tInput = FindObjectOfType<InputField>();
            Debug.Assert(tInput != null, "No InputField found");
            return tInput.text;
        }
        set {
            InputField tInput = FindObjectOfType<InputField>();
            Debug.Assert(tInput != null, "No InputField found");
            tInput.text = value;
        }
    }

}
