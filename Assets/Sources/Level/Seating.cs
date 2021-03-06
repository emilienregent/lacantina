﻿using La.Cantina.UI;
using UnityEngine;

public class Seating : MonoBehaviour
{
    [SerializeField] private Seat[] seats = null;
    [SerializeField] private PlayerTableCanvasController _playerTableCanvas = null;

    public PlayerTableCanvasController playerTableCanvas { get { return _playerTableCanvas; } }

    private void ShuffleSeats()
    {
        System.Random rng = new System.Random(Random.Range(int.MinValue, int.MaxValue));

        for (int i = 0; i < seats.Length; ++i)
        {
            int key = rng.Next(i, seats.Length);
            Seat tmp = seats[i];
            seats[i] = seats[key];
            seats[key] = tmp;
        }
    }

    public Seat GetEmptySeat()
    {
        ShuffleSeats();

        for (int i = 0; i < seats.Length; ++i)
        {
            if (!seats[i].isClaimed)
                return seats[i];
        }

        return null;
    }
}
