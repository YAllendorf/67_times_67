using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cell : MonoBehaviour
{
    #region variables
    //for communication
    public GameObject gameManager;

    //variables of Cell
    public bool isAlive;
    public int thisColor;

    //Colors
    public Color[] colors;

    //neighbours and stuff
    public Cell[] neighbours;
    public Cell up;
    public Cell upRight;
    public Cell right;
    public Cell rightDown;
    public Cell down;
    public Cell downLeft;
    public Cell left;
    public Cell leftUp;

    public int numberOfNeighbours;
    public int numberOfInvaders;

    //next Tick status
    public bool nextTickAlive;
    public int nextTickColor;
    #endregion

    //SetStatus MUST be used to change variables "isAlive" and "colorIndex"
    //TODO: create properties to make those variables read-only
    public void SetStatus (bool isAlive, int colorIndex)
    {
        if (isAlive == true)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<SpriteRenderer>().color = colors[colorIndex];
            this.isAlive = isAlive;
            this.thisColor = colorIndex;
        }
        else if (isAlive == false)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            this.isAlive = isAlive;
        }
    }

    public void StartTick(GameObject manager)
    {
        gameManager = manager;
        numberOfNeighbours = 0;

        //count alive neighbours
        if(up != null)
        {
            if (up.isAlive)
            {
                numberOfNeighbours += 1;
            }
        }
        if (right != null)
        {
            if (right.isAlive)
            {
                numberOfNeighbours += 1;
            }
        }
        if (down != null)
        {
            if (down.isAlive)
            {
                numberOfNeighbours += 1;
            }
        }
        if (left != null)
        {
            if (left.isAlive)
            {
                numberOfNeighbours += 1;
            }
        }
        gameManager.GetComponent<GameManager>().CellCount("startTickOver");
    }

    /// <rules>
    /// 1. Yellow moves up
    /// 2. Turquoise moves right
    /// 3. Blue moves down
    /// 4. Pink moves left
    /// 5. If 2 Cells push to the same Cell it turns white
    /// 6. If 2 Cells push to opposite directions and touch each other they stay in a stalemate
    /// 7. If a colored Cell goes into a white Cell it gets deleted and the white Cell generates one of each other colored Cell at its corresponding side
    /// </rules>


    //checks rules and saves upcoming changes in variables
    public void MidTick()
    {
        //if we are alive
        if (this.isAlive)
        {
            //if we are white
            if (thisColor == 4)
            {
                //we stay like this
                nextTickAlive = true;
                nextTickColor = 4;
            }
            //if we are not white
            else if (thisColor != 4)
            {
                //and have no neighbours
                if (numberOfNeighbours == 0)
                {
                    //and are not white
                    if (this.thisColor != 4)
                    {
                        //we turn off
                        nextTickAlive = false;
                    }
                }
                //if we have 1 neighbour exactly
                else if (numberOfNeighbours == 1)
                {
                    //if we are yellow
                    if (this.thisColor == 0)
                    {
                        //check up for alive Cell
                        if (up.isAlive)
                        {
                            //is it blue?
                            if (up.thisColor == 2)
                            {
                                //stalemate, we stay alive
                                nextTickAlive = true;
                                nextTickColor = 0;
                            }
                            //is it white?
                            else if (up.thisColor == 4)
                            {
                                //we get eaten by white
                                nextTickAlive = false;
                            }
                        }
                        //if up is neither Blue nor White, check other neighbours
                        //check right for Pink
                        else if (right.isAlive && right.thisColor == 3)
                        {
                            nextTickAlive = true;
                            nextTickColor = 3;
                        }
                        //check down for Yellow
                        else if (down.isAlive && down.thisColor == 0)
                        {
                            nextTickAlive = true;
                            nextTickColor = 0;
                        }
                        //check left for Turquoise
                        else if (left.isAlive && left.thisColor == 1)
                        {
                            nextTickAlive = true;
                            nextTickColor = 1;
                        }
                    }
                    //if we are Turquoise
                    else if (this.thisColor == 1)
                    {
                        //check right for alive Cell
                        if (right.isAlive)
                        {
                            //is it Pink?
                            if (right.thisColor == 3)
                            {
                                //stalemate, we stay alive
                                nextTickAlive = true;
                                nextTickColor = 1;
                            }
                            //is it white?
                            else if (right.thisColor == 4)
                            {
                                //we get eaten by white
                                nextTickAlive = false;
                            }
                        }
                        //if right is neither Pink nor White, check other neighbours
                        //check up for Blue
                        else if (up.isAlive && up.thisColor == 2)
                        {
                            nextTickAlive = true;
                            nextTickColor = 2;
                        }
                        //check down for Yellow
                        else if (down.isAlive && down.thisColor == 0)
                        {
                            nextTickAlive = true;
                            nextTickColor = 0;
                        }
                        //check left for Turquoise
                        else if (left.isAlive && left.thisColor == 1)
                        {
                            nextTickAlive = true;
                            nextTickColor = 1;
                        }
                    }
                    //if we are Blue
                    else if (this.thisColor == 2)
                    {
                        //check down for alive Cell
                        if (down.isAlive)
                        {
                            //is it Yellow?
                            if (down.thisColor == 0)
                            {
                                //stalemate, we stay alive
                                nextTickAlive = true;
                                nextTickColor = 2;
                            }
                            //is it white?
                            else if (down.thisColor == 4)
                            {
                                //we get eaten by white
                                nextTickAlive = false;
                            }
                        }
                        //if down is neither Yellow nor White, check other neighbours
                        //check up for Blue
                        else if (up.isAlive && up.thisColor == 2)
                        {
                            nextTickAlive = true;
                            nextTickColor = 2;
                        }
                        //check right for Pink
                        else if (right.isAlive && right.thisColor == 3)
                        {
                            nextTickAlive = true;
                            nextTickColor = 3;
                        }
                        //check left for Turquoise
                        else if (left.isAlive && left.thisColor == 1)
                        {
                            nextTickAlive = true;
                            nextTickColor = 1;
                        }
                    }
                    //if we are Pink
                    else if (this.thisColor == 3)
                    {
                        //check left for alive Cell
                        if (left.isAlive)
                        {
                            //is it Turquoise?
                            if (left.thisColor == 1)
                            {
                                //stalemate, we stay alive
                                nextTickAlive = true;
                                nextTickColor = 3;
                            }
                            //is it white?
                            else if (left.thisColor == 4)
                            {
                                //we get eaten by white
                                nextTickAlive = false;
                            }
                        }
                        //if left is neither Turquoise nor White, check other neighbours
                        //check up for Blue
                        else if (up.isAlive && up.thisColor == 2)
                        {
                            nextTickAlive = true;
                            nextTickColor = 2;
                        }
                        //check right for Pink
                        else if (right.isAlive && right.thisColor == 3)
                        {
                            nextTickAlive = true;
                            nextTickColor = 3;
                        }
                        //check down for Yellow
                        else if (down.isAlive && down.thisColor == 0)
                        {
                            nextTickAlive = true;
                            nextTickColor = 1;
                        }
                    }
                }
                //if we have 2 or more neighbours
                else if (numberOfNeighbours >= 2)
                {
                    //check up for Blue Cell
                    if (up.isAlive && up.thisColor == 2)
                    {
                        numberOfInvaders++;
                    }
                    //check right for Pink Cell
                    if (right.isAlive && right.thisColor == 3)
                    {
                        numberOfInvaders++;
                    }
                    //check down for Yellow Cell
                    if (down.isAlive && down.thisColor == 0)
                    {
                        numberOfInvaders++;
                    }
                    //check left for Turquoise Cell
                    if (left.isAlive && left.thisColor == 1)
                    {
                        numberOfInvaders++;
                    }
                    //are there at least 2 Cells pushing into this one?
                    if (numberOfInvaders >= 2)
                    {
                        //we turn white
                        nextTickAlive = true;
                        nextTickColor = 4;
                    }
                    //if we do have 2 neighbours but only 1 invader, let it push into this
                    else if (numberOfInvaders == 1)
                    {
                        //check up for alive Blue Cell
                        if (up.isAlive && up.thisColor == 2)
                        {
                            //we turn Blue
                            nextTickAlive = true;
                            nextTickColor = 2;
                        }
                        //check right for alive Pink Cell
                        if (right.isAlive && right.thisColor == 3)
                        {
                            //we turn Pink
                            nextTickAlive = true;
                            nextTickColor = 3;
                        }
                        //check down for alive Yellow Cell
                        if (down.isAlive && down.thisColor == 0)
                        {
                            //we turn Yellow
                            nextTickAlive = true;
                            nextTickColor = 0;
                        }
                        //check left for alive Turquoise Cell
                        if (left.isAlive && left.thisColor == 1)
                        {
                            //we turn Turquoise
                            nextTickAlive = true;
                            nextTickColor = 1;
                        }
                    }
                }
            }            
        }
        //are we dead?
        else if (this.isAlive == false)
        {
            //do we have neighbours?
            if (numberOfNeighbours == 0)
            {
                //we stay dead
                nextTickAlive = false;
            }
            //do we have exactly 1 neighbour?
            if (numberOfNeighbours == 1)
            {
                //check up for alive Blue Cell
                if (up.isAlive && up.thisColor == 2)
                {
                    //we turn Blue
                    nextTickAlive = true;
                    nextTickColor = 2;
                }
                //check right for alive Pink Cell
                if (right.isAlive && right.thisColor == 3)
                {
                    //we turn Pink
                    nextTickAlive = true;
                    nextTickColor = 3;
                }
                //check down for alive Yellow Cell
                if (down.isAlive && down.thisColor == 0)
                {
                    //we turn Yellow
                    nextTickAlive = true;
                    nextTickColor = 0;
                }
                //check left for alive Turquoise Cell
                if (left.isAlive && left.thisColor == 1)
                {
                    //we turn Turquoise
                    nextTickAlive = true;
                    nextTickColor = 1;
                }
            }
            //if we have 2 or more neighbours
            else if (numberOfNeighbours >= 2)
            {
                //check up for Blue Cell
                if (up.isAlive && up.thisColor == 2)
                {
                    numberOfInvaders++;
                }
                //check right for Pink Cell
                if (right.isAlive && right.thisColor == 3)
                {
                    numberOfInvaders++;
                }
                //check down for Yellow Cell
                if (down.isAlive && down.thisColor == 0)
                {
                    numberOfInvaders++;
                }
                //check left for Turquoise Cell
                if (left.isAlive && left.thisColor == 1)
                {
                    numberOfInvaders++;
                }
                //are there at least 2 Cells pushing into this one?
                if (numberOfInvaders >= 2)
                {
                    //we turn white
                    nextTickAlive = true;
                    nextTickColor = 4;
                }
                //if we do have 2 neighbours but only 1 invader, let it push into this
                else if (numberOfInvaders == 1)
                {
                    //check up for alive Blue Cell
                    if (up.isAlive && up.thisColor == 2)
                    {
                        //we turn Blue
                        nextTickAlive = true;
                        nextTickColor = 2;
                    }
                    //check right for alive Pink Cell
                    if (right.isAlive && right.thisColor == 3)
                    {
                        //we turn Pink
                        nextTickAlive = true;
                        nextTickColor = 3;
                    }
                    //check down for alive Yellow Cell
                    if (down.isAlive && down.thisColor == 0)
                    {
                        //we turn Yellow
                        nextTickAlive = true;
                        nextTickColor = 0;
                    }
                    //check left for alive Turquoise Cell
                    if (left.isAlive && left.thisColor == 1)
                    {
                        //we turn Turquoise
                        nextTickAlive = true;
                        nextTickColor = 1;
                    }
                }
            }
        }
        numberOfInvaders = 0;
        gameManager.GetComponent<GameManager>().CellCount("midTickOver");
    }

    //replaces current variables with upcoming nextTickVariables to apply changes
    public void EndTick()
    {
        SetStatus(nextTickAlive, nextTickColor);
        nextTickAlive = false;
        gameManager.GetComponent<GameManager>().CellCount("endTickOver");
    }

    //gets called by GameManager to update colors set by UI
    public void UpdateColors(Color[] newColors)
    {
        colors = newColors;
        SetStatus(isAlive, thisColor);
    }

    //called by GameManager
    //connects directional neighbour cell variables to cells from array for cleaner code
    public void CheckNeighbours()
    {
        up = neighbours[0];
        upRight = neighbours[1];
        right = neighbours[2];
        rightDown = neighbours[3];
        down = neighbours[4];
        downLeft = neighbours[5];
        left = neighbours[6];
        leftUp = neighbours[7];
    }
}
