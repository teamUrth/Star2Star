using System.Collections;
using UnityEngine;

[
    ExecuteInEditMode,                                       // Run this script in edit mode so the preview window looks good
    RequireComponent(typeof(Camera)),                       // Only add this component if there is a camera
    HelpURL("http://ggez.org/posts/perfect-pixel-camera/"), // Website opened by clicking the book icon on the component
    DisallowMultipleComponent,                               // Only one of these per GameObject
    AddComponentMenu("GGEZ/Camera/Perfect Pixel Camera")    // Insert into the "Add Component..." menu
]
public class PerfectPixelCamera : GameEvent
{
    [
        Tooltip("The number of texture pixels that fit in 1.0 world units. Common values are 8, 16, 32 and 64. If you're making a tile-based game, this is your tile size."),
        Range(1, 256)
    ]
    public int TexturePixelsPerWorldUnit = 16;
    public GameObject Spectate;
    public bool SpectateMode;
    
    private Vector3 _position;
    private Camera cameraComponent;

#if UNITY_5_5_OR_NEWER
    private const float halfPixelOffsetIfNeededForD3D = 0f;
#else
private float halfPixelOffsetIfNeededForD3D;
#endif


    public float SnapSizeWorldUnits { get; private set; }

    void OnEnable()
    {
        // Grab a reference to the camera
        cameraComponent = (Camera)GetComponent(typeof(Camera));

#if !UNITY_5_5_OR_NEWER

    // Detect whether we are using Direct3D, because D3D rendering has a
    // half-pixel offset from OpenGL rendering.
    bool isD3D =
            SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D9
            || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11
            || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D12;

    // 0.4975f and not 0.5f is used because 0.5f is able to be represented
    // as a perfect IEEE float. This means that when added to other
    // floats that are imperfect, the results can sometimes be rounded
    // the wrong way. It can be tricky to reproduce so this isn't part
    // of the main demo.
    this.halfPixelOffsetIfNeededForD3D = isD3D ? 0.4975f : 0f;

#endif

        // Run the LateUpdate immediately so that the projection gets set up
        LateUpdate();
    }

    void OnDisable()
    {
        if (cameraComponent == null)
        {
            return;
        }
        cameraComponent.ResetProjectionMatrix();
        cameraComponent = null;
    }

    /*
    void Awake()
    {
        Screen.SetResolution(512, 288, false);
        //Screen.SetResolution(512 * 4, 288 * 4, false);
        //Screen.SetResolution(4096, 2304, false);
    }*/
    public override void Initialize()
    {
        Speed = 1;
        Frequency = 5;
    }

    public void SetCameraPlayer()
    {
        Debug.Log("카메라를 플레이어로");
        Spectate = GameManager.PlayerCode.gameObject;
    }

    public void SetCamera()
    {
        Debug.Log("카메라 ID : " + Id);
        Spectate = gameObject;
    }

    void LateUpdate()
    {
        // Get a local reference
        Camera camera = cameraComponent;
        
        /*내가 추가한 사항*/
        if (SpectateMode)
        {
            if (Spectate.transform.position.x <= MapManager.ScreenX)
            {
                _position.x = MapManager.ScreenX;
            }
            else if (Spectate.transform.position.x >= MapManager.ScreenWidth)
            {
                _position.x = MapManager.ScreenWidth;
            }
            else
            {
                _position.x = Spectate.transform.position.x;
            }

            if (Spectate.transform.position.y >= MapManager.ScreenY)
            {
                _position.y = MapManager.ScreenY;
            }
            else if (Spectate.transform.position.y <= MapManager.ScreenHeight)
            {
                _position.y = MapManager.ScreenHeight;
            }
            else
            {
                _position.y = Spectate.transform.position.y;
            }
            transform.position = _position;
        }

        // Make sure the camera is in 2D mode
        camera.transparencySortMode = TransparencySortMode.Orthographic;
        camera.orthographic = true;
        camera.transform.rotation = Quaternion.identity;
        camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.00001f);

