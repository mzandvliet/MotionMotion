using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimWinInput;

/* Observations:
- My right shoulder gets tired from holding up the aim controller all the time
This is mostly because it has to hold awkward poses for long stretches of time
We need a better rest/neutral mode from which you can still do lots of things
- Using more of your body to control quake does feel like it has a lot of potential
Sometimes everything clicked, and I flicked my aim around to hit an instant rail.
It's possible.
- Interpreting/mapping input like this has infinity options, some of which are good
- Target tracing is still hard, flicks are not very precise yet either
- Sometimes push turn, precise aim and torque turn conflict and confuse

 */

public class MotionMotion : MonoBehaviour {
    private ControllerState _left;
    private ControllerState _right;

	private bool _active = true;

	private void Awake() {
		Application.runInBackground = true;
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

        float x = 0f;
        float y = 0f;

        // Shift look up and down by linearly moving controller
        // Todo: linear, but seen from which perspective? Hands and arms trace arcs. Use that.
        x += _right.LinearVelocity.x * (0.5f + 0.5f * indexR) * 100f;
        y += _right.LinearVelocity.y * (0.5f + 0.5f * indexR) * -100f;

        // Shift look by rotating
        x += _right.AngularVelocity.y * (0.5f + 0.5f * indexR) * 13f;
		y += _right.AngularVelocity.x * (0.5f + 0.5f * indexR) * 13f;

        // Push look by rotating
        float angleX = AngleAroundAxis(_right.Rotation * Vector3.forward, Vector3.forward, Vector3.up);
        float angleY = AngleAroundAxis(_right.Rotation * Vector3.forward, Vector3.forward, Vector3.right);
        x += (Mathf.Sign(angleX) * Mathf.Pow(Mathf.Min(Mathf.Abs(angleX) / 75f, 1f), 2f)) * -120f;
        y += (Mathf.Sign(angleY) * Mathf.Pow(Mathf.Min(Mathf.Abs(angleY) / 75f, 1f), 2f)) * -120f;

        if (_active) {
            // Look
			SimMouse.ActRaw(SimMouse.Action.MoveOnly, Mathf.RoundToInt(x), Mathf.RoundToInt(y));

            //Vector2 moveAnalog = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

            // Shoot
            HandleKey(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch, DirectInput.ScanCode.H);

            // Move            
            HandleKey(OVRInput.Button.Up, OVRInput.Controller.LTouch, DirectInput.ScanCode.W);
            HandleKey(OVRInput.Button.Down, OVRInput.Controller.LTouch, DirectInput.ScanCode.S);
            HandleKey(OVRInput.Button.Left, OVRInput.Controller.LTouch, DirectInput.ScanCode.A);
            HandleKey(OVRInput.Button.Right, OVRInput.Controller.LTouch, DirectInput.ScanCode.D);

            HandleKey(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch, DirectInput.ScanCode.Space);
            HandleKey(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch, DirectInput.ScanCode.LControl);

            // Switch weapons
            HandleKey(OVRInput.Button.Left, OVRInput.Controller.RTouch, DirectInput.ScanCode.Q);
            HandleKey(OVRInput.Button.Right, OVRInput.Controller.RTouch, DirectInput.ScanCode.E);
            HandleKey(OVRInput.Button.One, OVRInput.Controller.RTouch, DirectInput.ScanCode.R);
            HandleKey(OVRInput.Button.Up, OVRInput.Controller.RTouch, DirectInput.ScanCode.Two);
            HandleKey(OVRInput.Button.Down, OVRInput.Controller.RTouch, DirectInput.ScanCode.Three);
            HandleKey(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch, DirectInput.ScanCode.One);

            // Zoom
            HandleKey(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch, DirectInput.ScanCode.J);
        }
    }

    private void OnGUI() {
        GUILayout.BeginVertical(GUI.skin.box);
        {
            GUILayout.Label("Focus: " + OVRManager.hasVrFocus +
            ", Left: " + OVRInput.IsControllerConnected(OVRInput.Controller.LTouch) +
            ", Right: " + OVRInput.IsControllerConnected(OVRInput.Controller.RTouch));
        }
        GUILayout.EndVertical();
    }

    public static float AngleAroundAxis(Vector3 a, Vector3 b, Vector3 axis)
    {
        return Mathf.Atan2(
            Vector3.Dot(axis, Vector3.Cross(a, b)),
            Vector3.Dot(a, b)
            ) * Mathf.Rad2Deg;
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

