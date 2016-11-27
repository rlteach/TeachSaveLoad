using UnityEngine;
using System.Collections.Generic;           //List<generic>
using System.Runtime.Serialization.Formatters.Binary;       //Used to format data
using System.IO;        //Used for saving
using System;       //Used for Time & Date



public class SaveLoad : MonoBehaviour {

    //Serialisation is harder than is should be as Unity won't allow complex objects to be stored
    //To store game items the important information needs to be broken out and stored, the rest should be recreated
    //C# can store floats, strings etc. however not complex objects
    //A storage class is created and its this which is stored
    //C# store (serialise) generic lists List<>, so once the items are in a list the can all be saved
    //Loading (deserialisation) works the opposite way, the list of stored attributes is loaded into a list
    //From the copies of the prefab are made and using the loaded information, put in the right place on screen

 
    private string mFilePath;                   //Path for Save

    private string mLastError;                  //Last Error Message

    public string LastErrorMessage {
        get {
            return mLastError;
        }
    }

    void    Start() {
        mFilePath = Application.persistentDataPath + "/";
    }

    public T LoadClass<T>(string vFileName) where T : new() {       //Load file of List<SavedObject> containing Position, rotation & name for each item
        string tFullPath = mFilePath + vFileName;
        mLastError = "";
        T tSaveData = default(T);                    //Space to load header
        if (File.Exists(tFullPath)) {
            FileStream tFS = null;  //If null file was not opened
            try {       //This will try to run the code below, but if there is an error go straight to catch
                BinaryFormatter tBF = new BinaryFormatter();            //use C# Binary data, that way user cannot edit it easily
                tSaveData = new T();                    //Space to load header
                tFS = File.Open(tFullPath, FileMode.Open);       //Open File I/O
                tSaveData = (T)tBF.Deserialize(tFS);             //Grab Header
                tFS.Close();        //Close file
                mLastError = "Loaded OK";
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
        return tSaveData;
    }

    public bool SaveClass<T>(T vSaveClass,string vFileName) {
        string tFullPath = mFilePath + vFileName;
        mLastError = "";
        bool tSuccess = false;
        FileStream tFS = null;          //If null file was not opened
        try {
            BinaryFormatter tBF = new BinaryFormatter();        //Store as binary
            tFS = File.Create(tFullPath);
            tBF.Serialize(tFS, vSaveClass);          //Save Data
            tSuccess=true;
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
}
