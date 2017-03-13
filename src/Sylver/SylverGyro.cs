using System;
using UnityEngine;

namespace Sylver
{
	public class SylverGyro : Module
	{
		public SylverGyro()
		{
			this.stability = 0.015f;
			this.speed = 50f;
			this.fudgeVal = 100f;
			this.m_MaxReturnTorqueStuff = 125f;
		}

		public void OnAttach()
		{
		}

		public void OnDetach()
		{
		}

		public void FixedUpdate()
		{
			if (base.block.tank)
			{
				Rigidbody rbody = base.block.tank.rbody;
				Vector3 vector = Quaternion.AngleAxis(rbody.angularVelocity.magnitude * 57.29578f * (this.stability / this.speed), rbody.angularVelocity) * base.block.tank.rootBlockTrans.up;
				Vector3 a = Vector3.Cross(vector, Vector3.up);
				Vector3 vector2 = base.block.centreOfMassWorld - rbody.worldCenterOfMass;
				Vector3 normalized = a.normalized;
				float d = Vector3.Dot(vector2, normalized);
				Vector3 b = vector2 - normalized * d;
				Vector3 inertiaTensor = rbody.inertiaTensor;
				Vector3 vector3 = normalized;
				vector3.x *= inertiaTensor.x;
				vector3.y *= inertiaTensor.y;
				vector3.z *= inertiaTensor.z;
				bool arg_FE_0 = this.applyTorque;
				rbody.AddTorque(a * this.speed * this.speed);
				Debug.DrawLine(rbody.worldCenterOfMass, rbody.worldCenterOfMass + rbody.transform.up * 10f, Color.green);
				Debug.DrawLine(rbody.worldCenterOfMass, rbody.worldCenterOfMass + vector * 10f, Color.cyan);
				Debug.DrawLine(rbody.worldCenterOfMass, rbody.worldCenterOfMass + a * 10f, Color.red);
				Debug.DrawLine(base.block.centreOfMassWorld, base.block.centreOfMassWorld + a * 10f, Color.red);
				Debug.DrawLine(base.block.centreOfMassWorld, base.block.centreOfMassWorld - vector2, Color.white);
				Debug.DrawLine(base.block.centreOfMassWorld, base.block.centreOfMassWorld - b, Color.yellow);
			}
		}

		public float stability;

		public float speed;

		public bool applyTorque;

		public float fudgeVal;

		public float m_MaxReturnTorqueStuff;
	}
}