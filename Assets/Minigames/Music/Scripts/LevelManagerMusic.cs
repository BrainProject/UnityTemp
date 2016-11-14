using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManagerMusic : MonoBehaviour {

    public GameObject YellowNote;
    public GameObject BlueNote;
    public GameObject GreenNote;
    public GameObject PurpleNote;

    public List<GameObject> Blanks;


    private float counting;
    public float speedOfDisplayingButtons;

    public List<GameObject> listOfVisible;


	void Start () {
        listOfVisible = new List<GameObject>();

        Instantiate(YellowNote, GetRandomPositionOnScreen(), Quaternion.identity);
        listOfVisible.Add(YellowNote);

        Instantiate(BlueNote, GetRandomPositionOnScreen(), Quaternion.identity);
        listOfVisible.Add(BlueNote);

        Instantiate(GreenNote, GetRandomPositionOnScreen(), Quaternion.identity);
        listOfVisible.Add(GreenNote);

        Instantiate(PurpleNote, GetRandomPositionOnScreen(), Quaternion.identity);
        listOfVisible.Add(PurpleNote);
    }
	
	void Update () {
        // interval time between displaying buttons
        counting += Time.deltaTime;
        if (counting >= speedOfDisplayingButtons)
        {
            Debug.Log(listOfVisible.Count);
            counting = 0;
            foreach (GameObject note in listOfVisible)
            {
                // if the note is not displayed - display it
                if (note.GetComponent<SpriteRenderer>().enabled == false)
                {
                    DisplayButton(note);
                }
            }
        }
	    
	}

    /// <summary>
    /// Displays a note or a blank button depending on situation
    /// </summary>
    void AddNoteOrBlank()
    {

        //Random.Range(0, 10);
        //GameObject newButton = Instantiate(Notes[0]) as GameObject;
    }

    void DisplayButton(GameObject note)
    {
        note.GetComponent<Transform>().position = GetRandomPositionOnScreen();
        note.GetComponent<SpriteRenderer>().enabled = true;
    }

    Vector2 GetRandomPositionOnScreen()
    {
        float x = Random.Range(-1.8f, 1.8f);
        float y = Random.Range(0.8f, 2.1f);
        return new Vector2(x, y);
    }
}
