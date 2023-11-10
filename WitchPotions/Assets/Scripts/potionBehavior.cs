using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionBehavior : MonoBehaviour
{
    /*
     * OVERALL ARCHITECTURE:
     * Start() creates a radial grid of nodes, which are each points around the emotional spectrum.
     * MoveToward((Vector3, float)) moves the parent gameobject of PotionBehavior so many nodes across the 
     * grid towards a target node. AddIngredient(Ingredients_SO) interfaces with MoveToward by taking an object that 
     * provides the necessary args to call MoveTowards(), and calculate the total cost and poison value of the potion.
     * Each call of AddIngredient also calls SpecialNodeUpdate to see if the end point of your ingredient has hit
     * any special nodes such as voids (instant fail of potion), bipolar(teleports gameobject), or charger (extra value of potion).
     */
    Vector3 center;
    List<Vector3> nodes = new List<Vector3>();
    List<Vector3> previewNodes = new List<Vector3>();

    // adds ingredient added and starting point before ingredient was added at beginning of AddIngredient() 
    Stack<int> nodeStartHistory = new Stack<int>();
    Stack<Ingredients_SO> ingredientHistory = new Stack<Ingredients_SO>();
    //Stack<(int nodeIndex, Level_SO.NodeTypes type)> chargersHistory = new Stack<(int nodeIndex, Level_SO.NodeTypes type)>;
    //one grid unit
    float unit;

    int currentNodePosition = 0;
    // array of special nodes, given the node index and node type
    List<(int nodeIndex, Level_SO.NodeTypes type)> specials = new List<(int nodeIndex, Level_SO.NodeTypes type)>();
    //Special Node Prefabs
    [SerializeField] GameObject endpointPrefab;
    [SerializeField] GameObject chargerPrefab;
    [SerializeField] GameObject voidPrefab;
    [SerializeField] GameObject bipolarPrefab;
    GameObject[] instantiatedPrefabs = new GameObject[0];
    GameObject endpoint;

    //Debug
    public GameObject visablePoint;
    public GameObject backGround;

    //positions of each emotional extreme
  
    //relevant potion statistics
    int poison = 0;
    int cost = 0;
    int chargesCount = 0;
    int chargersHit = 0;
    [SerializeField]
    int endpointIndex;
    int currentMoney;

    //UI elements:
    public TextMeshProUGUI poisonText;
    public TextMeshProUGUI costText;


    //Grid Values:
    int rings = 10;
    int slices = 24;
    float theta = 15.0f;

    //Star system
    int star2Posion = 25;
    int star1Posion = 50;
    public GameObject[] stars;
    float moneyMadeOnFInish = 0;


    Dictionary<string, int> emotionValues = new Dictionary<string, int>()
    {
        {"Center", 0 },
        {"Terror", 10 },
        {"Admiration", 40 },
        {"Joy", 70},
        {"Vigilance",100},
        {"Rage", 130 },
        {"Loathing",160 },
        {"Grief", 190},
        {"Amazement", 220 },

    };

    [SerializeField] GameObject UIPanel;
    [SerializeField] TextMeshProUGUI UIText;
    private void Start()
    {
        center = gameObject.transform.localPosition;

        

        //potion position init, resolve how many rings will be in use and what the base ring units will be
      
        Vector3[] nodetemplate = new Vector3[rings];
        unit = 200f / rings;
        for (int i = 1; i <= rings; i++)
        {
            nodetemplate[i-1] = center + new Vector3(i * unit, 0, 0);
        }

        //indices for emotions and endpoints are 0 = center, 1-10 is fear,
        //11-20 is between fear and admiration, 21-30 is admiration, etc. 

        nodes.Add(center);
        //nodes.AddRange(nodetemplate);
        //populate node graph
        for (int i = 0; i < slices; i++)
        {
            for (int j = 0; j < rings; j++)
            {
                Vector3 newNode;
                newNode = Quaternion.Euler(0, 0, (i * theta)) * nodetemplate[j];
                GameObject point =  Instantiate(visablePoint, backGround.transform);
                point.transform.localPosition = newNode;
                nodes.Add(newNode);
            }
        }
        previewNodes = nodes;

        //load in the current level
        LoadLevelObject(GameManager.Instance.currentLevel, GameManager.Instance.currentCharacterDiscoveredInfo);

    }

    //Loading levels to add 
   public void LoadLevelObject(Level_SO level, bool[] discovered)
    {
        Transform parent = gameObject.transform.parent;
        int length = level.Special_Nodes_List.emotionIndex.Length;
        if (instantiatedPrefabs != null || instantiatedPrefabs.Length > 0)
        {
            for (int i = 0; i < instantiatedPrefabs.Length; i++)
            {
                Destroy(instantiatedPrefabs[i]);
            }
            specials.Clear();
        }
        instantiatedPrefabs = new GameObject[length];
        int currentIndex = 0;
        for (int i = 0; i < length; i++)
        {
            if (discovered[i])
            {
                switch (level.Special_Nodes_List.type[i])
                {
                   // case Level_SO.NodeTypes.voidNode:
                     //   instantiatedPrefabs[i] = Instantiate(voidPrefab,parent);
                       // instantiatedPrefabs[i].transform.localPosition = nodes[level.Special_Nodes_List.emotionIndex[i]];
                        
                       // break;
                    case Level_SO.NodeTypes.charger:
                        specials.Add((level.Special_Nodes_List.emotionIndex[i], level.Special_Nodes_List.type[i]));
                        instantiatedPrefabs[i] = Instantiate(chargerPrefab, parent);
                        instantiatedPrefabs[i].transform.localPosition = nodes[level.Special_Nodes_List.emotionIndex[i]];
                        currentIndex = i;
                        chargesCount++;
                        break;
                    default:
                        if (currentIndex == i - 1) currentIndex = i;
                        Level_SO.NodeTypes typing = level.Special_Nodes_List.type[currentIndex];
                        while (currentIndex < level.Special_Nodes_List.type.Length && typing == level.Special_Nodes_List.type[currentIndex])
                        {
                            if (typing == Level_SO.NodeTypes.voidNode)
                            {
                                specials.Add((level.Special_Nodes_List.emotionIndex[currentIndex], level.Special_Nodes_List.type[currentIndex]));
                                instantiatedPrefabs[i] = Instantiate(voidPrefab, parent);
                                instantiatedPrefabs[i].transform.localPosition = nodes[level.Special_Nodes_List.emotionIndex[currentIndex]];
                            }
                            else
                            {
                                specials.Add((level.Special_Nodes_List.emotionIndex[currentIndex], level.Special_Nodes_List.type[currentIndex]));
                                instantiatedPrefabs[i] = Instantiate(bipolarPrefab, parent);
                                instantiatedPrefabs[i].transform.localPosition = nodes[level.Special_Nodes_List.emotionIndex[currentIndex]];
                            }
                            currentIndex++;
                            i++;
                        }
                        break;
                }
                
            }
        }
        endpointIndex = level.Endpoint_Index;
        endpoint = Instantiate(endpointPrefab, parent);
        endpoint.transform.localPosition = nodes[level.Endpoint_Index];
        poison = 0;
        transform.localPosition = center;
        cost = 0;
        moneyMadeOnFInish = level.coin_Reward;
        Debug.Log(discovered.Length + " discovered indices");

    }

    /// <summary>
    /// Load level used for debug, open all chargers autmomethicly 
    /// </summary>
    /// <param name="level"></param>
    public void LoadLevelObject(Level_SO level)
    {
        
        Transform parent = gameObject.transform.parent;
        int length = level.Special_Nodes_List.emotionIndex.Length;
        if (instantiatedPrefabs != null || instantiatedPrefabs.Length > 0)
        {
            for (int i = 0; i < instantiatedPrefabs.Length; i++)
            {
                Destroy(instantiatedPrefabs[i]);
            }
            specials.Clear();
        }
        instantiatedPrefabs = new GameObject[length];
        int currentIndex = 0;
        moneyMadeOnFInish = level.coin_Reward;
        for (int i = 0; i < length; i++)
        {
            
                switch (level.Special_Nodes_List.type[i])
                {
                    // case Level_SO.NodeTypes.voidNode:
                    //   instantiatedPrefabs[i] = Instantiate(voidPrefab,parent);
                    // instantiatedPrefabs[i].transform.localPosition = nodes[level.Special_Nodes_List.emotionIndex[i]];

                    // break;
                    case Level_SO.NodeTypes.charger:
                        specials.Add((level.Special_Nodes_List.emotionIndex[i], level.Special_Nodes_List.type[i]));
                        instantiatedPrefabs[i] = Instantiate(chargerPrefab, parent);
                        instantiatedPrefabs[i].transform.localPosition = nodes[level.Special_Nodes_List.emotionIndex[i]];
                        currentIndex = i;
                        chargesCount++;
                    break;
                    default:
                        if (currentIndex == i - 1) currentIndex = i;
                        Level_SO.NodeTypes typing = level.Special_Nodes_List.type[currentIndex];
                        while (currentIndex < level.emotionIndices.Length && typing == level.Special_Nodes_List.type[currentIndex])
                        {
                            if (typing == Level_SO.NodeTypes.voidNode)
                            {
                                specials.Add((level.Special_Nodes_List.emotionIndex[currentIndex], level.Special_Nodes_List.type[currentIndex]));
                                instantiatedPrefabs[i] = Instantiate(voidPrefab, parent);
                                instantiatedPrefabs[i].transform.localPosition = nodes[level.Special_Nodes_List.emotionIndex[currentIndex]];
                            }
                            else
                            {
                                specials.Add((level.Special_Nodes_List.emotionIndex[currentIndex], level.Special_Nodes_List.type[currentIndex]));
                                instantiatedPrefabs[i] = Instantiate(bipolarPrefab, parent);
                                instantiatedPrefabs[i].transform.localPosition = nodes[level.Special_Nodes_List.emotionIndex[currentIndex]];
                            }
                            currentIndex++;
                            i++;
                        }
                   
                        break;              

            }
        }
        endpointIndex = level.Endpoint_Index;
        endpoint = Instantiate(endpointPrefab, parent);
        endpoint.transform.localPosition = nodes[level.Endpoint_Index];
        poison = 0;
        transform.localPosition = center;
        cost = 0;
    }



    //ingredients 2.0
    //TODO takes IngredientObject
    /*
     * IngredientObject Properties:
     * (String, int)[] emoProp
     * int price
     * int poison
     * String name
     */
    public void AddIngredient(Ingredients_SO ingredient) 
    {
        nodeStartHistory.Push(currentNodePosition);
        ingredientHistory.Push(ingredient);

        //Making sure move towards center allways moves towards center without side movements
        if (ingredient.ingredients_Emotion[0] == "Center")
        {
            //If value of movment is bigger then the current emotional value, just go to center
            if (currentNodePosition % 10 > ingredient.ingredients_Value[0] ||  currentNodePosition%10 ==0 && currentNodePosition!=0)
            {
                currentNodePosition = currentNodePosition - ingredient.ingredients_Value[0];
                transform.localPosition = nodes[currentNodePosition];
                
            }
            //If it's not bigger just move toward centers on the same axis
            else
            {
                currentNodePosition = 0;
                transform.localPosition = nodes[0];
            }
        }
        //If ing is not center, do calculate movement
        else
        {

            //creates a path, sets the current node position to the end point of the path, and then sets localposition to that end point
            int[] path = Pathing(emotionValues[ingredient.Ingredients_Vector.emotion[0]], currentNodePosition, ingredient.Ingredients_Vector.value[0]);
            currentNodePosition = path[path.Length - 1];
            transform.localPosition = nodes[currentNodePosition];

            //repeat if two different modes to ingredient
            if (ingredient.Ingredients_Vector.emotion.Length > 1)
            {
                path = Pathing(emotionValues[ingredient.Ingredients_Vector.emotion[1]], currentNodePosition, ingredient.Ingredients_Vector.value[1]);
                currentNodePosition = path[path.Length - 1];
                transform.localPosition = nodes[path[path.Length - 1]];
            }
        }
        cost += ingredient.ingredients_Price;
        poison += ingredient.ingredients_Poison;
        poisonText.text = poison.ToString();
        costText.text = cost.ToString();

        SpecialNodeUpdate();
        previewNodes = nodes;
      

    }

    //Undo move button! (TODO, undo charger use)
    public void Undo()
    {
        if (nodeStartHistory.Count >= 2)
        {
            nodeStartHistory.Pop();
            transform.localPosition = nodes[nodeStartHistory.Peek()];
            Ingredients_SO tempIng = ingredientHistory.Pop();
            poison -= tempIng.ingredients_Poison;
            cost -= tempIng.ingredients_Price;

        }
        else
        {
            if (nodeStartHistory.Count == 1)
            {
                Ingredients_SO tempIng = ingredientHistory.Peek();
                poison -= tempIng.ingredients_Poison;
                cost -= tempIng.ingredients_Price;
                nodeStartHistory.Pop();
                ingredientHistory.Pop();
            }
            
            transform.localPosition = nodes[0];
        }
        poisonText.text = poison.ToString();
        costText.text = cost.ToString();

    }

    //Reset all move button! 
    public void Reset()
    {
        nodeStartHistory.Clear();
        ingredientHistory.Clear();
        transform.localPosition = nodes[0];
        poison = 0;
        cost = 0;
        currentNodePosition = 0;
        LoadLevelObject(GameManager.Instance.currentLevel, GameManager.Instance.currentCharacterDiscoveredInfo);
        poisonText.text = poison.ToString();
        costText.text = cost.ToString();
        poisonText.text = poison.ToString();
        costText.text = cost.ToString();
    }

    // moves toward a given endpoint by a certain distance
    void MoveToward((Vector3, float) inputs) {

        (Vector3 node, float distance) = inputs;
        Vector3 v;
        Vector3 vSpace;
        Vector3 finalNode;
        float distBest;
        if (node == transform.localPosition) return;
        float distleft = Vector3.Distance(node, transform.localPosition);
        if (distleft >= distance)
        {
            v = (node - transform.localPosition).normalized * distance;
            vSpace = v + transform.localPosition;
            //set final as destination node with distance 
            finalNode = node;
            distBest = Vector3.Distance(node, vSpace);
        }
        else
        {
            transform.localPosition = node;
            return;
        }

        //calcualtes the closest node (that is NOT the current node) and locks the point's position to it
        foreach (Vector3 point in nodes)
        {
            float temp = Vector3.Distance(vSpace, point);

            if (distBest > temp && point != transform.localPosition) { distBest = temp; finalNode = point; }
        }
        transform.localPosition = finalNode;
    }

    Vector3 CreatePathEndpoint(Vector3 node, float distance)
    {
        Vector3 v = (node - transform.localPosition).normalized * distance;
        Vector3 vSpace = v + transform.localPosition;

        return vSpace;
    }


    //called at the end of movetoward to see if we landed on any special nodes
    void SpecialNodeUpdate()
    {
        if (specials.Count == 0) return;
        for (int i = 0; i < specials.Count; i++)
        {
            if (nodes[specials[i].nodeIndex] == transform.localPosition)
            {
                switch(specials[i].type)
                {
                    case Level_SO.NodeTypes.voidNode:
                        Debug.Log("oops! you hit a void :(");
                        Reset();
                        break;
                    case Level_SO.NodeTypes.charger:
                        chargersHit++;
                        GameObject.Destroy(instantiatedPrefabs[i]);
                        break;
                    case Level_SO.NodeTypes.bipolar:
                        for (int j = 0; j < specials.Count; j++)
                        {
                            if (i != j && specials[j].type == Level_SO.NodeTypes.bipolar)
                            {
                                transform.localPosition = nodes[specials[j].nodeIndex];
                                currentNodePosition = specials[j].nodeIndex;
                            }
                        }
                        break;
                    default:
                        break;
                }
                return;
            }
        }

    }


    //returns all neighbors 
    int[] GetNodeNeigbors(int node)
    {
        int sliceCutsIndexs = (slices) * 10;
        int[] neighbors;
        //innermost ring case
        if (node % 10 == 1)
        {
            neighbors = new int[6] {0, node + 10, node + 11, node + 1, node - 9, node - 10 };
            
        }
        //outermost ring case
        else if (node % 10 == 0 && node != 0)
        {
            neighbors = new int[5] { node + 10, node +9, node -1, node -11, node - 10 };

        }
        //center case
        else if (node == 0)
        { 
            neighbors = new int[slices];
            for (int i = 0; i < slices; i++)
            {
                neighbors[i] = 1 + i * 10;
            }
        }
        //middle rings case
        else { neighbors = new int[8] {node + 9, node + 10, node + 11, node + 1, node - 9, node -10, node-11, node-1 }; }

        //looping rings around when out of index
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] <= 0 && i > 0 )
            {
                neighbors[i] = sliceCutsIndexs + neighbors[i];
            }
            else if (neighbors[i] > sliceCutsIndexs) { neighbors[i] = neighbors[i] - sliceCutsIndexs; }
        }
        return neighbors;
    }



    // End of path, Start of Path (usually current position), how many nodes into the path you traverse
    int[] Pathing(int endNode,  int startNode, int nodesToTraverse)
    {

        List<int> pathNodes = new List<int>();
        int[] neigbors = GetNodeNeigbors(startNode);

        int closestToEndpoint = 0;
        float bestDist = Vector3.Distance(nodes[endNode], nodes[startNode]);

        //checks neighbors for the new best distance
        for (int i = 0; i < neigbors.Length; i++)
        {
            if (endNode == neigbors[i] || endNode == startNode)
            {
                closestToEndpoint = endNode;
                break;
            }
            float dist = Vector3.Distance(nodes[endNode], nodes[neigbors[i]]);
            if (dist <= bestDist)
            {
                bestDist = dist;
                closestToEndpoint = neigbors[i];
            }
        }

        //adds the winner to pathNodes, recursive calls for the rest of the nodes in the pathing, path endpoint being last
        Debug.Log(closestToEndpoint);
        pathNodes.Add(closestToEndpoint);
        if (nodesToTraverse > 1 && endNode != closestToEndpoint)
        {
            pathNodes.AddRange(Pathing(endNode, closestToEndpoint, nodesToTraverse - 1));
        }
        
        return pathNodes.ToArray();
    }

    //Create a line preview 
    public UILineRenderer lineRenderer;
    public void HoverOverIngredeint(Ingredients_SO ingredient)
    {
        lineRenderer.gameObject.SetActive(true);

        


        //Set up like the movement, only using Line renderer to save the data
        //instead of moving using transform 
        List<Vector2> pointsV2 = new List<Vector2>();
        //Vector2[] pointsV2 = new Vector2[ingredient.ingredients_Value.Length + 1];
        pointsV2.Add(transform.localPosition);

        //In case of move towards center, force preview and ignore pathing
        if (ingredient.ingredients_Emotion[0] == "Center")
        {
            //If value of movment is bigger then the current emotional value, just go to center
            if (currentNodePosition % 10 > ingredient.ingredients_Value[0] || currentNodePosition % 10 == 0 && currentNodePosition != 0)
            {
                pointsV2.Add(nodes[currentNodePosition - ingredient.ingredients_Value[0]]);

            }
            //If it's not bigger just move toward centers on the same axis
            else
            {
                pointsV2.Add(nodes[0]);
            }
        }
        //If not center use notmal pathing
        else
        {
            int[] path = PreviewPathing(emotionValues[ingredient.Ingredients_Vector.emotion[0]], currentNodePosition, ingredient.Ingredients_Vector.value[0]);
            int pos = path[path.Length - 1];
            /*Vector2 newLocation = nodes[pos];
            *pointsV2[1] = newLocation;
            */
            foreach (int p in path) pointsV2.Add(nodes[p]);
            //repeat if two different modes to ingredient
            if (ingredient.Ingredients_Vector.emotion.Length > 1)
            {
                 path = PreviewPathing(emotionValues[ingredient.Ingredients_Vector.emotion[1]], pos, ingredient.Ingredients_Vector.value[1]);
                 /*pos = path[path.Length - 1];
                  *newLocation = nodes[path[path.Length - 1]];
                  *pointsV2[2] = newLocation;
                  */
                  foreach (int p in path) pointsV2.Add(nodes[p]);
            }
        }
        //make sure to not draw a line if start and end point are the same
        if (pointsV2[0] != pointsV2[pointsV2.Count - 1])
        {
            lineRenderer.Points = pointsV2.ToArray();
        }
        else
        {
            lineRenderer.gameObject.SetActive(false);
        }

    }

    // End of path, Start of Path (usually current position), how many nodes into the path you traverse
    int[] PreviewPathing(int endNode, int startNode, int nodesToTraverse)
    {

        List<int> pathNodes = new List<int>();
        int[] neigbors = GetNodeNeigbors(startNode);
        int closestToEndpoint = endNode;
        float bestDist = Vector3.Distance(previewNodes[endNode], previewNodes[startNode]);

        //checks neighbors for the new best distance
        for (int i = 0; i < neigbors.Length; i++)
        {
            if (endNode == neigbors[i] || endNode == startNode)
            {
                closestToEndpoint = endNode;
                break;
            }
            float dist = Vector3.Distance(previewNodes[endNode], previewNodes[neigbors[i]]);
            if (dist <= bestDist)
            {
                bestDist = dist;
                closestToEndpoint = neigbors[i];
            }
        }

        //adds the winner to pathNodes, recursive calls for the rest of the nodes in the pathing, path endpoint being last
        Debug.Log(closestToEndpoint);
        pathNodes.Add(closestToEndpoint);
        if (nodesToTraverse > 1 && endNode != closestToEndpoint)
        {
            pathNodes.AddRange(Pathing(endNode, closestToEndpoint, nodesToTraverse - 1));
        }
        return pathNodes.ToArray();
    }
    public void HoverEnd()
    {
        lineRenderer.gameObject.SetActive(false);
        Vector2[] pointsV2 = new Vector2[1];
        lineRenderer.Points = pointsV2;
        previewNodes = nodes;

    }

    //finish brewing, probably to be called by InventoryManager or another GameManager
    //TODO by Elad
    public void  FinishBrew() 
    {
        int starcount = 0;
        float moneyEarned = moneyMadeOnFInish;
        foreach (var item in stars)
        {
            item.SetActive(false);
        }
        if (currentNodePosition == endpointIndex)
        {
            if (UIPanel)
            {
                if(chargersHit == chargesCount && poison <star2Posion)
                {
                    starcount = 3;
                }
                else if (chargersHit == chargesCount-1 && poison < star2Posion || chargersHit == chargesCount && poison < star1Posion && poison > star2Posion)
                {
                    starcount = 2;
                    moneyEarned = moneyEarned * 0.75f;
                }
                else
                {
                    starcount = 1;
                    moneyEarned = moneyEarned * 0.5f;

                }
                UIPanel.SetActive(true);
              
                UIText.text = ("Chargers Hit:" + chargersHit + "<br>" + "Poison:" + poison + "<br>" + "Money Spent:" + cost + "<br>" + "Money Earned:" + moneyEarned);
                for (int i = 0; i < starcount; i++)
                {
                    stars[i].SetActive(true);
                }
            }
            sendData();
        }
        else { Debug.Log("nuh uh, you're not ready yet!"); }
    }
    (int chargers, int poison) sendData()
    {
        return (chargersHit, poison);
    }
}

