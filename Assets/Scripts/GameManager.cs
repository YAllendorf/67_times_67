using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System.Xml;

public class GameManager : MonoBehaviour
{
    #region variables
    //playable area
    private static int screenWidth = 67;
    private static int screenHeight = 67;

    //for game speed
    public float speed = 0.1f;
    private float timer = 0f;
    private float timerReset = 1f;

    //array that holds colors
    public Color[] colors;

    // array that holds all cells, array size equals Width*Height of playable area
    public Cell[,] grid = new Cell[screenWidth, screenHeight];


    //////////////
    ///UI stuff///
    //////////////

    //which toggle is active
    public bool yellowIsActive = false;
    public bool turquoiseIsActive = false;
    public bool blueIsActive = false;
    public bool pinkIsActive = false;

    //what size of a brush was selected
    public int brushSize = 1;

    //toggles for color picking
    public GameObject colorWheel;
    public ToggleGroup colorToggles;
    public Toggle toggleYellow;
    public Toggle toggleTurquoise;
    public Toggle toggleBlue;
    public Toggle togglePink;

    //colorWheel background images
    public Image yellowActive;
    public Image turquoiseActive;
    public Image blueActive;
    public Image pinkActive;

    //array that holds all UI images, that change their color when colors are changed
    public Image[] yellowImages;
    public Image[] turquoiseImages;
    public Image[] blueImages;
    public Image[] pinkImages;

    //play, pause and fast sprites to indicate gameSpeed
    public Sprite[] speedSprites;
    public GameObject speedSprite;

    //brush size sprites to indicate size
    public Sprite[] brushSprites;
    public GameObject brushSprite;

    //bools for using colorWheel
    public bool colorWheelOn = false;

    public Slider r;
    public Slider g;
    public Slider b;

    //count for Cells to make sure every cell finished the corresponding phase
    public int countStartTick;
    public int countMidTick;
    public int countEndTick;
    public bool tickOver = true;

