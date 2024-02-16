using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacementManager placementManager;
    public List<Vector3Int> temporaryPlacementPosition = new List<Vector3Int>();
    public List<Vector3Int> roadPositionToRecheck = new List<Vector3Int>();
    private Vector3Int startPosition;
    private bool placementMode = false;
    public RoadFixer roadFixer;

    private void Start()
    {
        roadFixer = GetComponent<RoadFixer>();  
    }

    public void PlaceRoad(Vector3Int position)
    {
        if(placementManager.CheckIfPositionInBound(position)==false)
        {
            return;
        }

        if (placementManager.CheckIfPositionIsFree(position)==false)
        {
            return;
        }
        if (placementMode == false)
        {
            temporaryPlacementPosition.Clear();
            roadPositionToRecheck.Clear();

            foreach (var positionToFix in roadPositionToRecheck)
            {
                roadFixer.FixRoadAtPosition(placementManager, positionToFix);
            }

            placementMode = true;
            startPosition = position;   
            temporaryPlacementPosition.Add(position);
            placementManager.PlaceTemporaryStructure(position, roadFixer.deadEnd, CellType.Road);
        }
        else
        {
            placementManager.RemoveAllTemporaryStructure();
            temporaryPlacementPosition.Clear(); 
            roadPositionToRecheck.Clear();
            temporaryPlacementPosition = placementManager.GetPathBetween(startPosition, position);

            foreach (var temporaryPosition in temporaryPlacementPosition)
            {
                if (placementManager.CheckIfPositionIsFree(temporaryPosition) == false)
                {
                    continue;
                }
                placementManager.PlaceTemporaryStructure(temporaryPosition, roadFixer.deadEnd, CellType.Road);

            }
        }
        FixRoadPrefabs();

    }

    private void FixRoadPrefabs()
    {
        foreach (var temporaryPosition in temporaryPlacementPosition)
        {
            roadFixer.FixRoadAtPosition(placementManager, temporaryPosition);
            var neighbours = placementManager.GetNeighboursOfTypeFor(temporaryPosition, CellType.Road);
            foreach (var roadposition in neighbours)
            {
                if (roadPositionToRecheck.Contains(roadposition)==false)
                {
                    roadPositionToRecheck.Add(roadposition);

                }
            }
        }
        foreach (var positionToFix in roadPositionToRecheck)
        {
            roadFixer.FixRoadAtPosition(placementManager, positionToFix);
        }
    }
    public void FinishPlacingRoad()
    {
        placementMode = false;
        placementManager.AddTemporaryStructuresToStructureDictionary();
        if(temporaryPlacementPosition.Count > 0)
        {
            AudioPlayer.instance.PlayPlacementSound();
        }
        temporaryPlacementPosition.Clear();
        startPosition = Vector3Int.zero;
    }
}
