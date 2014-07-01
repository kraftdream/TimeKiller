using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

    private State _currentState;
    public State CurrentState 
    {
        get { return _currentState; }
        set { _currentState = value; }
    }

	void Start () {
        CurrentState = State.IDLE;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
