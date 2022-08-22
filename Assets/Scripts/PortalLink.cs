using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PortalLink : MonoBehaviour
{

	List<string> portalListString = new();

	private Portal[] tempArray;

	List<Portal> portalList = new();

	// Start is called before the first frame update
	void Start()
	{
		//finds all portals
		tempArray = GameObject.FindObjectsOfType<Portal>();

		//turns portal list into a list of strings
		for (int i = 0; i < tempArray.Count(); i++)
		{
			portalListString.Add(tempArray[i].ToString());
		}
		
		portalListString.Sort(); //sorts string name list

		//creates list of gameobject portals thats sorted
		for (int i = 0; i < portalListString.Count; i++)
		{
			for (int j = 0; j < tempArray.Count(); j++)
			{			
				if (portalListString[i] == tempArray[j].gameObject.name + " (Portal)")
				{
					portalList.Add(tempArray[j]);				
				}
			}
		}




	}


	// Update is called once per frame
	void Update()
	{

	}
}
