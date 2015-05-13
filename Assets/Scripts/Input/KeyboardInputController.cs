//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.18444
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

public class KeyboardIOController:OnlyExpirationInputController
{
	private bool isSpaceDown=true;

	private float strongBreathValue;

	public KeyboardIOController (ParameterManager parameterManager, DrainageAutogene exercice):base(parameterManager, exercice)
	{
		this.strongBreathValue = parameterManager.StrongBreathValue;
	}

	public override void Update(){
		this.isSpaceDown=Input.GetKey (KeyCode.Space);
	}
	
	protected override bool isBlowing(){
		return this.isSpaceDown;
	}

	public override float GetExpirationStrength(){

		if (this.isSpaceDown) {
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
				return strongBreathValue;
			}
			else{
				return 1.0f;
			}
		} else {
			return 0.0f;
		}
	}

	public override bool IsMoving(){
		return Input.GetKey (KeyCode.RightArrow);
	}
}

