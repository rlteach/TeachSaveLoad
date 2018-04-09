using UnityEngine;
using System.IO;        //Used for saving
using System.Runtime.Serialization.Formatters.Binary;       //Used to format data


public class SaveBall : MonoBehaviour {

    public  void    Save(FileStream vFS, BinaryFormatter vBF) {
        Rigidbody2D tRB = GetComponent<Rigidbody2D>();
        vBF.Serialize(vFS, tRB.position);          //Save only data we need for reload
        vBF.Serialize(vFS, tRB.velocity);
        vBF.Serialize(vFS, tRB.rotation);
        vBF.Serialize(vFS, tRB.name);
    }

    static  public void Load(int vVersion,FileStream vFS, BinaryFormatter vBF,GameObject vPrefab) {

        switch(vVersion) {
            case 1: {
                    GameObject  tGO=Instantiate(vPrefab);                   //Make GameObject from Template
                    Rigidbody2D tRB = tGO.GetComponent<Rigidbody2D>();      //Get its RB2D to set it up
                    tRB.position = (Vector2)vBF.Deserialize(vFS);           //Get Data from file, in same order in which it was saved
                    tRB.velocity = (Vector2)vBF.Deserialize(vFS);
                    tRB.rotation = (float)vBF.Deserialize(vFS);
                    tRB.name = (string)vBF.Deserialize(vFS);
                    break;
                }
            default:
                Debug.LogErrorFormat("No loader for version %d", vVersion);
                break;
        }
    }
}
