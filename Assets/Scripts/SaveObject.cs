using UnityEngine;
using System.IO;        //Used for saving
using System.Runtime.Serialization.Formatters.Binary;       //Used to format data


public class SaveObject : MonoBehaviour {

    int PrefabIndex;  //Version 3+ feature, allows multiple prefabs by using index

    public  void    Save(int vVersion,FileStream vFS, BinaryFormatter vBF) {

        if(vVersion>=3) {   //Version 3+ feature
            vBF.Serialize(vFS, PrefabIndex);          //Save index of prefab used to make this
        }

        Rigidbody2D tRB = GetComponent<Rigidbody2D>();
        vBF.Serialize(vFS, tRB.position);          //Save only data we need for reload
        vBF.Serialize(vFS, tRB.velocity);
        vBF.Serialize(vFS, tRB.rotation);
        vBF.Serialize(vFS, tRB.name);
    }


    public static int PrefabCount {
        get {
            return LoadedPrefabNames.Length;
        }
    }

    private static string[] LoadedPrefabNames = { "Ball", "Square" };   //Keep in this order, so index matches
    //Create from Index
    static public GameObject Create(int vVersion, int vPrefabNameIndex) {       //Create Prefab from Index

        Debug.AssertFormat(vPrefabNameIndex >= 0 && vPrefabNameIndex < LoadedPrefabNames.Length, "Index {0:d} out of range"); //Illegal object

        string tPrefabName = LoadedPrefabNames[vPrefabNameIndex];       //Get filename of indexed object

        GameObject tGO = Instantiate(LoadPrefab(tPrefabName));  //Load and instantiate object   //Create it
        Debug.AssertFormat(tGO != null, "Could not Instantiate prefab {0:s}", tPrefabName);

        if (vVersion >= 3) {       //V3+ feature
            SaveObject tSO = tGO.GetComponent<SaveObject>();    //Get Saveobject
            Debug.AssertFormat(tGO != null, "Could not find SaveObject on {0:s}", tPrefabName);
            tSO.PrefabIndex = vPrefabNameIndex;
        }
        return tGO;
    }

    //Create from SaveGame
    static  public void Create(int vVersion,FileStream vFS, BinaryFormatter vBF) {
        GameObject tGO = null;

        if(vVersion>=3) {   //V3 + allows multiple prefabs
            int tPrefabIndex = (int)vBF.Deserialize(vFS);       //For V3 objects get Index to load correct prefab
            tGO = Create(vVersion,tPrefabIndex);                   //Make GameObject from PrefabIndex
        } else { //Only Balls
            tGO = Instantiate(LoadPrefab("Ball"));                   //Make GameObject from single Template
        }

        Debug.AssertFormat(tGO!=null,"Could not Instantiate prefab");

        SaveObject tSaveObject = tGO.GetComponent<SaveObject>();

        Debug.AssertFormat(tSaveObject != null, "No SaveBall script found on {0:s}", tGO.name);

        Rigidbody2D tRB = tGO.GetComponent<Rigidbody2D>();      //Get its RB2D to set it up
        Debug.AssertFormat(tRB != null, "No RigidBody2D script found on {0:s}", tGO.name);

        tRB.position = (Vector2)vBF.Deserialize(vFS);           //Get Data from file, in same order in which it was saved
        tRB.velocity = (Vector2)vBF.Deserialize(vFS);
        tRB.rotation = (float)vBF.Deserialize(vFS);
        tRB.name = (string)vBF.Deserialize(vFS);
    }


    private  static  GameObject  LoadPrefab(string vName) {
        GameObject  tPrefab=Resources.Load<GameObject>(vName);
        Debug.AssertFormat(tPrefab != null, "Cannot find {0:s}", vName);
        return tPrefab;
    }

//Legacy code V1 loader, examples superceeded in most recent version, FYI only


    public void SaveV1(int vVersion, FileStream vFS, BinaryFormatter vBF) {

        Rigidbody2D tRB = GetComponent<Rigidbody2D>();
        vBF.Serialize(vFS, tRB.position);          //Save only data we need for reload
        vBF.Serialize(vFS, tRB.velocity);
        vBF.Serialize(vFS, tRB.rotation);
    }

    void LoadV1(int vVersion, FileStream vFS, BinaryFormatter vBF) {
        Rigidbody2D tRB = GetComponent<Rigidbody2D>();      //Get its RB2D to set it up
        Debug.AssertFormat(tRB != null, "No RigidBody2D script found on {0:s}", name);

        tRB.position = (Vector2)vBF.Deserialize(vFS);           //Get Data from file, in same order in which it was saved
        tRB.velocity = (Vector2)vBF.Deserialize(vFS);
        tRB.rotation = (float)vBF.Deserialize(vFS);
    }

}
