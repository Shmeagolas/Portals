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

	public int mostDoors = 4;

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


		List<string> treeList = new();//list of portals 1-2 in rooms with 2 or more portals
		List<string> singleRoom = new();//list of portals 1 in rooms with 1 portals
		List<string> nonTreeList = new();//list of portals 3 and greater
		//Sorts portals into 3 lists 
		for(int i = 0; i < portalListString.Count; i++)
		{
			if(portalListString[i].Substring(2, 1) == "2")
			{
				treeList.Add(portalListString[i]);
			}
			else if (portalListString[i].Substring(2, 1) == "1")
			{
				if(i >= portalListString.Count - 1 || portalListString[i+1].Substring(2,1) == "1")
				{
					singleRoom.Add(portalListString[i]);
				}
				else
				{
					treeList.Add(portalListString[i]);
				}
			}
			else
			{
				nonTreeList.Add(portalListString[i]);
			}
		}

		//randomizes order of rooms in the treeS
		for(int i = 0; i < treeList.Count; i += 2 )
		{
			string temp1 = treeList[i];
			string temp2 = treeList[i + 1];
			int rand = Random.Range(i / 2, (treeList.Count - 2) /2) * 2;
			treeList[i] = treeList[rand];
			treeList[i + 1] = treeList[rand + 1];
			treeList[rand] = temp1;
			treeList[rand + 1] = temp2;
		}

		//remove first and last portals in the tree as they are unnessary
		nonTreeList.Add(treeList[0]);
		nonTreeList.Add(treeList[treeList.Count - 1]);
		treeList.RemoveAt(treeList.Count - 1);
		treeList.RemoveAt(0);

		//create pairs array (two collumns)
		int len = portalListString.Count / 2;
		pairs = new string[2, len];

		//adds treed pairs to pairsList
		int c = 0;
		for(int i = 0; i < treeList.Count; i+=2)
		{		
			pairs[0, c] = treeList[i];
			pairs[1, c] = treeList[i + 1];
			c++;
		}

		//adds single rooms to one collumn to avoid two singles being connected
		for(int i = 0; i < singleRoom.Count; i++)
		{
			pairs[0, i + (treeList.Count / 2)] = singleRoom[i];
		}

		int randPort;

		do //loops if last two portals end up being from same room
		{
			//finds matches for single room portals
			for(int i = 0; i < singleRoom.Count; i++)
			{
				do
				{ 
					randPort = Random.Range(0, nonTreeList.Count - 1);

				} while (pairs[0, i].Substring(1, 1) == nonTreeList[randPort].Substring(1, 1));
				pairs[1, i + (treeList.Count / 2)] = nonTreeList[randPort];
				nonTreeList.RemoveAt(randPort);
			}

			//mixes and matches remaining nonTree allocated Portals
			List<string> tempNonTree = new List<string>(nonTreeList);

			for(int i = 0; i < nonTreeList.Count / 2; i++)
			{
				int iterations = 0;
				do
				{
					iterations++;//for niche bug if last 4 doors from same room
					randPort = Random.Range(1, tempNonTree.Count - 1);//random index
				} while (tempNonTree.Count > 2 && tempNonTree[0].Substring(1, 1) == tempNonTree[randPort].Substring(1, 1) && iterations <= mostDoors);
				
				if (iterations > mostDoors) break;

				pairs[0, i + (treeList.Count / 2) + singleRoom.Count] = tempNonTree[0];
				pairs[1, i + (treeList.Count / 2) + singleRoom.Count] = tempNonTree[randPort];

				tempNonTree.RemoveAt(randPort);
				tempNonTree.RemoveAt(0);
			}

		} while (pairs[0, len - 1].Substring(1, 1) == pairs[1, len - 1].Substring(1, 1));

		//creates list of gameobject portals thats sorted
		/*for (int i = 0; i < portalListString.Count; i++)
		{
			for (int j = 0; j < tempArray.Count(); j++)
			{			
				if (portalListString[i] == tempArray[j].gameObject.name + " (Portal)")
				{
					portalList.Add(tempArray[j]);				
				}
			}
		}*/
		int ten = pairs.GetLength(1);
		ten += 10;
		for (int i = 0; i < pairs.GetLength(1); i++)
		{

		}




	}


	// Update is called once per frame
	void Update()
	{

	}
}
