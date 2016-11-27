using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

    public  void    QuitToMenu() {
        GameManager.LoadLevel(0);
    }

    public void ChooseLevel(int vIndex) {
        GameManager.LoadLevel(vIndex);
    }
}
