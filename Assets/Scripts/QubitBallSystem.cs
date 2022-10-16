using System.Collections;
using System.Collections.Generic;
using Qrack;
using UnityEngine;

public class QubitBallSystem : MonoBehaviour
{
    public uint ballsInPlay { get; private set; }
    protected QuantumSystem quantumSystem;
    protected List<QubitBall> qubitBallMap;

    // Start is called before the first frame update
    void Awake()
    {
        ballsInPlay = 0;
        quantumSystem = GetComponent<QuantumSystem>();
        qubitBallMap = new List<QubitBall>();
    }

    // Update is called once per frame
    public uint Allocate(QubitBall qubitBall) {
        ballsInPlay++;

        if (qubitBallMap.Count == 0)
        {
            qubitBallMap.Add(qubitBall);
            return 0;
        }

        int qid;
        for (qid = 0; qid < qubitBallMap.Count; qid++)
        {
            if (qubitBallMap[qid] == qubitBall)
            {
                return (uint)qid;
            }
        }

        quantumSystem.AllocateQubit((uint)qid);
        qubitBallMap.Add(qubitBall);

        return (uint)qid;
    }

    public void Release(uint qid) {
        quantumSystem.M(qid);
        ballsInPlay--;
    }
}
