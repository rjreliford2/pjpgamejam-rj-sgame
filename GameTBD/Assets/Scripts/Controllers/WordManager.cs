using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordManager : MonoBehaviour
{
    public float wordSpeed = 2f; // Speed at which words fall

    private int currentLevel = 0; // Current level index
    private int currentWordIndex = 0; // Current word index within the level
    private List<string> currentCombination; // Current combination to match
    private List<string> wordPool; // Pool of words to choose from

    public bool IsGameOver { get; private set; } = false; // Flag to indicate game over

    private List<List<string>> levels; // List of levels, each containing a list of words

    private List<GameObject> spawnedWords = new List<GameObject>(); // Keep track of spawned word objects

    private void Start()
    {
        // Initialize game state
        InitializeLevels();
        LoadLevel(currentLevel);
        CreateWordPool();

        // Start spawning words
        StartCoroutine(SpawnWordsCoroutine());
    }

    private void InitializeLevels()
    {
        // Define levels and their word combinations
        levels = new List<List<string>>();

        // Level 1
        List<string> level1Combination = new List<string> { "word1", "word2", "word3" };
        levels.Add(level1Combination);

        // Level 2
        List<string> level2Combination = new List<string> { "word4", "word5", "word6" };
        levels.Add(level2Combination);

        // Level 3
        List<string> level3Combination = new List<string> { "word7", "word8", "word9" };
        levels.Add(level3Combination);

        // Add more levels as needed
    }

    private void CreateWordPool()
    {
        // Create a pool of words from the current level's combination
        switch (currentLevel)
        {
            case 0: // Level 1
                wordPool = new List<string> { "word1", "word2", "word3" };
                break;
            case 1: // Level 2
                wordPool = new List<string> { "word4", "word5", "word6" };
                break;
            case 2: // Level 3
                wordPool = new List<string> { "word7", "word8", "word9" };
                break;
            default:
                // Set a default word pool if needed
                wordPool = new List<string>();
                break;
        }
    }


    private IEnumerator SpawnWordsCoroutine()
    {
        while (!IsGameOver)
        {
            // Spawn new word if there are more words in the combination
            if (currentWordIndex < currentCombination.Count)
            {
                SpawnWord();
            }

            // Wait for a short delay before spawning the next word
            yield return new WaitForSeconds(2f);
        }
    }

    private void SpawnWord()
    {
        if (wordPool.Count == 0)
        {
            Debug.Log("Current level completed!");
            // Handle level completed logic (e.g., show level completion screen, increase score)
            currentLevel++;
            if (currentLevel < levels.Count)
            {
                LoadLevel(currentLevel);
                CreateWordPool();
            }
            else
            {
                Debug.Log("All levels completed! Game over.");
                IsGameOver = true;
                // Handle game over logic (e.g., display game over screen, reset game)
            }
            return;
        }

        // Select a random word from the pool
        int randomIndex = Random.Range(0, wordPool.Count);
        string word = wordPool[randomIndex];
        wordPool.RemoveAt(randomIndex);

        // Create a new word object in the scene
        GameObject wordObject = new GameObject(word);
        TextMesh textMesh = wordObject.AddComponent<TextMesh>();
        textMesh.text = word;
        textMesh.color = Color.red;
        textMesh.fontSize = 8;

        // Add a 2D Box Collider to the word object
        BoxCollider2D collider = wordObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(2f, 2f); // Adjust the collider size as needed


        // Set initial position and speed
        wordObject.transform.position = new Vector3(Random.Range(-5f, 5f), 10f, 0f);
        Rigidbody2D rb = wordObject.AddComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * wordSpeed;

        // Attach a script to handle word interactions
        WordInteraction wordInteraction = wordObject.AddComponent<WordInteraction>();
        wordInteraction.SetWord(word);
        wordInteraction.SetGameManager(this);
    }

    private void LoadLevel(int levelIndex)
    {
        // Set the current level's combination and reset the word index
        currentCombination = new List<string>(levels[levelIndex]);
        currentWordIndex = 0;
        Debug.Log("Level " + levelIndex + " loaded. Combination: " + string.Join(", ", currentCombination));
    }

    private void Update()
    {
        if (IsGameOver)
        {
            return;
        }

        // Check if any words have left the screen and destroy them
        for (int i = spawnedWords.Count - 1; i >= 0; i--)
        {
            GameObject word = spawnedWords[i];
            if (word.transform.position.y < -10f)
            {
                spawnedWords.RemoveAt(i);
                Destroy(word);
            }
        }
    }

    public void CheckWord(string clickedWord)
    {
        if (clickedWord == currentCombination[currentWordIndex])
        {
            Debug.Log("Correct word! Proceed to the next word.");
            currentWordIndex++;
        }
        else
        {
            Debug.Log("Incorrect word! Game over.");
            IsGameOver = true;
            // Handle game over logic (e.g., display game over screen, reset game)
            SceneManager.LoadScene("GameOver");
        }
    }
}
