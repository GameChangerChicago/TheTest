using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{

	public GameObject HomeScreen, MessageScreen, LornaDialogueScreen;
	private List<GameObject> _menuList;


	// Use this for initialization
	void Start ()
	{
		_menuList = new List<GameObject> ();
		_menuList.Add (HomeScreen);
		_menuList.Add (MessageScreen);
		_menuList.Add (LornaDialogueScreen);



		foreach (GameObject g in _menuList) {
			if (g == HomeScreen)
				g.SetActive (true);
			else
				g.SetActive (false);
		}
			

	}


	public void ToMessageScreen ()
	{
		foreach (GameObject g in _menuList) {
			if (g == MessageScreen)
				g.SetActive (true);
			else
				g.SetActive (false);
		}

	}

	public void ToHomeScreen ()
	{
		foreach (GameObject g in _menuList) {
			if (g == HomeScreen)
				g.SetActive (true);
			else
				g.SetActive (false);

		}

	}

	public void ToDialogueScreen ()
	{
		foreach (GameObject g in _menuList) {
			if (g == LornaDialogueScreen)
				g.SetActive (true);
			else
				g.SetActive (false);

		}

	}

	//	public void ToScreen (string a)
	//	{
	//		foreach (GameObject g in _menuList)
	//			if (g.ToString () == a)
	//				g.SetActive (true);
	//			else
	//				g.SetActive (false);
	//	}
	//
	//	public void ShowAllScreens ()
	//	{
	//		foreach (GameObject g in _menuList)
	//			g.SetActive (true);
	//	}
}
