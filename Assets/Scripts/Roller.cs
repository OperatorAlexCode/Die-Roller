using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Roller : MonoBehaviour
{
    // float
    public float MinForce;
    public float MaxForce;
    public float MinTorque;
    public float MaxTorque;
    public float RollInitiateSize;

    // GameObject
    public GameObject Die;
    List<GameObject> Dice = new();
    public GameObject NorthBound;
    public GameObject SouthBound;
    public GameObject EastBound;
    public GameObject WestBound;

    // Int
    public int RollForceMinMax;
    public int RollTorqueMinMax;
    public int DefaultDiceAmount;

    // Other
    public Camera GameCamera;
    string SavedDiceAmountKey = "DiceAmount";

    // Start is called before the first frame update
    void Start()
    {
        float xBound = GameCamera.aspect * GameCamera.orthographicSize * 2f;
        float zBound = GameCamera.orthographicSize * 2f;

        NorthBound.transform.position = new Vector3(0, 0, GameCamera.transform.position.z + zBound / 2);
        SouthBound.transform.position = new Vector3(0, 0, GameCamera.transform.position.z - zBound / 2);
        EastBound.transform.position = new Vector3(GameCamera.transform.position.x - xBound / 2, 0, 0);
        WestBound.transform.position = new Vector3(GameCamera.transform.position.x + xBound / 2, 0, 0);

        int diceAmount;

        if (!PlayerPrefs.HasKey(SavedDiceAmountKey))
            diceAmount = DefaultDiceAmount;

        else
            diceAmount = PlayerPrefs.GetInt(SavedDiceAmountKey);

        for (int x = 0; x < diceAmount; x++)
            AddDie();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RollDice()
    {
        foreach (GameObject die in Dice)
        {
            Vector3 forceVector = new Vector3(Random.Range(-RollForceMinMax, RollForceMinMax), Random.Range(1, RollForceMinMax), Random.Range(-RollForceMinMax, RollForceMinMax)).normalized;
            Vector3 torqueVector = new Vector3(Random.Range(-RollTorqueMinMax, RollTorqueMinMax), Random.Range(-RollTorqueMinMax, RollTorqueMinMax), Random.Range(-RollTorqueMinMax, RollTorqueMinMax)).normalized;
            Rigidbody rigidBody = die.GetComponent<Rigidbody>();
            rigidBody.AddForce(forceVector * Random.Range(MinForce, MaxForce), ForceMode.Impulse);
            rigidBody.AddTorque(torqueVector * Random.Range(MinTorque, MaxTorque), ForceMode.Impulse);
        }
    }

    public void AddDie()
    {
        GameObject newDie = Instantiate(Die);
        Dice.Add(newDie);
        SaveDiceAmount();
    }

    public void RemoveDie()
    {
        if (Dice.Count > 0)
        {
            int index = Dice.Count - 1;
            Destroy(Dice[index]);
            Dice.RemoveAt(index);
            SaveDiceAmount();
        }
    }

    public void SaveDiceAmount()
    {

        PlayerPrefs.SetInt(SavedDiceAmountKey, Dice.Count);
        PlayerPrefs.Save();
    }
}
