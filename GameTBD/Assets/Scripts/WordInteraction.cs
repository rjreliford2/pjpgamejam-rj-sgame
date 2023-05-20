using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordInteraction : MonoBehaviour
{
    private string word;
    private WordManager gameManager;

    public void SetWord(string word)
    {
        this.word = word;
    }

    public void SetGameManager(WordManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void OnMouseDown()
    {
        gameManager.CheckWord(word);
        Debug.Log("clicked!");
        Destroy(gameObject); // Delayed destruction of the word object
    }

    private void Update()
    {
        // Check if the word object is out of view and delete it
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
