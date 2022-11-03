using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollwerCount : MonoBehaviour
{
    [SerializeField] GameObject[] Followers;

    public int player1FollowerCount = 0;
    public int player2FollowerCount = 0;


    public void CountFollowerColor()
    {
        Followers = GameObject.FindGameObjectsWithTag("Follower");
        for (int i = 0; i < Followers.Length; i++)
        {
            if (Followers[i].GetComponent<MeshRenderer>().material.color == Color.red)
            {
                player1FollowerCount++;
            }
            else if (Followers[i].GetComponent<MeshRenderer>().material.color == Color.blue)
            {
                player2FollowerCount++;
            }
        }
    }

}