        // This is the code that computes the parameters needed to perfectly map
        // world-space pixels to screen-space pixels.
        var pixelRect = camera.pixelRect;
        float texturePixelsPerWorldUnit = this.TexturePixelsPerWorldUnit;
        float zoomFactor = Mathf.Max(1f, Mathf.Ceil((1f * pixelRect.height) / (camera.orthographicSize * 2f * texturePixelsPerWorldUnit)));
        float halfWidth = (1f * pixelRect.width) / (zoomFactor * 2f * texturePixelsPerWorldUnit);
        float halfHeight = (1f * pixelRect.height) / (zoomFactor * 2f * texturePixelsPerWorldUnit);
        float snapSizeWorldUnits = 1f / (zoomFactor * texturePixelsPerWorldUnit);
        float halfPixelOffsetInWorldUnits = halfPixelOffsetIfNeededForD3D * snapSizeWorldUnits;
        float pixelPerfectXOffset = halfPixelOffsetInWorldUnits - Mathf.Repeat(snapSizeWorldUnits + Mathf.Repeat(camera.transform.position.x, snapSizeWorldUnits), snapSizeWorldUnits);
        float pixelPerfectYOffset = halfPixelOffsetInWorldUnits - Mathf.Repeat(snapSizeWorldUnits + Mathf.Repeat(camera.transform.position.y, snapSizeWorldUnits), snapSizeWorldUnits);

        // Save the snap size so other scripts can use it
        this.SnapSizeWorldUnits = snapSizeWorldUnits;

        // Build a manual projection matrix that fixes the camera!
        camera.projectionMatrix = Matrix4x4.Ortho(
                -halfWidth + pixelPerfectXOffset,
                 halfWidth + pixelPerfectXOffset,
                -halfHeight + pixelPerfectYOffset,
                 halfHeight + pixelPerfectYOffset,
                camera.nearClipPlane,
                camera.farClipPlane
                );
        /*내가 추가한 사항*/
        camera.transparencySortMode = TransparencySortMode.CustomAxis;
        //camera.transparencySortAxis = new Vector3(0, 1, 0);
    }

    public void Scroll(Vector3 direction)
    {
        StartCoroutine(ScrollMap(direction));
    }

    public void Scroll(Vector3 direction, string name)
    {
        StartCoroutine(ScrollMap(direction, name));
    }

    public IEnumerator ScrollMap(Vector3 direction)
    {
        WaitForSeconds wait = new WaitForSeconds(.01f);
        SpectateMode = false;
        GameManager.isAction = true;
        // 방향에 따라 이동 값을 설정
        var block = 9;
        if (direction == Vector3.left || direction == Vector3.right)
        {
            block = 16;
        }
        for (var i = 0; i < GameManager.PPU * block / 4; i++)
        {
            transform.Translate(direction * GameManager.Pixel * 4);
            yield return wait;
        }
        SpectateMode = true;
        MapManager.Instance.SetScreen();
    }
    
    public IEnumerator ScrollMap(Vector3 direction, string name)
    {
        WaitForSeconds wait = new WaitForSeconds(.01f);
        GameManager.isAction = true;
        SpectateMode = false;
        // 방향에 따라 이동 값을 설정
        var block = 9;
        if (direction == Vector3.left || direction == Vector3.right)
        {
            block = 16;
        }
        for (var i = 0; i < GameManager.PPU * block / 4; i++)
        {
            transform.Translate(direction * GameManager.Pixel * 4);
            yield return wait;
        }
        SpectateMode = true;
        MapManager.Instance.SetScreen(name);
    }

    public void ShakeScreen(int value, int duration)
    {
        StartCoroutine(Shake(value, duration));
    }

    public IEnumerator Shake(int value, int duration)
    {
        Debug.Log("실행중");
        float timer = 0;
        GameManager.isAction = true;
        SpectateMode = false;
        Vector3 position = transform.position;
        while (timer <= duration)
        {
            transform.position = (Vector3)Random.insideUnitCircle * value + position;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = position;
        GameManager.isAction = false;
        SpectateMode = true;
    }
}
