using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCounter : MonoBehaviour
{
    [SerializeField] GameObject[] Cubes;

    public int player1CubeCount = 0;
    public int player2CubeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cubes = GameObject.FindGameObjectsWithTag("Cube");
    }


    //게임시간이 끝난 후 실행 되어 화면에 표시되어야 한다. -> gameManager에서 실행
    public void CountCubeColor()
    {
        for(int i=0;i<Cubes.Length;i++)
        {
            if(Cubes[i].GetComponent<MeshRenderer>().material.color == Color.red)
            {
                player1CubeCount++;
            }
            else if(Cubes[i].GetComponent<MeshRenderer>().material.color == Color.blue)
            {
                player2CubeCount++;
            }
        }
    }
}
