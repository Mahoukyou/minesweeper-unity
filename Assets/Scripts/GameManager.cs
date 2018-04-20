using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager instance { get; private set; }

    public int numberOfBombs = 5;
    public int numberOfRows = 10;
    public int numberOfColumns = 10;

	void Awake ()
    {
        if(instance != null && instance != this)
        {
            Debug.Log("More than one gamemanager instance");
            Destroy(instance);
        }

        instance = this;
	}


    void Start()
    {

    }

}