    //saving/loading
    private string saveString = null;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SetupCells();
        toggleYellow.image.color = colors[0];
        toggleTurquoise.image.color = colors[1];
        toggleBlue.image.color = colors[2];
        togglePink.image.color = colors[3];
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= timerReset && tickOver == true)
        {
            timer = 0f;
            tickOver = false;
            foreach (Cell cell in grid)
            {
                cell.StartTick(this.gameObject);
            }
        }
        else
        {
            timer += Time.deltaTime * speed;
        }
        UserInput();
    }

    //instantiating Cells, sets each Cell up with its neighbours, sets each Cell to dead and without a sprite
    void SetupCells()
    {
        //Instantiate Cells
        for (int y = 0; y < screenHeight; y++)
        {
            for (int x = 0; x < screenWidth; x++)
            {
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                grid[x, y] = cell;
            }
        }

        //setup neighbours
        //make grid wrap around
        for (int y = 0; y < screenHeight; y++)
        {
            for (int x = 0; x < screenWidth; x++)
            {
                //define neighbour
                Cell up = null;
                Cell upRight = null;
                Cell right = null;
                Cell rightDown = null;
                Cell down = null;
                Cell downLeft = null;
                Cell left = null;
                Cell leftUp = null;

                //up row exception
                if (x > 0 && x + 1 < screenWidth && y + 1 == screenHeight)
                {
                    up = grid[x, 0];
                    upRight = grid[x + 1, 0];
                    right = grid[x + 1, y];
                    rightDown = grid[x + 1, y - 1];
                    down = grid[x, y - 1];
                    downLeft = grid[x - 1, y - 1];
                    left = grid[x - 1, y];
                    leftUp = grid[x - 1, 0];
                }
                //up right exception
                else if (x + 1 == screenWidth && y + 1 == screenHeight)
                {
                    up = grid[x, 0];
                    upRight = grid[0, 0];
                    right = grid[0, y];
                    rightDown = grid[0, y - 1];
                    down = grid[x, y - 1];
                    downLeft = grid[x - 1, y - 1];
                    left = grid[x - 1, y];
                    leftUp = grid[x - 1, 0];
                }
                //right row exception
                else if (x + 1 == screenWidth && y > 0 && y + 1 < screenHeight)
                {
                    up = grid[x, y + 1];
                    upRight = grid[0, y + 1];
                    right = grid[0, y];
                    rightDown = grid[0, y - 1];
                    down = grid[x, y - 1];
                    downLeft = grid[x - 1, y - 1];
                    left = grid[x - 1, y];
                    leftUp = grid[x - 1, y + 1];
                }
                //down right exception
                else if (x + 1 == screenWidth && y == 0)
                {
                    up = grid[x, y + 1];
                    upRight = grid[0, y + 1];
                    right = grid[0, y];
                    rightDown = grid[0, screenHeight - 1];
                    down = grid[x, screenHeight - 1];
                    downLeft = grid[x - 1, screenHeight - 1];
                    left = grid[x - 1, y];
                    leftUp = grid[x - 1, y + 1];
                }
                //down row exception
                else if (x > 0 && x + 1 < screenWidth && y == 0)
                {
                    up = grid[x, y + 1];
                    upRight = grid[x + 1, y + 1];
                    right = grid[x + 1, y];
                    rightDown = grid[x + 1, screenHeight - 1];
                    down = grid[x, screenHeight - 1];
                    downLeft = grid[x - 1, screenHeight - 1];
                    left = grid[x - 1, y];
                    leftUp = grid[x - 1, y + 1];
                }
                //down left exception
                else if (x == 0 && y == 0)
                {
                    up = grid[x, y + 1];
                    upRight = grid[x + 1, y + 1];
                    right = grid[x + 1, y];
                    rightDown = grid[x + 1, screenHeight - 1];
                    down = grid[x, screenHeight - 1];
                    downLeft = grid[screenWidth - 1, screenHeight - 1];
                    left = grid[screenWidth - 1, y];
                    leftUp = grid[screenWidth - 1, y + 1];
                }
                //left row exception
                else if (x == 0 && y > 0 && y + 1 < screenHeight)
                {
                    up = grid[x, y + 1];
                    upRight = grid[x + 1, y + 1];
                    right = grid[x + 1, y];
                    rightDown = grid[x + 1, y - 1];
                    down = grid[x, y - 1];
                    downLeft = grid[screenWidth - 1, y - 1];
                    left = grid[screenWidth - 1, y];
                    leftUp = grid[screenWidth - 1, y + 1];
                }
                //left up exception
                else if (x == 0 && y + 1 == screenHeight)
                {
                    up = grid[0, 0];
                    upRight = grid[x + 1, 0];
                    right = grid[x + 1, y];
                    rightDown = grid[x + 1, y - 1];
                    down = grid[x, y - 1];
                    downLeft = grid[screenWidth - 1, y - 1];
                    left = grid[screenWidth - 1, y];
                    leftUp = grid[screenWidth - 1, 0];
                }
                //inner cells
                else if (x > 0 && x + 1 < screenWidth && y > 0 && y + 1 < screenHeight)
                {
                    up = grid[x, y + 1];
                    upRight = grid[x + 1, y + 1];
                    right = grid[x + 1, y];
                    rightDown = grid[x + 1, y - 1];
                    down = grid[x, y - 1];
                    downLeft = grid[x - 1, y - 1];
                    left = grid[x - 1, y];
                    leftUp = grid[x - 1, y + 1];
                }

                //setup neighbours
                grid[x, y].neighbours = new Cell[]
                {
                    up,
                    upRight,
                    right,
                    rightDown,
                    down,
                    downLeft,
                    left,
                    leftUp
                };
                grid[x, y].CheckNeighbours();
            }
        }

        foreach (Cell activeCell in grid)
        {
            activeCell.SetStatus(false, 5);
            activeCell.colors = colors;
        }
    }

    //count cells to make sure that every cell ended the corresponding phase
    //if all cells responded the next phase for all cells is started
    //phases happen in Cell.cs
    public void CellCount(string phase)
    {
        if (phase == "startTickOver")
        {
            countStartTick++;
            if (countStartTick >= grid.Length)
            {
                foreach (Cell activeCell in grid)
                {
                    activeCell.MidTick();
                }
                countStartTick = 0;
            }
        }
        if (phase == "midTickOver")
        {
            countMidTick++;
            if (countMidTick >= grid.Length)
            {
                foreach (Cell activeCell in grid)
                {
                    activeCell.EndTick();
                }
                countMidTick = 0;
            }
        }
        if (phase == "endTickOver")
        {
            countEndTick++;
            if (countEndTick >= grid.Length)
            {
                tickOver = true;
                countEndTick = 0;
            }
        }
    }

    //Inputs:
    //left click to interact with UI or place Cells
    //mid click to reset
    //R for random Cells(for debugging
    //WASD or arrows for color picking shortcuts
    void UserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePoint.x);
            int y = Mathf.RoundToInt(mousePoint.y);

            //if we are in bounds
            if (x >= 0 && y >= 0 && x < screenWidth && y < screenHeight)
            {
                if (brushSize == 1)
                {
                    //set alive status to opposite of current status if it should be dead
                    if (grid[x, y].isAlive)
                    {
                        grid[x, y].SetStatus(false, 5);
                    }
                    else
                    {
                        if (yellowIsActive == true)
                        {
                            grid[x, y].SetStatus(true, 0);
                        }
                        else if (turquoiseIsActive == true)
                        {
                            grid[x, y].SetStatus(true, 1);
                        }
                        else if (blueIsActive == true)
                        {
                            grid[x, y].SetStatus(true, 2);
                        }
                        else if (pinkIsActive == true)
                        {
                            grid[x, y].SetStatus(true, 3);
                        }
                    }
                }
                //is the brushssize is bigger than 1
                else if (brushSize > 1)
                {
                    //calculate affected Cells with brushSize
                    int xMin = x - brushSize / 2;
                    int xMax = x + brushSize / 2;
                    int yMin = y - brushSize / 2;
                    int yMax = y + brushSize / 2;

                    //change all Cells that are within bounds of the brushSize
                    for (int i = xMin; i < xMax; i++)
                    {
                        for (int j = yMin; j < yMax; j++)
                        {
                            //bounds check
                            if (grid[i, j] != null)
                            {
                                //set alive status to opposite of current status with random color if it should be dead
                                if (grid[i, j].isAlive)
                                {
                                    grid[i, j].SetStatus(false, 5);
                                }
                                else
                                {
                                    if (yellowIsActive == true)
                                    {
                                        grid[i, j].SetStatus(true, 0);
                                    }
                                    else if (turquoiseIsActive == true)
                                    {
                                        grid[i, j].SetStatus(true, 1);
                                    }
                                    else if (blueIsActive == true)
                                    {
                                        grid[i, j].SetStatus(true, 2);
                                    }
                                    else if (pinkIsActive == true)
                                    {
                                        grid[i, j].SetStatus(true, 3);
                                    }
                                }
                            }
                        }
                    }
                }                  
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            RearrangeCells();
        }

        if (Input.GetMouseButtonDown(2))
        {
            ClearCells();
        }

        //press WASD or up, left, down, right for color shortcuts
        #region colorShortcuts
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            yellowIsActive = true;
            turquoiseIsActive = false;
            blueIsActive = false;
            pinkIsActive = false;
            toggleYellow.isOn = true;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            yellowIsActive = false;
            turquoiseIsActive = false;
            blueIsActive = false;
            pinkIsActive = true;
            togglePink.isOn = true;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            yellowIsActive = false;
            turquoiseIsActive = false;
            blueIsActive = true;
            pinkIsActive = false;
            toggleBlue.isOn = true;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            yellowIsActive = false;
            turquoiseIsActive = true;
            blueIsActive = false;
            pinkIsActive = false;
            toggleTurquoise.isOn = true;
        }
        #endregion
    }

    //resets all Cells
    void ClearCells()
    {
        foreach (Cell activeCell in grid)
        {
            activeCell.SetStatus(false, 5);
        }
    }

    //randomizers for R-press
    void RearrangeCells()
    {
        foreach (Cell activeCell in grid)
        {
            activeCell.SetStatus(false, 5);
        }
        foreach (Cell activeCell in grid)
        {
            if (activeCell.GetComponent<SpriteRenderer>().enabled == false)
            {
                activeCell.SetStatus(RandomAliveCell(), RandomCellColor());
            }
        }
    }
    bool RandomAliveCell()
    {
        int randomNumber = UnityEngine.Random.Range(0, 100);

        if (randomNumber > 95)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    int RandomCellColor()
    {
        int randomIndex = UnityEngine.Random.Range(0, 4);
        return randomIndex;
    }

    #region UI
    //changes what color placed Cells will have to the selected color in the colorWheel
    //matches slider values with selected color
    public void ColorUI(Toggle clickedToggle)
    {
        if (clickedToggle == toggleYellow)
        {
            yellowIsActive = true;
            turquoiseIsActive = false;
            blueIsActive = false;
            pinkIsActive = false;
            r.value = colors[0].r;
            g.value = colors[0].g;
            b.value = colors[0].b;
        }
        if (clickedToggle == toggleTurquoise)
        {
            yellowIsActive = false;
            turquoiseIsActive = true;
            blueIsActive = false;
            pinkIsActive = false;
            r.value = colors[1].r;
            g.value = colors[1].g;
            b.value = colors[1].b;
        }
        if (clickedToggle == toggleBlue)
        {
            yellowIsActive = false;
            turquoiseIsActive = false;
            blueIsActive = true;
            pinkIsActive = false;
            r.value = colors[2].r;
            g.value = colors[2].g;
            b.value = colors[2].b;
        }
        if (clickedToggle == togglePink)
        {
            yellowIsActive = false;
            turquoiseIsActive = false;
            blueIsActive = false;
            pinkIsActive = true;
            r.value = colors[3].r;
            g.value = colors[3].g;
            b.value = colors[3].b;
        }
    }

    //three methods for 3 rgb slider
    public void ChangeColorR(float value)
    {
        if (yellowIsActive)
        {
            colors[0].r = value;
            toggleYellow.image.color = colors[0];
            yellowActive.color = colors[0];
        }
        else if (turquoiseIsActive)
        {
            colors[1].r = value;
            toggleTurquoise.image.color = colors[1];
            turquoiseActive.color = colors[1];
        }
        else if (blueIsActive)
        {
            colors[2].r = value;
            toggleBlue.image.color = colors[2];
            blueActive.color = colors[2];
        }
        else if (pinkIsActive)
        {
            colors[3].r = value;
            togglePink.image.color = colors[3];
            pinkActive.color = colors[3];
        }
        ColorChange();
    }
    public void ChangeColorG(float value)
    {
        if (yellowIsActive)
        {
            colors[0].g = value;
            toggleYellow.image.color = colors[0];
            yellowActive.color = colors[0];
        }
        else if (turquoiseIsActive)
        {
            colors[1].g = value;
            toggleTurquoise.image.color = colors[1];
            turquoiseActive.color = colors[1];
        }
        else if (blueIsActive)
        {
            colors[2].g = value;
            toggleBlue.image.color = colors[2];
            blueActive.color = colors[2];

        }
        else if (pinkIsActive)
        {
            colors[3].g = value;
            togglePink.image.color = colors[3];
            pinkActive.color = colors[3];
        }
        ColorChange();
    }
    public void ChangeColorB(float value)
    {
        if (yellowIsActive)
        {
            colors[0].b = value;
            toggleYellow.image.color = colors[0];
            yellowActive.color = colors[0];
        }
        else if (turquoiseIsActive)
        {
            colors[1].b = value;
            toggleTurquoise.image.color = colors[1];
            turquoiseActive.color = colors[1];
        }
        else if (blueIsActive)
        {
            colors[2].b = value;
            toggleBlue.image.color = colors[2];
            blueActive.color = colors[2];
        }
        else if (pinkIsActive)
        {
            colors[3].b = value;
            togglePink.image.color = colors[3];
            pinkActive.color = colors[3];
        }
        ColorChange();
    }

    //refreshes Colors for Cells after each slider method
    public void ColorChange()
    {
        //change all assigned sprites and images from the UI to the new chosen color
        //done seperately for each color
        foreach (Image yellow in yellowImages)
        {
            yellow.color = colors[0];
        }
        foreach (Image turquoise in turquoiseImages)
        {
            turquoise.color = colors[1];
        }
        foreach (Image blue in blueImages)
        {
            blue.color = colors[2];
        }
        foreach (Image pink in pinkImages)
        {
            pink.color = colors[3];
        }

        //change color pallets of all cells
        foreach (Cell activeCell in grid)
        {
            activeCell.UpdateColors(colors);
        }
    }

    //change game speed and show change with corresponding icon
    public void ChangeSpeed(float speed)
    {
        if (speed >= 0 && speed <= 100)
        {
            this.speed = speed;
        }
        //change Sprite
        if (speed == 0)
        {
            speedSprite.GetComponent<Image>().sprite = speedSprites[0];
        }
        else if (speed > 0 && speed < 50)
        {
            speedSprite.GetComponent<Image>().sprite = speedSprites[1];
        }

        else if (speed <= 100 && speed >= 50)
        {
            speedSprite.GetComponent<Image>().sprite = speedSprites[2];
        }
    }

    //change brush size and show change with corresponding icon
    public void ChangeBrush(float brushSizeFloat)
    {
        int brushSize = Mathf.RoundToInt(brushSizeFloat);
        if (brushSize >= 0 && brushSize <= 67)
        {
            this.brushSize = brushSize;
        }
        //change Sprite
        if (brushSize == 1)
        {
            brushSprite.GetComponent<Image>().sprite = brushSprites[0];
        }
        else if (brushSize > 1 && brushSize <= 9)
        {
            brushSprite.GetComponent<Image>().sprite = brushSprites[1];
        }
        else if (brushSize > 9 && brushSize <= 18)
        {
            brushSprite.GetComponent<Image>().sprite = brushSprites[2];
        }
        else if (brushSize > 18 && brushSize <= 27)
        {
            brushSprite.GetComponent<Image>().sprite = brushSprites[3];
        }
        else if (brushSize > 27 && brushSize <= 36)
        {
            brushSprite.GetComponent<Image>().sprite = brushSprites[4];
        }
        else if (brushSize > 36 && brushSize <= 45)
        {
            brushSprite.GetComponent<Image>().sprite = brushSprites[5];
        }
        else if (brushSize > 45 && brushSize <= 54)
        {
            brushSprite.GetComponent<Image>().sprite = brushSprites[6];
        }
        else if (brushSize > 54 && brushSize <= 67)
        {
            brushSprite.GetComponent<Image>().sprite = brushSprites[7];
        }
    }
    #endregion

    #region saving and loading
    //on button press the game in its current state is written to a text file and copied to the clipboard
    public void SaveGame()
    {
        saveString = null;
        for (int y = 0; y < screenHeight; y++)
        {
            for (int x = 0; x < screenWidth; x++)
            {
                if (grid[x, y].isAlive == false)
                {
                    saveString += "0";
                }
                else
                {
                    saveString += "1";
                }
                saveString += grid[x, y].thisColor;
                GUIUtility.systemCopyBuffer = saveString;
            }
        }
    }

    public void Import()
    {
        saveString = GUIUtility.systemCopyBuffer;
        if (saveString.Length != 8978)
        {
            return;
        }
        else
        {
            int x = 0, y = 0;

            for (int z = 0; z < 4489; z++)
            {
                grid[x, y].SetStatus(Convert.ToBoolean(Convert.ToInt32(saveString.Substring((z * 2), 1))), int.Parse(saveString.Substring(((z * 2) + 1), 1)));
                x++;

                if (x == screenWidth)
                {
                    x = 0;
                    y++;
                }
            }
        }
    }
    #endregion
}
//TODO:
//rule changing
//save/load
//sounds
//selection tool + copy/paste
//implement switch cases
//skip 1 frame ahead/back
//preview where Cells are going to be placed

//IDEAS
//custom area size
//sounds
//rule changing
//selection tool and copy/paste
//save/load with seeds
//skip frame forward/backward
