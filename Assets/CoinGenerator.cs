using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    private int amountOfCoin;
    [SerializeField] private GameObject coinPrefab;

    [SerializeField] private int minCoin;
    [SerializeField] private int maxCoin;
    private void Start() {
        amountOfCoin = Random.Range(minCoin, maxCoin);
        int addOffset = amountOfCoin / 2;

        for (int i = 0; i < amountOfCoin; i++)
        {
            Vector3 offset = new Vector2(i - addOffset,0);
            Instantiate(coinPrefab,transform.position + offset, Quaternion.identity,transform);
        }
    }
}
