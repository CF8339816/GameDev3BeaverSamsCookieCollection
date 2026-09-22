using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Hands : MonoBehaviour
{
    public static Hands instance;

    [SerializeField] Transform _leftHand;
    [SerializeField] Transform _rightHand;

    [Header("Camera follow")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Vector3 cameraOffset = Vector3.zero;

    [Header("Rest position (relative to camera)")]
    [SerializeField] private Vector3 leftHandRestOffset = new Vector3(-0.2f, -0.25f, 0.5f);
    [SerializeField] private Vector3 rightHandRestOffset = new Vector3(0.2f, -0.25f, 0.5f);

    [Header("Render on top (avoid wall clipping)")]
    [SerializeField] private string handsLayerName = "Hands";
    [SerializeField] private float handsNearClip = 0.01f;

    [Header("Walk wobble")]
    [SerializeField] private float wobbleSpeed = 8f;
    [SerializeField] private float wobbleAmount = 0.02f;
    [SerializeField] private float wobbleSmoothing = 8f;

    [Header("Cookie reach")]
    [SerializeField] private Vector3 reachOffset = new Vector3(0f, -0.05f, 0.4f);
    [SerializeField] private Vector3 grabSpread = new Vector3(0.08f, 0f, 0f);
    [SerializeField] private float reachDuration = 0.25f;
    [SerializeField] private float holdDuration = 0.15f;

    private Vector3 _leftHandStartPos;
    private Vector3 _rightHandStartPos;
    private float _wobbleTimer;
    private bool _isReaching;
    private Camera _handsCamera;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _leftHandStartPos = leftHandRestOffset;
        _rightHandStartPos = rightHandRestOffset;

        if (_leftHand != null) _leftHand.localPosition = _leftHandStartPos;
        if (_rightHand != null) _rightHand.localPosition = _rightHandStartPos;

        if (playerCamera == null) playerCamera = Camera.main;

        SetupHandsCamera();
    }

    void LateUpdate()
    {
        if (playerCamera == null) return;

        Vector3 targetPos = playerCamera.transform.TransformPoint(cameraOffset);
        transform.SetPositionAndRotation(targetPos, playerCamera.transform.rotation);
    }

    void Update()
    {
        if (_isReaching) return;

        float horizontalSpeed = FPS_Controller.horizontalSpeed;

        if (horizontalSpeed > 0.1f)
        {
            _wobbleTimer += Time.deltaTime * wobbleSpeed * (horizontalSpeed / 5f + 1f);
        }

        float bobY = Mathf.Sin(_wobbleTimer) * wobbleAmount;
        float bobX = Mathf.Cos(_wobbleTimer * 0.5f) * wobbleAmount * 0.5f;
        Vector3 wobbleOffset = horizontalSpeed > 0.1f ? new Vector3(bobX, bobY, 0f) : Vector3.zero;

        if (_leftHand != null)
        {
            _leftHand.localPosition = Vector3.Lerp(_leftHand.localPosition, _leftHandStartPos + wobbleOffset, wobbleSmoothing * Time.deltaTime);
        }
        if (_rightHand != null)
        {
            _rightHand.localPosition = Vector3.Lerp(_rightHand.localPosition, _rightHandStartPos - wobbleOffset, wobbleSmoothing * Time.deltaTime);
        }
    }

    private void SetupHandsCamera()
    {
        if (playerCamera == null) return;

        int handsLayer = LayerMask.NameToLayer(handsLayerName);
        if (handsLayer < 0)
        {
            Debug.LogWarning($"Hands: layer '{handsLayerName}' does not exist. Create it as a NEW User Layer (index 8-31) in Project Settings > Tags and Layers to enable overlay rendering.");
            return;
        }

        if (handsLayer == 0)
        {
            Debug.LogError("Hands: layer '" + handsLayerName + "' resolves to layer 0 (Default). This would hide the rest of the scene from the main camera. Create a NEW User Layer (index 8-31) named '" + handsLayerName + "' instead of renaming the Default layer.");
            return;
        }

        SetLayerRecursively(_leftHand != null ? _leftHand.gameObject : null, handsLayer);
        SetLayerRecursively(_rightHand != null ? _rightHand.gameObject : null, handsLayer);

        // stop the main camera from rendering the hands so they don't get clipped by world geometry
        playerCamera.cullingMask &= ~(1 << handsLayer);

        GameObject camObj = new GameObject("HandsCamera");
        camObj.transform.SetParent(playerCamera.transform, false);
        _handsCamera = camObj.AddComponent<Camera>();
        _handsCamera.CopyFrom(playerCamera);
        _handsCamera.cullingMask = 1 << handsLayer;
        _handsCamera.clearFlags = CameraClearFlags.Depth;
        _handsCamera.depth = playerCamera.depth + 1;
        _handsCamera.nearClipPlane = handsNearClip;

        // URP requires overlay cameras to be explicitly added to the base camera's stack,
        // simply setting depth (like in the Built-in pipeline) is not enough.
        var handsCameraData = _handsCamera.GetUniversalAdditionalCameraData();
        handsCameraData.renderType = CameraRenderType.Overlay;

        var baseCameraData = playerCamera.GetUniversalAdditionalCameraData();
        if (baseCameraData.renderType == CameraRenderType.Base && !baseCameraData.cameraStack.Contains(_handsCamera))
        {
            baseCameraData.cameraStack.Add(_handsCamera);
        }
    }

    private static void SetLayerRecursively(GameObject obj, int layer)
    {
        if (obj == null) return;

        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public Coroutine ReachForCookie(Transform cookie, Action onReached)
    {
        return StartCoroutine(ReachRoutine(cookie, onReached));
    }

    private IEnumerator ReachRoutine(Transform cookie, Action onReached)
    {
        _isReaching = true;

        Vector3 leftTarget = _leftHandStartPos + reachOffset;
        Vector3 rightTarget = _rightHandStartPos + reachOffset;

        if (cookie != null && _leftHand != null && _rightHand != null)
        {
            Vector3 localCookiePos = _leftHand.parent.InverseTransformPoint(cookie.position);
            leftTarget = localCookiePos + grabSpread;
            rightTarget = localCookiePos - grabSpread;
        }

        yield return MoveHands(leftTarget, rightTarget, reachDuration);

        yield return new WaitForSeconds(holdDuration);

        onReached?.Invoke();

        yield return MoveHands(_leftHandStartPos, _rightHandStartPos, reachDuration);

        _isReaching = false;
    }

    private IEnumerator MoveHands(Vector3 leftTarget, Vector3 rightTarget, float duration)
    {
        Vector3 leftStart = _leftHand != null ? _leftHand.localPosition : Vector3.zero;
        Vector3 rightStart = _rightHand != null ? _rightHand.localPosition : Vector3.zero;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            if (_leftHand != null) _leftHand.localPosition = Vector3.Lerp(leftStart, leftTarget, t);
            if (_rightHand != null) _rightHand.localPosition = Vector3.Lerp(rightStart, rightTarget, t);

            yield return null;
        }

        if (_leftHand != null) _leftHand.localPosition = leftTarget;
        if (_rightHand != null) _rightHand.localPosition = rightTarget;
    }
}
