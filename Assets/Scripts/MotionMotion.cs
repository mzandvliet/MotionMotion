using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimWinInput;
public class MotionMotion : MonoBehaviour {
    private ControllerState _left;
    private ControllerState _right;

	private bool _active = true;

	private void Awake() {
		Application.runInBackground = true;

		Debug.Log("Left Touch Conroller active: " + OVRInput.IsControllerConnected(OVRInput.Controller.LTouch));
       	Debug.Log("Right Touch Conroller active: " + OVRInput.IsControllerConnected(OVRInput.Controller.RTouch));
	}

    private void Update() {
		if (Input.GetKeyDown(KeyCode.M)) {
			_active = !_active;
		}

        _left.Position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        _right.Position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        _left.AngularVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        _right.LinearVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        _left.LinearAccelleration = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.LTouch);
        _right.AngularAccelleration = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch);
        
        _left.Rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        _right.Rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        _left.AngularVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);
        _right.AngularVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
        _left.AngularAccelleration = OVRInput.GetLocalControllerAngularAcceleration(OVRInput.Controller.LTouch);
        _right.AngularAccelleration = OVRInput.GetLocalControllerAngularAcceleration(OVRInput.Controller.RTouch);

        var indexL = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        var indexR = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);

        int x = (int)(_right.AngularVelocity.y * (0.5f + 0.5f * indexR) * 30f);
		int y = (int)(_right.AngularVelocity.x * (0.5f + 0.5f * indexR) * 30f);

		if (_active) {
            // Look
			SimMouse.ActRaw(SimMouse.Action.MoveOnly, x, y);

            // Shoot
			if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)) {
				SimMouse.Act(SimMouse.Action.LeftButtonDown, 0, 0);
			}
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)) {
				SimMouse.Act(SimMouse.Action.LeftButtonUp, 0, 0);
			}

            //Vector2 moveAnalog = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

            HandleKey(OVRInput.Button.Up, OVRInput.Controller.LTouch, DirectInput.ScanCode.W);
            HandleKey(OVRInput.Button.Down, OVRInput.Controller.LTouch, DirectInput.ScanCode.S);
            HandleKey(OVRInput.Button.Left, OVRInput.Controller.LTouch, DirectInput.ScanCode.A);
            HandleKey(OVRInput.Button.Right, OVRInput.Controller.LTouch, DirectInput.ScanCode.D);

            HandleKey(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch, DirectInput.ScanCode.Space);
            HandleKey(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch, DirectInput.ScanCode.LControl);

            HandleKey(OVRInput.Button.Left, OVRInput.Controller.RTouch, DirectInput.ScanCode.Q);
            HandleKey(OVRInput.Button.Right, OVRInput.Controller.RTouch, DirectInput.ScanCode.E);

        }
    }

    private void HandleKey(OVRInput.Button button, OVRInput.Controller controller, DirectInput.ScanCode scancode) {
        if (OVRInput.GetDown(button, controller))
        {
            DirectInput.Input.SendKey(scancode, false);
        }
        if (OVRInput.GetUp(button, controller))
        {
            DirectInput.Input.SendKey(scancode, true);
        }
    }

    private void FixedUpdate() {
        // OVRInput.FixedUpdate();
    }
}

public struct ControllerState {
    public Vector3 Position;
    public Vector3 LinearVelocity;
    public Vector3 LinearAccelleration;
    
    public Quaternion Rotation;
    public Vector3 AngularVelocity;
    public Vector3 AngularAccelleration;
}

