using UnityEngine;
using System.IO;        //Used for saving
using System;       //Used for Time & Date
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;       //Used to format data
using UnityEngine.UI;


// This class serializes a Vector3 object.
sealed class Vector3SerializationSurrogate : ISerializationSurrogate 
{
	// Serialize Vector3
	public void GetObjectData(System.Object obj,SerializationInfo info, StreamingContext context) 
	{
		Vector3 tVector3 = (Vector3) obj;
		info.AddValue("X", tVector3.x);
		info.AddValue("Y", tVector3.y);
		info.AddValue("Z", tVector3.z);
	}

	// Deserialize Vector3
	public System.Object SetObjectData(System.Object obj,SerializationInfo info, StreamingContext context,ISurrogateSelector selector) 
	{
		Vector3 tVector3 = (Vector3) obj;
		tVector3.x = (float)info.GetDouble("X");
		tVector3.y = (float)info.GetDouble("Y");
		tVector3.z = (float)info.GetDouble("Z");
		return tVector3;
	}
}

// This class serializes a Vector2 object.
sealed class Vector2SerializationSurrogate : ISerializationSurrogate 
{
	// Serialize Vector2
	public void GetObjectData(System.Object obj,SerializationInfo info, StreamingContext context) 
	{
		Vector2 tVector2 = (Vector2) obj;
		info.AddValue("X", tVector2.x);
		info.AddValue("Y", tVector2.y);
	}

	// Deserialize Vector2
	public System.Object SetObjectData(System.Object obj,SerializationInfo info, StreamingContext context,ISurrogateSelector selector) 
	{
		Vector2 tVector2 = (Vector2) obj;
		tVector2.x = (float)info.GetDouble("X");
		tVector2.y = (float)info.GetDouble("Y");
		return tVector2;
	}
}
// This class serializes a Color object.
sealed class ColourSerializationSurrogate : ISerializationSurrogate 
{
	// Serialize Color
	public void GetObjectData(System.Object obj,SerializationInfo info, StreamingContext context) 
	{
		Color tColor = (Color) obj;
		info.AddValue("R", tColor.r);
		info.AddValue("G", tColor.g);
		info.AddValue("B", tColor.b);
		info.AddValue("A", tColor.a);
	}

	// Deserialize Color
	public System.Object SetObjectData(System.Object obj,SerializationInfo info, StreamingContext context,ISurrogateSelector selector) 
	{
		Color tColor = (Color) obj;
		tColor.r = (float)info.GetDouble("R");
		tColor.g = (float)info.GetDouble("G");
		tColor.b = (float)info.GetDouble("B");
		tColor.a = (float)info.GetDouble("A");
		return tColor;
	}
}

sealed class QuaternionSerializationSurrogate : ISerializationSurrogate 
{
	// Serialize Quaternion
	public void GetObjectData(System.Object obj,SerializationInfo info, StreamingContext context) 
	{
		Quaternion tQuaternion = (Quaternion) obj;
		info.AddValue("X", tQuaternion.x);
		info.AddValue("Y", tQuaternion.y);
		info.AddValue("Z", tQuaternion.z);
		info.AddValue("W", tQuaternion.w);
	}

	// Deserialize Quaternion
	public System.Object SetObjectData(System.Object obj,SerializationInfo info, StreamingContext context,ISurrogateSelector selector) 
	{
		Quaternion tQuaternion = (Quaternion) obj;
		tQuaternion.x = (float)info.GetDouble("X");
		tQuaternion.y = (float)info.GetDouble("Y");
		tQuaternion.z = (float)info.GetDouble("Z");
		tQuaternion.w = (float)info.GetDouble("W");
		return tQuaternion;
	}
}

public class SaveGame : MonoBehaviour {

    //Serialisation is harder than is should be as Unity won't allow complex objects to be stored
    //To store game items the important information needs to be broken out and stored, the rest should be recreated
    //C# can store floats, strings etc. however not complex objects
    //A storage class is created and its this which is stored
    //C# store (serialise) generic lists List<>, so once the items are in a list the can all be saved
    //Loading (deserialisation) works the opposite way, the list of stored attributes is loaded into a list
    //From the copies of the prefab are made and using the loaded information, put in the right place on screen

 
 
    private string mLastError;                  //Last Error Message

    public string LastErrorMessage {
        get {
            return mLastError;
        }
    }

    //Used to access SaveLoad from entire Game
    public static SaveGame singleton;
    SurrogateSelector mAdditionalSerialisers;

    private void Awake() {
        if(singleton==null) {
            mAdditionalSerialisers = ExtendSurrogates();
            singleton = this;
            DontDestroyOnLoad(gameObject);
        } else if(singleton!=this) {
            DestroyObject(gameObject);
        }
    }


