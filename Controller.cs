using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject snake; //Snake
    public GameObject snakeElement; //prefab
    public float speed = 1f;

    private GameObject[,] MA; //matrice di GameObject
    private int xItem, yItem; //Fruit
    private int xSnake, ySnake; //posizione dello Snake
    private int xSnakePred, ySnakePred; // posizione precedente dello Snake

    private bool up = false;
    private bool down = false;
    private bool left = false;
    private bool right = false;

    private List<GameObject> snakeParts; // parti dello Snake
    private List<Vector3> tmpPosParts;

    private float progress = 0;


    void Start()
    {
        snakeParts = new List<GameObject>();
        tmpPosParts = new List<Vector3>();
        snakeParts.Add(snake.transform.GetChild(4).gameObject);

        MA = new GameObject[8,8];

        for(int i = 0; i < 8; i++) {
            for(int j = 0; j < 8; j++) {
                MA[i,j] = GameObject.Find("M" + i + "" + j);
            }
        }

        xSnake = Random.Range(0, 7);
        ySnake = Random.Range(0, 7);

        snake.transform.position = MA[xSnake,ySnake].transform.position;

        SetRandomItem();
    }


    void Update()
    {
        if(Input.GetKeyDown("w") && !down) {
            up = true;
            down = false; 
            left = false;
            right = false;
        }

        if(Input.GetKeyDown("a") && !right) {
            up = false;
            down = false; 
            left = true;
            right = false;
        }

        if(Input.GetKeyDown("s") && !up) {
            up = false;
            down = true; 
            left = false;
            right = false;
        }

        if(Input.GetKeyDown("d") && !left) {
            up = false;
            down = false; 
            left = false;
            right = true;
        }

        if(progress == 0) {
            xSnakePred = xSnake;
            ySnakePred = ySnake;

            tmpPosParts.Clear();

            for(int i = 0; i < snakeParts.Count; i++) {
                tmpPosParts.Add(snakeParts[i].transform.position);
            }

            if(up) {
                xSnake--;
            } else if(down) {
                xSnake++;
            } else if(left) {
                ySnake--;
            } else if(right) {
                ySnake++;
            }
        }
        
        if(xSnake > 7 || ySnake > 7 || xSnake < 0 || ySnake < 0) {
            print("Game Over!");
        } 
		//<-[T][][][][]
		else {
            float distance = Vector3.Distance(snake.transform.position, MA[xSnake, ySnake].transform.position);
            progress += speed * Time.deltaTime / distance;
            snake.transform.position = Vector3.Lerp(snake.transform.position, MA[xSnake, ySnake].transform.position, progress);
            int j=0;
            print(tmpPosParts.Count + " : " + snakeParts.Count);
			for(int i=1;i<snakeParts.Count;i++) {
                // float distancePart = Vector3.Distance(snakeParts[i].transform.position, snakeParts[j].transform.position);
                // float progressPart = speed * Time.deltaTime / distancePart;
				snakeParts[i].transform.position = Vector3.Lerp(snakeParts[i].transform.position, tmpPosParts[i - 1], progress);	
			}
			
			if(progress >= 1) {
                progress = 0;
            }
			
        }
    }

    public void SetRandomItem() {
        for(int i = 0; i < 8; i++) {
            for(int j = 0; j < 8; j++) {
                MA[i,j].GetComponent<Renderer>().enabled = false;
            }
        }

        do {
            xItem = Random.Range(0, 7);
            yItem = Random.Range(0, 7);
        } while(xItem == xSnake && yItem == ySnake);

        MA[xItem,yItem].GetComponent<Renderer>().enabled = true;
    }

    public void IncrementSnake(Transform itemTransform) {
        GameObject tail = Instantiate(snakeElement, itemTransform.position, itemTransform.rotation);
		if(snakeParts.Count>1)
		{
			Transform t = snakeParts[snakeParts.Count-1].transform;
			tail.transform.position = t.position;
		}
		else
			tail.transform.position = MA[xSnakePred, ySnakePred].transform.position;
        
        tail.transform.SetParent(snake.transform);
        snakeParts.Add(tail);
    }
}

