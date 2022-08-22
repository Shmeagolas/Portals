using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PortalLink : MonoBehaviour
{

	List<string> portalListString = new();

	private Portal[] tempArray;

	List<Portal> portalList = new();

	private string[,] pairs;

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

		int len = portalListString.Count / 2;
		pairs = new string[2, len];

		//loops if any portal is linked to the same room
		do
		{
			//copies list of portals
			List<string> pairListString = new List<string>(portalListString);
			int randPort;
			for (int i = 0; i < len; i++)
			{
				
				do
				{
					randPort = Random.Range(1, pairListString.Count - 1);
				} while (pairListString.Count > 2 && pairListString[0].Substring(1, 1) == pairListString[randPort].Substring(1, 1));


				pairs[0, i] = pairListString[0];
				pairs[1, i] = pairListString[randPort];
				pairListString.Remove(pairListString[randPort]);
				pairListString.Remove(pairListString[0]);
			}

		} while(pairs[0, len - 1].Substring(1, 1) == pairs[1, len - 1].Substring(1, 1)); 


	}


	// Update is called once per frame
	void Update()
	{

	}
}
