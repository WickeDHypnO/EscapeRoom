using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LNBasementController : RoomController {
    private const int ELECTRICAL_PUZZLE_ELEMENTS_COUNT = 5;
    private const int STEPS_TO_OPEN_ELEVATOR_COUNT = 3;
    public UnityEvent OnElectricBoxPuzzleCompleted;
    public UnityEvent OnElevatorOpenStepsCompleted;
    private int placedPuzzleElementsCount;
    private int completedSteps;

    public void PlaceElectircBoxElement()
    {
        puzzleElementPlaced();
    }

    public void CompleteElevatorOpenStep()
    {
        completeElevatorOpenStep();
    }

    [PunRPC]
    private void puzzleElementPlaced()
    {
        ++placedPuzzleElementsCount;
        if (placedPuzzleElementsCount >= ELECTRICAL_PUZZLE_ELEMENTS_COUNT)
        {
            OnElectricBoxPuzzleCompleted.Invoke();
            completeElevatorOpenStep();
        }
    }

    [PunRPC]
    private void completeElevatorOpenStep()
    {
        ++completedSteps;
        if (completedSteps >= STEPS_TO_OPEN_ELEVATOR_COUNT)
        {
            OnElevatorOpenStepsCompleted.Invoke();
        }
    }

    // Use this for initialization
    void Start () {
        placedPuzzleElementsCount = 0;
        completedSteps = 0;
    }
}