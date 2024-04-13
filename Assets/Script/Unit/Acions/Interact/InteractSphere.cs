using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

// Class representing an interactable sphere in the game
public class InteractSphere : MonoBehaviour, IInteractable
{
    public event EventHandler OnSphereTurnedRed;

    [SerializeField]
    private Material greenMaterial;
    [SerializeField]
    private Material redMaterial;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private List<Unit> enemyUnitList;
    [SerializeField]
    private AudioClip sphereClip;
    [SerializeField]
    private bool isLastSphere;

    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;
    private AudioSource sphereAudioSource;

    private bool isGreen;
    private void Start()
    {
        sphereAudioSource = GetComponent<AudioSource>();
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        SetColorGreen();
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        // Countdown timer for sphere interaction
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();
        }
    }


    private void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;
    }

    private void SetColorRed()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;

        //Sound
        sphereAudioSource.clip = sphereClip;
        sphereAudioSource.Play();

        // If this is the last sphere, unlock new level
        if (isLastSphere)
        {
            UnlockNewLevel();
        }

        OnSphereTurnedRed?.Invoke(this, EventArgs.Empty);
        foreach(Unit unit
            in enemyUnitList)
        {
            unit.SetUnitActive(true);
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 0.5f;
        if (isGreen) 
        {
            SetColorRed();
        }
        else
        {
            SetColorGreen();
        }
        
    }

    private void UnlockNewLevel()
    {
        // Update player's progress if the current scene is the farthest reached scene
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();

        }
    }
}
