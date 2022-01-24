using System.Collections;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [Header("Reward Score")]
    [SerializeField]
    private int ringScore = 10;

    [SerializeField]
    private int scoreDoubleLevel = 1;

    [SerializeField]
    private int maxLevel = 3;

    [Header("Fever")]
    [SerializeField]
    private bool isFever = false;

    [SerializeField]
    private int feverScore = 100;

    [SerializeField]
    private float feverScale = 0.4f;

    [SerializeField]
    private RainbowColor rainbowColor;

    [SerializeField]
    private ParticleSystem feverEffect;

    private Vector3 originScale;

    [Header("Force")]
    [SerializeField]
    private float maxForce = 200f;

    [SerializeField]
    private float addForceMaxSpeed = 10f;

    [SerializeField]
    private float addForceMinSpeed = 5f;

    private float currentForce;

    private float currentVelocity;
    private Vector3 currentRotateVelocity;

    private bool hasForce = false;
    private Transform[] WayPointsTrans;

    private Waypoints waypoints;

    [Header("Physics")]
    [SerializeField]
    private bool isHitTarget;

    [SerializeField]
    private float effectDistance = 5f; 

    [SerializeField]
    private float rotateSpeedMin = 3.5f;

    [SerializeField]
    private float rotateSpeedMax = 5.0f;

    [SerializeField]
    private Rigidbody _rigidbody;

    [Header("Effect")]

    [SerializeField]
    private TrailRenderer trailEffect;

    [SerializeField]
    private ParticleSystem upgradeEffect;

    [SerializeField]
    private ParticleSystem starsEffect;

    [SerializeField]
    private PulseColor pulseColorEffect;

    private Coroutine lerpPositionCor;

    private GameInput gameInput;

    public bool HitTarget
    {
        get
        {
            return isHitTarget;
        }

        set
        {
            isHitTarget = value;
            currentForce = 0f;
            _rigidbody.velocity = Vector3.down;
        }
    }


    public int CurrentScore { get; private set; }

    public int GetScoreDoubleLevel => scoreDoubleLevel;

    public bool IsFever => isFever;

    private void Awake()
    {
        gameInput = FindObjectOfType<GameInput>();
        waypoints = FindObjectOfType<Waypoints>();
    }

    private void Start()
    {
        CurrentScore = ringScore;
        originScale = transform.localScale;
        ParticleSystem.MainModule mainModule = feverEffect.main;
        mainModule.startSize = feverScale;
        WayPointsTrans = waypoints.GetWayPoints(Waypoints.WayTag.Low);
    }

    private void OnEnable()
    {
        gameInput.OnTouchingAnyScreen += IncreaseForce;
        gameInput.OnTouchStrengthEvent += SelectStrengthWaypoints;
    }


    private void OnDisable()
    {
        gameInput.OnTouchingAnyScreen -= IncreaseForce;
        gameInput.OnTouchStrengthEvent -= SelectStrengthWaypoints;
    }

    private void IncreaseForce()
    {
        // currentForce = Random.Range(350f, 550f);
        //currentForce = Random.Range(150f, 250f);

        if (currentForce < maxForce)
            currentForce += Random.Range(addForceMinSpeed, addForceMaxSpeed);
    }


    private void SelectStrengthWaypoints(float strength)
    {
        if (waypoints == null)
            return;

        if (strength > 0.8f)
        {
            WayPointsTrans = waypoints.GetWayPoints(Waypoints.WayTag.High);
        }
        else if (strength > 0.5f)
        {
            WayPointsTrans = waypoints.GetWayPoints(Waypoints.WayTag.Normal);
        }
        else
        {
            WayPointsTrans = waypoints.GetWayPoints(Waypoints.WayTag.Low);
        }
    }

    public void InitCustomizeSetting()
    {
        ChangeColor changeColor = GetComponent<ChangeColor>();
        if (changeColor)
        {
            MaterialPropertyBlock materialProperty = new MaterialPropertyBlock();
            ParticleSystemRenderer renderer = upgradeEffect.GetComponent<ParticleSystemRenderer>();

            renderer.GetPropertyBlock(materialProperty);
            materialProperty.SetColor("_Color", changeColor.PropertyColor);
            renderer.SetPropertyBlock(materialProperty);


            ParticleSystem.TrailModule trailModule = upgradeEffect.trails;
            trailModule.colorOverLifetime = changeColor.PropertyColor;
        }
    }

    private void Update()
    {
        TrailEffect();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (currentForce <= 50f)
        {
            hasForce = false;
            return;
        }

        if (!hasForce)
        {
            hasForce = true;
            _rigidbody.velocity = Vector3.zero;
        }

        //if (currentForce <= 50f)
        //{
        //    currentForce -= 0.15f;
        //    AddForce(_rigidbody.velocity.normalized, currentForce);
        //    return;
        //}


        currentForce = Mathf.SmoothDamp(currentForce, 50f, ref currentVelocity, Random.Range(.5f, 1f));
        //currentForce -= Random.Range(0.5f, 2.5f);

        float minimumDistance = 10f;
        int index = -1;

        if(WayPointsTrans != null)
        {
            for (int i = 0; i < WayPointsTrans.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, WayPointsTrans[i].position);
                if (effectDistance < distance)
                    continue;

                if (minimumDistance >= distance)
                {
                    minimumDistance = distance;
                    index = i;
                }
            }
        }

        if (index == -1)
            return;

        float singleStep = Random.Range(rotateSpeedMin, rotateSpeedMax) * Time.fixedDeltaTime;

        //Vector3 smoothDir = Vector3.Lerp(_rigidbody.velocity.normalized, transforms[index].up, 0.55f);
        //Vector3 newDirection = Vector3.RotateTowards(_rigidbody.velocity.normalized, WayPointsTrans[index].up, singleStep, singleStep);
        Vector3 newDirection = Vector3.SmoothDamp(_rigidbody.velocity.normalized, WayPointsTrans[index].up, ref currentRotateVelocity, singleStep);

