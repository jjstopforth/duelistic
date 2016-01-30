using UnityEngine;
using System.Collections;

public class TempFloatText : MonoBehaviour {

    public float FloatingTime = 1f;
    public float FadeTime = 1f;
    Vector3 _originalPos = Vector3.one;
    public Vector3 _originalScale = Vector3.one;

    public Vector3 DeltaPos = Vector3.up;
    public Vector3 DeltaScale = Vector3.one*2;
    private TextMesh textMesh;
    public Color FadeColor;
    Color _originialColor;
    public EasingType ColorEasingType;
    public EasingType PosEasingType;
    public EasingType ScaleEasingType;

    void Awake()
    {
        _originalPos = gameObject.transform.position;
        textMesh = GetComponent<TextMesh>();
        _originialColor=textMesh.color;
        //FadeColor = _originialColor;
        //_fadeColor.a
        //_originalScale = gameObject.transform.localScale;
    }
	// Use this for initialization
	void OnEnable ()
    {
        gameObject.transform.position = _originalPos;
        gameObject.transform.localScale = _originalScale;
	    textMesh.color = _originialColor;
	    StartCoroutine(TimeRoutine());
	}

    IEnumerator TimeRoutine()
    {
        float deltaTime = 0f;
        Vector3 originalPos = gameObject.transform.position;
        Vector3 originalScale = gameObject.transform.localScale;
        while (deltaTime < FloatingTime)
        {
            float t = 0f;
            if (!Mathf.Approximately(0f, deltaTime))
            {
                t = deltaTime/FloatingTime;
            }

            SetHeight(_originalPos,t);
            SetScale(_originalScale,t);
            yield return new WaitForFixedUpdate();
            deltaTime += Time.deltaTime;
        }
        SetHeight(_originalPos + DeltaPos, 1f);
        SetScale(_originalScale + DeltaScale, 1f);
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float deltaTime = 0f;
        while (deltaTime < FadeTime)
        {
            float t = 0f;
            if (!Mathf.Approximately(0f, deltaTime))
            {
                t = deltaTime / FadeTime;
            }
            SetAlpha(_originialColor, t);
            yield return new WaitForFixedUpdate();
            deltaTime += Time.deltaTime;
        }
        SetAlpha(FadeColor, 1f);
        Destroy(gameObject);
    }
    void SetAlpha(Color color, float t)
    {
        textMesh.color = Easing.Ease(color, FadeColor, t, ColorEasingType);
    }
    void SetHeight(Vector3 originalPos, float t)
    {
        Vector3 pos = Easing.Ease(originalPos, _originalPos + DeltaPos, t, PosEasingType);
        gameObject.transform.position = pos;
    }
    void SetScale(Vector3 originalScale, float t)
    {
        Vector3 scale = Easing.Ease(originalScale, _originalScale + DeltaScale, t, ScaleEasingType);
        gameObject.transform.localScale = scale;
    }
}
