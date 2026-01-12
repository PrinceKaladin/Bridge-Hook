using UnityEngine;
using UnityEngine.UI;

public class FisherCastController : MonoBehaviour
{
    [Header("Fisher Objects")]
    public GameObject idleFisher;
    public GameObject castFisher;

    [Header("Refs")]
    public Transform castOrigin;
    public LineRenderer line;

    [Header("Power UI")]
    public GameObject sliderRoot; // показать/скрыть
    public Slider powerSlider;    // 0..1

    [Header("Hook")]
    public HookController hookPrefab;
    public float minForce = 5f;
    public float maxForce = 18f;
    public Vector2 throwDirection = new Vector2(1f, 0.6f);

    [Header("Power Move")]
    public float powerSpeed = 1.2f;

    [Header("Cast Lock")]
    public float minTimeBetweenCasts = 0.2f;

    bool aiming;
    float t;
    float lastCastTime;
    HookController currentHook;

    void Start()
    {
        SetFisherIdle();
    }

    void Update()
    {
        // 1) Тапы (мышь/тач)
        if (Input.GetMouseButtonDown(0))
        {
            // если уже есть крючок — не даём начать новый прицел
            if (!aiming && currentHook == null && Time.time - lastCastTime >= minTimeBetweenCasts)
                BeginAim();
            else if (aiming)
                ConfirmAndCast();
        }

        // 2) Движение силы
        if (aiming)
        {
            t += Time.deltaTime * powerSpeed;
            float ping = Mathf.PingPong(t, 1f);
            if (powerSlider) powerSlider.value = ping;
        }

        // 3) Леска
        if (currentHook != null && line != null)
        {
            line.enabled = true;
            line.SetPosition(0, castOrigin.position);
            line.SetPosition(1, currentHook.transform.position);
        }
        else
        {
            if (line != null) line.enabled = false;
        }
    }

    void BeginAim()
    {
        aiming = true;
        t = 0f;
        if (sliderRoot) sliderRoot.SetActive(true);
        SetFisherIdle();
    }

    void ConfirmAndCast()
    {
        aiming = false;
        if (sliderRoot) sliderRoot.SetActive(false);

        float power01 = powerSlider ? powerSlider.value : Mathf.PingPong(t, 1f);
        float force = Mathf.Lerp(minForce, maxForce, power01);

        // ВАЖНО: включаем CastFisher и держим его
        SetFisherCast();

        currentHook = Instantiate(hookPrefab, castOrigin.position, Quaternion.identity);
        currentHook.Init(this);
        currentHook.Launch(throwDirection * force);

        lastCastTime = Time.time;
    }

    public void ClearHook()
    {
        currentHook = null;

        // теперь можно вернуть idle
        SetFisherIdle();
    }

    void SetFisherIdle()
    {
        idleFisher.SetActive(true);
        castFisher.SetActive(false);
    }

    void SetFisherCast()
    {
        idleFisher.SetActive(false);
        castFisher.SetActive(true);
    }
}
