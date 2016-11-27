using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Error : MonoBehaviour {

	private	Text[]	mText;
	private	Image	mPanelImage;


	// Use this for initialization
	void Awake () {
		mText = GetComponentsInChildren<Text> ();	//This will get both Text objects, the one we want for the message is the second one
		mPanelImage=GetComponentInChildren<Image>();
	}

	public	string	Message {				//Set the text box to the message
		set {
			mText [1].text = value;
		}
	}

    public string Title {             //Set the Title box to the message
        set {
            mText[0].text = value;
        }
    }

    public Color	Colour {				//Set the text box Colour
		set {
			mPanelImage.color = value;
		}
	}

	public	void	Close() {		//Remove Error Box
		Destroy (gameObject);
	}
}
