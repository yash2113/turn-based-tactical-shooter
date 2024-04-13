using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private string IS_OPEN = "IsOpen";

    [SerializeField]
    private bool isOpen;
    [SerializeField]
    private List<Unit> enemyUnitList;
    [SerializeField]
    private AudioClip doorSound;

    private GridPosition gridPosition;
    private Animator animator;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;
    private AudioSource doorAudioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        doorAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        if (isOpen )
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }

    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        timer -= Time.deltaTime;

        // If the interaction timer reaches zero, deactivate interaction and invoke the completion callback
        if (timer <= 0f )
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 0.5f;
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool(IS_OPEN, isOpen);
        doorAudioSource.clip = doorSound;
        doorAudioSource.Play();
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);

        foreach(Unit unit in enemyUnitList)
        {
            unit.SetUnitActive(true);
        }
    }
    private void CloseDoor()
    {
        isOpen = false;
        doorAudioSource.Play();
        animator.SetBool(IS_OPEN, isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
