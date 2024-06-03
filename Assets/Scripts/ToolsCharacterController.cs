using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolsCharacterController : MonoBehaviour
{
    CharacterController2D character;
    Rigidbody2D rgbd2d;
    ToolbarController toolbarController;
    Animator animator;  
    [SerializeField] float offSetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] float maxDistance = 1.5f;
    [SerializeField] ToolAction onTilePickUp;
    [SerializeField] IconHighlight iconHighlight;

    Vector3Int selectedTilePosition;
    bool selectable;

    private void Awake()
    {
        character = GetComponent<CharacterController2D>();
        rgbd2d = GetComponent<Rigidbody2D>();
        toolbarController = GetComponent<ToolbarController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SelectTile();
        CanSelectCheck();
        Marker();
        if (Input.GetMouseButtonDown(0))
        {
            if (UseToolWorld() == true)
            {
                return;
            }
            UseToolGrid();
        }
    }

    private void SelectTile()
    {
        selectedTilePosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
    }

    void CanSelectCheck()
    {
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
        iconHighlight.CanSelect = selectable;
    }

    private void Marker()
    {
        markerManager.markedCellPosition = selectedTilePosition;
        iconHighlight.cellPosition = selectedTilePosition;
    }

    private bool UseToolWorld()
    {
        Vector2 position = rgbd2d.position + character.lastMotionVector * offSetDistance;

        Item item = toolbarController.GetItem;
        if (item == null) { return false; }
        if (item.onAction == null) { return false; }

        animator.SetTrigger("act");
        bool complete = item.onAction.OnApply(position);

        if (complete == true)
        {
            if (item.onItemUsed != null)
            {
                item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);
            }
        }

        return complete;
    }

    private void UseToolGrid()
    {
        if (selectable == true)
        {
            Item item = toolbarController.GetItem;
            if (item == null) 
            {
                PickUpTile();

                return; 
            }
            if (item.onTileMapAction == null) { return; }

            animator.SetTrigger("act");
            bool complete = item.onTileMapAction.OnApplyToTileMap(
                selectedTilePosition, 
                tileMapReadController, 
                item
                );

            if (complete == true)
            {
                if (item.onItemUsed != null)
                {
                    item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);
                }
            }
        }
    }

    private void PickUpTile()
    {
        if (onTilePickUp == null) { return;}
        
        onTilePickUp.OnApplyToTileMap(selectedTilePosition, tileMapReadController, null);
    }
}