#if UNITY_EDITOR
        Debug.DrawRay(transform.position, newDirection, Color.red);
#endif
        AddForce(newDirection, currentForce);
    }

    private void TrailEffect()
    {
        if (_rigidbody.velocity.y > 0)
        {
            trailEffect.time = 0.5f;
        }
        else
        {
            trailEffect.time = 0.25f;
        }
    }

    private void LevelEffect(int level)
    {
        switch (level)
        {
            case 1:
                // Level 1. No any effect.
                if (pulseColorEffect)
                    pulseColorEffect.enabled = false;
                starsEffect.Stop();
                break;
            case 2:
                // Level 2. Add pulse color effect.
                if (pulseColorEffect)
                    pulseColorEffect.enabled = true;
                break;
            case 3:
                // Level 3. Add stars effect.
                starsEffect.Play();
                break;
            default:
                break;
        }
    }

    public void AddScoreDoubleLevel()
    {
        if (scoreDoubleLevel >= maxLevel)
            return;

        scoreDoubleLevel++;

        if (isFever)
            return;

        upgradeEffect.Play();
        LevelEffect(scoreDoubleLevel);
    }

    public void AddForce(Vector3 dir, float force)
    {
        if (isHitTarget)
            return;

        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.velocity = dir * force * Time.fixedDeltaTime;
    }

    public void AddExplosionForce(Vector3 position, float force, float radius, float upwards, ForceMode forceMode)
    {
        if (isHitTarget)
            return;

        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddExplosionForce(force, position, radius, upwards, forceMode);
    }



    public void SetPosition(Vector3 position)
    {

        if(lerpPositionCor != null)
        {
            StopCoroutine(lerpPositionCor);
            lerpPositionCor = null;
        }

        lerpPositionCor = StartCoroutine(LerpPosition(position));
    }

    public void SetFeverRing(bool isFever)
    {
        this.isFever = isFever;

        if (this.isFever)
        {
            LevelEffect(1);

            CurrentScore = feverScore;
            transform.localScale = Vector3.one * feverScale;
            rainbowColor.enabled = true;
            feverEffect.Play();
        }
        else
        {
            LevelEffect(scoreDoubleLevel);

            CurrentScore = ringScore;
            transform.localScale = originScale;
            rainbowColor.enabled = false;
            feverEffect.Stop();
        }
    }

    private IEnumerator LerpPosition(Vector3 newPosition)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        float normalize = 0f;

        while (1f > normalize)
        {
            newPosition.y = transform.position.y;
            _rigidbody.MovePosition(Vector3.Lerp(transform.position , newPosition, normalize));

            normalize += Time.fixedDeltaTime;
            yield return waitForEndOfFrame;
        }

        _rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        lerpPositionCor = null;
    }

}
