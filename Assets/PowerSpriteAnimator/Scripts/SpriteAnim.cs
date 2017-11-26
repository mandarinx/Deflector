﻿//-----------------------------------------
//          PowerSprite Animator
//  Copyright © 2017 Powerhoof Pty Ltd
//			  powerhoof.com
//----------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace PowerTools
{

/// Component allowing animations to be played without adding them to a unity animation controller first. 
// A shared animation controller is used, it has a single state which is overridden whenever an animation is played.
[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class SpriteAnim : SpriteAnimEventHandler 
{	
	#region Definitions

	static readonly string STATE_NAME = "a";
	static readonly string CONTROLLER_PATH = "SpriteAnimController";

	#endregion
	#region Vars: Editor

	[SerializeField] AnimationClip m_defaultAnim = null;

	#endregion
	#region Vars: Private

	static RuntimeAnimatorController m_sharedAnimatorController = null;

	Animator m_animator = null;
	AnimatorOverrideController m_controller = null;
	SpriteAnimNodes m_nodes = null;

	#if UNITY_5_6_OR_NEWER
		List< KeyValuePair<AnimationClip, AnimationClip> > m_clipPairList = new List< KeyValuePair<AnimationClip, AnimationClip> >(1);
	#else
	AnimationClipPair[] m_clipPairArray = null;
	#endif

	AnimationClip m_currAnim = null;
	float m_speed = 1;

	#endregion
	#region Funcs: Properties


	/// True if an animation is currently playing (even if paused)
	public bool Playing { get { return IsPlaying(); } }

	/// Property for pausing or resuming the currently playing animation
	public bool Paused
	{ 
		get { return IsPaused(); } 
		set 
		{
			if ( value == true ) Pause();
			else Resume();
		}
	}

	/// Property for setting the playback speed
	public float Speed { get { return m_speed; } set { SetSpeed(value); } }

	/// Property for setting/getting the current playback time of the animation
	public float Time { get { return GetTime(); } set { SetTime( value); } }

	/// Property to get or set the the normalized time (between 0.0 to 1.0 from start to end of anim) of the currently playing clip
	public float NormalizedTime { get { return GetNormalisedTime(); } set { SetNormalizedTime( value); } }

	/// The currently playing animation clip
	public AnimationClip Clip { get { return m_currAnim; } }

	/// The name of the currently playing animation clip
	public string ClipName { get { return m_currAnim != null ? m_currAnim.name : string.Empty; } }

	#endregion
	#region Funcs: Public 

	/// Plays the specified clip
	public void Play( AnimationClip anim, float speed = 1 ) 
	{
		if ( anim == null )
			return;

		if ( m_animator.enabled == false )
			m_animator.enabled = true;

		// Reset animation nodes so any curves are 
		if ( m_nodes != null )
		    m_nodes.Reset();

		#if UNITY_5_6_OR_NEWER			
			m_clipPairList[0] = new KeyValuePair<AnimationClip, AnimationClip>(m_clipPairList[0].Key, anim);
			m_controller.ApplyOverrides(m_clipPairList);		
		#else
			m_clipPairArray[0].overrideClip = anim;
			m_controller.clips = m_clipPairArray;
		#endif
		m_animator.Update(0.0f); // Update so that new clip state is reset before hitting play
		m_animator.Play(STATE_NAME,0,0);
		m_speed = Mathf.Max(0,speed);
		m_animator.speed = m_speed;
		m_currAnim = anim;
		m_animator.Update(0.0f); // Update so that normalized time is updated immediately
	}		 	 

	/// Stops the clip by disabling the animator
	public void Stop()
	{		
		m_animator.enabled = false;
	}

	/// Pauses the animation. Call Resume to start again
	public void Pause()
	{
		m_animator.speed = 0;
	}

	/// Resumes animation playback at previous speed
	public void Resume()
	{
		m_animator.speed = m_speed;
	}

	/// Returns the currently playing clip
	public AnimationClip GetCurrentAnimation() { return m_currAnim; }

	///  Returns true if the passed clip is playing. If no clip is passed, returns true if ANY clip is playing
	public bool IsPlaying(AnimationClip clip = null) 
	{		
		if ( clip == null || m_currAnim == clip )
			return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
		return false;
	} 

	/// Returns true if a clip with the specified name is playing
	public bool IsPlaying(string animName) 
	{ 
		if ( m_currAnim == null )
			return false;
		if ( m_currAnim.name == animName )
			return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
		return false;
	} 

	public bool IsPaused()
	{
		if ( m_currAnim == null )
			return false;
		return m_animator.speed == 0;		
	}

	/// Sets the current animation playback speed
	public void SetSpeed(float speed)
	{
		m_speed = Mathf.Max(0,speed);
		m_animator.speed = m_speed;
	}

	/// Returns the current animation playback speed
	public float GetSpeed() { return m_speed; }

	/// Returns the time of the currently playing clip (or zero if no clip is playing)
	public float GetTime()
	{ 
		if ( m_currAnim != null )
			return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime * m_currAnim.length;
		return 0;
	}

	/// Property for setting/getting the current playback time of the animation
	public void SetTime( float time )
	{
		if ( m_currAnim == null || m_currAnim.length <=  0 )
			return;
		SetNormalizedTime(time / m_currAnim.length);
	}


	/// Returns the normalized time (between 0.0 and 1.0) of the currently playing clip (or zero if no clip is playing)
	public float GetNormalisedTime()
	{ 
		if ( m_currAnim != null )
			return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		return 0;
	}

	/// Property to get or set the the normalized time (between 0.0 to 1.0 from start to end of anim) of the currently playing clip
	public void SetNormalizedTime( float ratio )
	{
		if ( m_currAnim == null )
			return;
		m_animator.Play(STATE_NAME,0,  m_currAnim.isLooping ? Mathf.Repeat(ratio,1) : Mathf.Clamp01(ratio) );
	}


	#endregion
	#region Funcs: Init

	void Awake()
	{
		m_controller = new AnimatorOverrideController();

		if ( m_sharedAnimatorController == null )
		{
			// Lazy load the shared animator controller
			m_sharedAnimatorController = Resources.Load<RuntimeAnimatorController>(CONTROLLER_PATH);
		}

		m_controller.runtimeAnimatorController = m_sharedAnimatorController;
		m_animator = GetComponent<Animator>();
		m_animator.runtimeAnimatorController = m_controller;

		#if UNITY_5_6_OR_NEWER
			m_controller.GetOverrides(m_clipPairList);			
		#else
			m_clipPairArray = m_controller.clips;
		#endif

		Play(m_defaultAnim);

		m_nodes = GetComponent<SpriteAnimNodes>();
	}

	// Called when component is first added. Used to add the sprite renderer
	void Reset()
	{		
		// NB: Doing this here rather than using the RequireComponent Attribute means we can add a UI.Image instead if it's a UI Object
		if ( GetComponent<RectTransform>() == null )
		{
			// It's a regular sprite, add the sprite renderer component if it doesn't already exist
			if ( GetComponent<Sprite>() == null )
			{
				gameObject.AddComponent<SpriteRenderer>();
			}
		}
		else 
		{
			// It's a UI Image, so add the Image component if it doesn't already exist
			if ( GetComponent<UnityEngine.UI.Image>() == null )
			{
				gameObject.AddComponent<UnityEngine.UI.Image>();
			}
		}

	}

	#endregion

}

}