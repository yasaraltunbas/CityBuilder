using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public InputManager inputManager;
    public RoadManager roadManager;
    public UiController uiController;
    public StructureManager structureManager;
    private void Start()
    {
        uiController.onRoadPlacement += RoadPlacementHandler;
        uiController.onHousePlacement += HousePlacementHandler;
        uiController.onSpecialPlacement += SpecialPlacementHandler;


    }

    private void SpecialPlacementHandler()
    {
        ClearInputAction();
        inputManager.OnMouseClick += structureManager.PlaceSpecial;
    }

    private void HousePlacementHandler()
    {
        ClearInputAction();
        inputManager.OnMouseClick += structureManager.PlaceHouse;

    }

    private void RoadPlacementHandler()
    {
        ClearInputAction();
        inputManager.OnMouseClick += roadManager.PlaceRoad;
        inputManager.OnMouseHold += roadManager.PlaceRoad;
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
    }

    private void ClearInputAction()
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null;
    }

        private void HandleMouseClick(Vector3Int position)
    {
        Debug.Log(position);
        roadManager.PlaceRoad(position);

    }
    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x, 0, inputManager.CameraMovementVector.y));
    }
}
