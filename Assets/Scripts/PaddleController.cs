using UnityEngine;
using System.Collections.Generic;
using System;
using OpenRelativity.Objects;

namespace OpenRelativity
{
    public class PaddleController : RelativisticBehavior
    {
        public float controllerAcceleration = 12.0f;
        public float positionRange = 20.0f;

        protected RelativisticObject myRelativisticObject;

        public void Start()
        {
            //same for RigidBody
            myRelativisticObject = GetComponent<RelativisticObject>();

            //Lock and hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
        //Again, use LateUpdate to solve some collision issues.
        public void FixedUpdate()
        {
            if (state.isMovementFrozen)
            {
                return;
            }

            float temp;
            // Movement due to forward/back input
            Vector3 totalAccel = new Vector3(0, (temp = Input.GetAxis("Vertical")) * controllerAcceleration, 0);
            if (temp != 0)
            {
                state.keyHit = true;
            }

            //3-acceleration acts as classically on the rapidity, rather than velocity.
            Vector3 totalVel = myRelativisticObject.peculiarVelocity.AddVelocity((totalAccel * state.FixedDeltaTimeWorld).RapidityToVelocity());
            float tvMag = totalVel.magnitude;

            if (tvMag >= state.maxPlayerSpeed - .01f)
            {
                float gamma = totalVel.Gamma();
                Vector3 diff = totalVel.normalized * (state.maxPlayerSpeed - .01f) - totalVel;
                totalVel += diff;
                totalAccel += diff * gamma;
            }
            else if (float.IsInfinity(tvMag) || float.IsNaN(tvMag))
            {
                totalVel = state.PlayerVelocityVector;
                totalAccel = Vector3.zero;
            }

            Vector3 p = myRelativisticObject.piw + totalVel * state.FixedDeltaTimeWorld;
            if ((totalVel.y > 0) && (p.y > 0) && (p.y >= positionRange))
            {
                totalVel.y = 0;
                totalAccel.y = 0;
                p.y = positionRange - 0.01f;
            }
            else if ((totalVel.y < 0) && (p.y < 0) && (-p.y >= positionRange))
            {
                totalVel.y = 0;
                totalAccel.y = 0;
                p.y = -(positionRange - 0.01f);
            }

            myRelativisticObject.UpdateMotion(totalVel, totalAccel);
            myRelativisticObject.piw = p;
        }
    }
}