	//Extend SurrogateSelectors for loading & saving
	private	SurrogateSelector	ExtendSurrogates() {
		SurrogateSelector tSS = new SurrogateSelector();
		tSS.AddSurrogate(typeof(Vector3),new StreamingContext(StreamingContextStates.All),new Vector3SerializationSurrogate());
		tSS.AddSurrogate(typeof(Vector2),new StreamingContext(StreamingContextStates.All),new Vector2SerializationSurrogate());
		tSS.AddSurrogate(typeof(Quaternion),new StreamingContext(StreamingContextStates.All),new QuaternionSerializationSurrogate());
		tSS.AddSurrogate(typeof(Color),new StreamingContext(StreamingContextStates.All),new ColourSerializationSurrogate());
		return tSS;
	}


    //FileStructure
    //Version number

    [SerializeField]
    private int CurrentVersionNumber = 2;

    public static  int CurrentVersion {
        get {
            return singleton.CurrentVersionNumber;
        }
    }

    public  bool    Load(string vFilename) {
        bool tSuccess = false;
        string tFullPath = Application.persistentDataPath + "/" + vFilename;
        FileStream tFS = null;
        if (File.Exists(tFullPath)) {
            try {       //This will try to run the code below, but if there is an error go straight to catch
                BinaryFormatter tBF = new BinaryFormatter();            //use C# Binary data, that way user cannot edit it easily
                tBF.SurrogateSelector = mAdditionalSerialisers;		//Include the code to allow serialization of Vectors & Quaternions 
                tFS = File.Open(tFullPath, FileMode.Open);       //Open File I/O
                int tVersionNumber = (int)tBF.Deserialize(tFS);             //Grab Version Number
                Debug.LogFormat("Current GameVersion V{0:d} LoadGame V{1:d}", CurrentVersionNumber,tVersionNumber);
                tSuccess=LoadVersion(tVersionNumber, tFS, tBF);      //Load using correct loader
                tFS.Close();        //Close file
            } catch (Exception tE) {      //If an error happens above, comes here
                mLastError = "Load Error:" + tE.Message;
            } finally {     //This will run at the end of the try, if it succeeded or failed
                if (tFS != null) {      //If we opened the file, close it again, this is in case we have an error above, we ensure file is closed
                    tFS.Close();        //Close file
                }
            }
        } else {
            mLastError = tFullPath + " not found";
        }
        return tSuccess;
    }


    private bool LoadVersion(int vVersionNumber,FileStream vFS, BinaryFormatter vBF) {
        string tOK = "OK";
        switch (vVersionNumber) {       //Use correct loader

            case 1: {
                    int tObjectCount = (int)vBF.Deserialize(vFS);       ///Get number of Objects
                    while(tObjectCount > 0) {
                        SaveObject.Create(vVersionNumber, vFS, vBF);  //Get Object to create itself
                        tObjectCount--;
                    }
                    mLastError = tOK;
                    return true;
            }
            case 3:     //V2&3 are the same at this level
            case 2: {
                    GameManager.PanelInputField = (string)vBF.Deserialize(vFS);     //Version 2 feature
                    int tObjectCount = (int)vBF.Deserialize(vFS);       ///Get number of Objects
                    while (tObjectCount > 0) {
                        SaveObject.Create(vVersionNumber, vFS, vBF);     //Get Object to create itself
                        tObjectCount--;
                    }
                    mLastError = tOK;
                    return true;
                }
            default:
                mLastError = "Wrong Savegame Version";
                break;
        }
        return false;
    }



    public bool Save(string vFilename) {
        bool tSuccess = false;
        string tFullPath = Application.persistentDataPath + "/" + vFilename;
        mLastError = "";
        FileStream tFS = null;          //If null file was not opened
        try {
            BinaryFormatter tBF = new BinaryFormatter();        //Store as binary
			tBF.SurrogateSelector = mAdditionalSerialisers;	//Include the code to allow serialization of Vectors & Quaternions
            tFS = File.Create(tFullPath);		//Open File
            SaveCurrentVersion(tFS, tBF);
            tSuccess =true;
            mLastError = "Saved OK";
        } catch (Exception tE) {        //Deal with error
            mLastError = "Save Error:" + tE.Message;
        } finally {     //Make sure file is closed, if it was open
            if (tFS != null) {
                tFS.Close();        //Close file
            }
        }
        return tSuccess;
    }

    //Save file format for V1
    //Version Number
    //Number of Objects
    //SaveObjects[] must all be Balls


    //Save file format for V2
    //Version Number
    //Panel Input field
    //Number of Objects
    //SaveObjects[] must all be Balls

    //Save file format for V3
    //Version Number
    //Panel Input field
    //Number of Objects
    //SaveObjects[] Can be other prefabs

    private void    SaveCurrentVersion(FileStream vFS, BinaryFormatter vBF) {
        vBF.Serialize(vFS, CurrentVersionNumber);          //Save Data, Current Version
        if(CurrentVersionNumber>1) {
            //Version 2 feature
            vBF.Serialize(vFS, GameManager.PanelInputField);            //Save Input field Added in Version 2
        }
        SaveObject[] tSaveArray = FindObjectsOfType<SaveObject>();      //Find Objects which have Save code
        vBF.Serialize(vFS, tSaveArray.Length);          //Store Number of game objects saved
        foreach (var tSB in tSaveArray) {
            tSB.Save(CurrentVersionNumber,vFS, vBF);         //Ask object to save itself
        }
    }
}
