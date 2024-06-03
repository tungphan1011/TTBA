using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootContainedInteract : Interactable
{
    [SerializeField] GameObject closedChest;
    [SerializeField] GameObject openedChest;
    [SerializeField] bool opened;
    [SerializeField] AudioClip onOpenedAudio;
    [SerializeField] AudioClip onClosedAudio;

    public override void Interact(Character character)
    {
        if (opened == false)
        {
            opened = true;
            closedChest.SetActive(false);
            openedChest.SetActive(true);
            Debug.Log("Chest opened");

            AudioManager.instance.Play(onOpenedAudio);
        }
        else 
        {
            opened = false;
            closedChest.SetActive(true);
            openedChest.SetActive(false);
            Debug.Log("Chest closed");

            AudioManager.instance.Play(onClosedAudio);
        }
    }
}
