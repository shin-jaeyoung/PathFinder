using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Item weapon;
    public Item helmet;
    public Item armor;
    public Item shoes;
    public Skill testSkill;
    public Skill testSwap;
    public Item extraItem;
    public Skill testDash;


    public Player player;
    void Start()
    {
        player = GameManager.instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.Inventory.AddItem(weapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.Inventory.AddItem(helmet);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.Inventory.AddItem(armor);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            player.Inventory.AddItem(shoes);
        }
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            player.Skills.AddActiveSkill(testSkill);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            player.Skills.AddActiveSkill(testSwap);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            player.Inventory.AddItem(extraItem);
        }
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            player.Skills.AddDashSkill(testDash);
        }
    }
}
