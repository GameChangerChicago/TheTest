using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{


	private string _time, _date, _meridiem;
	private Text _phoneTime, _phoneDate, _phoneMeridiem, _timeStamp;

	// Use this for initialization
	void Start ()
	{
	
		_phoneTime = GameObject.Find ("SystemTime").GetComponent<Text> ();
		_phoneDate = GameObject.Find ("SystemDate").GetComponent<Text> ();
		_phoneMeridiem = GameObject.Find ("Meridiem").GetComponent<Text> ();
		_time = System.DateTime.Now.ToString ("h:mm");
		_date = System.DateTime.Now.ToString ("ddd, MMMM, dd");
		_meridiem = System.DateTime.Now.ToString ("tt");

	}
	
	// Update is called once per frame
	void Update ()
	{
	
		UpdatePhoneClock ();

	}

	void UpdatePhoneClock ()
	{

		string _currentTime = System.DateTime.Now.ToString ("h:mm");
		string _currentDate = System.DateTime.Now.ToString ("ddd, MMMM, dd");
		string _currentMeridiem = System.DateTime.Now.ToString ("tt");

		if (_phoneDate.ToString () != _currentDate)
			_phoneDate.text = _currentDate;

		if (_phoneMeridiem.ToString () != _currentMeridiem)
			_phoneMeridiem.text = _currentMeridiem;

		if (_phoneTime.ToString () != _currentTime)
			_phoneTime.text = _currentTime;
	}
}
