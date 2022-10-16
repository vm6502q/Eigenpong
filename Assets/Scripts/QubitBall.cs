using System.Collections;
using System.Collections.Generic;
using OpenRelativity;
using OpenRelativity.Objects;
using Qrack;
using UnityEngine;

public class QubitBall : MonoBehaviour
{
    public PlayerUI[] playerUI;
    public Color qubit0Color = Color.blue;
    public Color qubit1Color = Color.red;
    public bool isPhasing = false;
    // public ParticleSystem[] particleSystems;
    protected QubitBallSystem qubitBallSystem;
    protected QuantumRegister qubit;
    protected RelativisticObject relativisticObject;
    protected Vector3 startingPiw;
    protected uint qubitId;
    protected float qubitProb;
    protected float timer;
    protected float lastResetTime;
    protected bool didServe;

    protected const float PHASE_PERIOD = 4.0f;
    protected const float SERVE_DELAY = 5.0f;
    protected const float SERVE_SPEED = 24.0f;
    protected const float SERVE_DEGREES = 16.0f;
    protected const float BOUNCE_FACTOR = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        qubitBallSystem = FindObjectOfType<QubitBallSystem>();
        qubitId = qubitBallSystem.Allocate(this);
        qubit = GetComponent<QuantumRegister>();
        qubit.QuantumSystemMappings[0] = qubitId;
        qubit.QubitCount = 1;
        qubitProb = 0.5f;

        relativisticObject = GetComponent<RelativisticObject>();
        startingPiw = relativisticObject.piw;
        lastResetTime = qubit.LocalDeltaTime;
        timer = 0;
        didServe = false;

        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (relativisticObject.state.isMovementFrozen)
        {
            return;
        }

        // for (int i = 0; i < particleSystems.Length; i++)
        // {
        //     ParticleSystem.MainModule main = particleSystems[i].main;
        //     main.simulationSpeed = relativisticObject.GetTimeFactor();
        // }

        if (!didServe && ((qubit.LocalTime - lastResetTime) > SERVE_DELAY))
        {
            Serve();
        }

        if (!isPhasing)
        {
            return;
        }

        timer += qubit.LocalDeltaTime;
        if (timer > PHASE_PERIOD) {
            timer -= PHASE_PERIOD;
            HZ();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        string tag = collider.gameObject.tag;
        if (tag == "Goal0")
        {
            Score(0);
        }
        else if (tag == "Goal1")
        {
            Score(1);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        string tag = collision.collider.gameObject.tag;
        if (tag == "Player0")
        {
            ApplyGate(0);
        }
        else if (tag == "Player1")
        {
            ApplyGate(1);
        }

        Vector3 vel = relativisticObject.peculiarVelocity;
        float x = vel.x * vel.x;
        float y = vel.y * vel.y;

        if ((y > x) && (y > (16 * x)))
        {
            if (y > (128 * x))
            {
                Score(relativisticObject.piw.x > 0 ? 1 : 0);
                Reset();
            }

            return;
        }

        if ((x > y) && (x > (32 * y)))
        {
            return;
        }

        vel = vel.AddVelocity((BOUNCE_FACTOR - 1.0f) * vel);
        float maxVel = relativisticObject.state.maxPlayerSpeed;
        if (vel.sqrMagnitude < (maxVel * maxVel))
        {
            relativisticObject.peculiarVelocity = vel;
        }
        else
        {
            Score(relativisticObject.piw.x > 0 ? 1 : 0);
            if (qubitBallSystem.ballsInPlay == 1)
            {
                Reset();
            }
            else
            {
                qubitBallSystem.Release(qubitId);
                Reset();
                gameObject.SetActive(false);
            }
        }
    }

    protected void Reset()
    {
        qubitProb = 0.5f;
        relativisticObject.peculiarVelocity = Vector3.zero;
        relativisticObject.piw = startingPiw;
        lastResetTime = qubit.LocalTime;
        timer = 0.0f;
        didServe = false;

        UpdateColor();
    }

    protected void UpdateColor() {
        // for (int i = 0; i < particleSystems.Length; i++) {
        //     ParticleSystem.MainModule main = particleSystems[i].main;
        //     main.startColor = Color.Lerp(qubit0Color, qubit1Color, qubitProb);
        // }

        MeshRenderer myRenderer = GetComponent<MeshRenderer>();
        for (int i = 0; i < myRenderer.materials.Length; ++i) {
            myRenderer.materials[i].color = Color.Lerp(qubit0Color, qubit1Color, qubitProb);
        }
    }

    protected void UpdateProbAndColor()
    {
        qubitProb = qubit.Prob(0);
        UpdateColor();
    }

    protected void Score(int goal)
    {
        qubitProb = qubit.M(0) ? 1.0f : 0.0f;

        // TODO: Add to tally

        Reset();
    }

    protected void Serve()
    {
        qubit.M(0);
        qubit.H(0);
        bool up = qubit.M(0);
        qubit.H(0);
        bool left = qubit.M(0);
        qubitProb = left ? 1.0f : 0.0f;

        UpdateColor();

        Vector3 vel = SERVE_SPEED * (Quaternion.AngleAxis(SERVE_DEGREES, up ? Vector3.forward : Vector3.back) * (left ? Vector3.left : Vector3.right));
        relativisticObject.peculiarVelocity = vel;

        didServe = true;
        timer = 0.0f;
    }

    void HZ() {
        qubit.H(0);
        qubit.Z(0);

        qubitProb = qubit.Prob(0);

        UpdateColor();
    }

    void ApplyGate(int player)
    {
        if (playerUI[player].x.isActive)
        {
            qubit.X(0);
        }
        if (playerUI[player].h.isActive)
        {
            qubit.H(0);
        }
        if (playerUI[player].z.isActive)
        {
            qubit.Z(0);
        }
        if (playerUI[player].s.isActive)
        {
            qubit.S(0);
        }

        if (playerUI[player].control.axis == UIScrollAxis.I)
        {
            UpdateProbAndColor();
            return;
        }

        bool anti = (playerUI[player].control.axis == UIScrollAxis.A);
        if (playerUI[player].pauli.axis == UIScrollAxis.X)
        {
            if (anti)
            {
            }
            else
            {
            }
        }
        else if (playerUI[player].pauli.axis == UIScrollAxis.Y)
        {
            if (anti)
            {
            }
            else
            {
            }
        }
        else if (playerUI[player].pauli.axis == UIScrollAxis.Z)
        {
            if (anti)
            {
            }
            else
            {
            }
        }

        UpdateProbAndColor();
    }
}
