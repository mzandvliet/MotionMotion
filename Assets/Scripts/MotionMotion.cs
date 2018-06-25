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

        int x = (int)(_right.AngularVelocity.y * (0.25f + 0.75f * indexR) * 30f);
		int y = (int)(_right.AngularVelocity.x * (0.25f + 0.75f * indexR) * 30f);

		if (_active) {
            // Look
			// SimMouse.ActRaw(SimMouse.Action.MoveOnly, x, y);

            // // Shoot
			// if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)) {
			// 	SimMouse.Act(SimMouse.Action.LeftButtonDown, 0, 0);
			// }
            // if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)) {
			// 	SimMouse.Act(SimMouse.Action.LeftButtonUp, 0, 0);
			// }

            Vector2 move = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            if (move.x > 0.5f) {
                Debug.Log("Move");
            }
            // if (move.x < 0.5f) {
            //     SimKeyboard.Press((byte)'A');
            // }
            // if (move.y > 0.5f) {
            //     SimKeyboard.Press((byte)'W');
            // }
            // if (move.y < 0.5f) {
            //     SimKeyboard.Press((byte)'S');
            // }
		}

        if (OVRInput.GetDown(OVRInput.Button.One)) {
            //RamjetInput.DirectInput.SendKey(0x11);
            //WarhammerMan.Input.PressKey('w', true);
            DirectInput.Input.SendKey((short)0x11); // YES THIS WORKS!
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

