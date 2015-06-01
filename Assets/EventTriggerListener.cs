﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class EventTriggerListener : MonoBehaviour ,IPointerClickHandler,IPointerDownHandler,IPointerEnterHandler,
IPointerExitHandler,IPointerUpHandler,ISelectHandler,IUpdateSelectedHandler,IBeginDragHandler, IDragHandler, IEndDragHandler{
	public delegate void VoidDelegate (GameObject go);
	public VoidDelegate onClick;
	public VoidDelegate onDown;
	public VoidDelegate onEnter;
	public VoidDelegate onExit;
	public VoidDelegate onUp;
	public VoidDelegate onSelect;
	public VoidDelegate onUpdateSelect;
	public VoidDelegate onBeginDrag;



	static public EventTriggerListener Get (GameObject go)
	{
		EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
		if (listener == null) listener = go.AddComponent<EventTriggerListener>();
		return listener;
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		if(onClick != null) 	onClick(gameObject);
	}
	public void OnPointerDown (PointerEventData eventData){
		if(onDown != null) onDown(gameObject);
	}
	public void OnPointerEnter (PointerEventData eventData){
		if(onEnter != null) onEnter(gameObject);
	}
	public void OnPointerExit (PointerEventData eventData){
		if(onExit != null) onExit(gameObject);
	}
	public void OnPointerUp (PointerEventData eventData){
		if(onUp != null) onUp(gameObject);
	}
	public void OnSelect (BaseEventData eventData){
		if(onSelect != null) onSelect(gameObject);
	}
	public void OnUpdateSelected (BaseEventData eventData){
		if(onUpdateSelect != null) onUpdateSelect(gameObject);
	}
	public void OnBeginDrag(PointerEventData eventData){
		if(onBeginDrag != null) onBeginDrag(gameObject);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
	}

	public void OnDrag(PointerEventData data)
	{
	}

}
