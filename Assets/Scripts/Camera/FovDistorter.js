#pragma strict

@script RequireComponent(Camera)
private var cam : Camera;

public var distort = 30.0;
public var damping = 3.0;
private var targetDistort = 0.0;
private var baseFov = 0.0;



function Awake() {
    cam = GetComponent(Camera);
    baseFov = cam.fieldOfView;
}

function Update() {}

public function distortFov() {
    targetDistort = baseFov + distort;
    // print("distorting");
}

function LateUpdate() {
    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetDistort, damping*Time.deltaTime);
    targetDistort = baseFov; // reset, so if distortFov() isn't called we go back to normal
    // print(cam.fieldOfView);
}