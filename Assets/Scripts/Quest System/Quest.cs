using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum QuestType
{
    Tutorial,
    TalkToNPC,
    Collection,
    Escort,
    Input,
    PickUp
}
[Serializable]
public class QuestInfo
{
    public string questName;
    public string questDescription;
    public QuestType questType;
    public GameObject[] objectiveItems;
    public bool turnInQuest;
    [Header("Use only if turn in quest is true")]
    public GameObject NPC;
    [Header("Use only if input quest")]
    public UnityEngine.InputSystem.InputAction input;
    public GameObject[] rewards;
    [HideInInspector]
    public bool active;
    [HideInInspector]
    public bool completed;
    [HideInInspector]
    public bool submitQuest;
}
public class Quest : MonoBehaviour
{
    [HideInInspector]
    public Interactor interactor;
    //tutorial is 0, Talk to NPC is 1, Collection is 2, Escort is 3 
    public MonoBehaviour[] questScript;
    //public Image questItem;
    //public Color completedColor;
    //public Color currentColor;
    public TextMeshProUGUI text;
    public TextMeshProUGUI nextQuestText;

    public QuestInfo[] allQuests;
    public List<QuestInfo> currentQuests = new List<QuestInfo>();
    [HideInInspector]
    public QuestInfo activeQuest;

    private void Start()
    {
        interactor = GameObject.FindGameObjectWithTag("Player").GetComponent<Interactor>();
        //currentColor = questItem.color;
    }
    //activates the quest based off the name
    public void ActivateQuest(string nameOfQuest)
    {
        nameOfQuest = nameOfQuest.ToUpper();
        //checks all the quests in the quest list to find it
        foreach (QuestInfo quest in allQuests)
        {
            //checks to see if the names match
            if(quest.questName.ToUpper() == nameOfQuest)
            {
                //if so activate the quest and add it to the list of current quests
                quest.active = true;
                currentQuests.Add(quest);
                //if there is no active quest make it the active quest
                if (activeQuest.questName == string.Empty)
                {
                    activeQuest = quest;
                    print(activeQuest.questName);
                    processQuest(activeQuest);
                }
            }
        }
        UpdateText();
    }
    //Updates the quest and sees if its ready to be complete or not
    public void UpdateQuest()
    {
        //if its a npc quest finish the quest
        if (activeQuest.questType == QuestType.TalkToNPC)
        {
            activeQuest.completed = true;
        }
        //updates the quest
        if (activeQuest.completed)
        {
            print("quest updated");
            activeQuest.submitQuest = true;
            NextQuest();
        }
    }
    void processQuest(QuestInfo quest)
    {
        //checks to see which quest to activate out of all the quests
        switch (quest.questType)
        {
            case QuestType.Tutorial:
                questScript[0].enabled = true;
                questScript[0].GetComponent<TutorialQuest>().startQuest(activeQuest, this);
                break;
            case QuestType.TalkToNPC:
                questScript[1].enabled = true;
                questScript[1].GetComponent<TalkToNPCQuest>().startQuest(activeQuest, this);
                break;
            case QuestType.Collection:
                questScript[2].enabled = true;
                questScript[2].GetComponent<CollectionQuest>().startQuest(activeQuest, this);
                break;
            case QuestType.Escort:
                questScript[3].enabled = true;
                questScript[3].GetComponent<EscortQuest>().startQuest(activeQuest, this);
                break;
            case QuestType.Input:
                questScript[4].enabled = true;
                questScript[4].GetComponent<ButtonQuest>().startQuest(activeQuest, this, activeQuest.input);
                break;
            case QuestType.PickUp:
                questScript[5].enabled = true;
                questScript[5].GetComponent<PickUpQuest>().startQuest(activeQuest, this);
                break;
            default:
                print("quest not found");
                break;
        }
        UpdateText();
    }
    //moves onto the next quest and sees if its ready to turn in or not
    public void NextQuest()
    {
        if (!activeQuest.completed)
        {
            activeQuest.completed = true;
        }
        if (!activeQuest.turnInQuest)
        {
            activeQuest.submitQuest = true;
        }
        if (activeQuest.turnInQuest)
        {
            text.text = $"Return to {activeQuest.NPC.name}";
        }
        //checks to see if the quest is ready to be turned in
        if (activeQuest.submitQuest == true)
        {
            //if there are rewards activate them
            if(activeQuest.rewards.Length != 0)
            {
                foreach(GameObject gO in activeQuest.rewards)
                {
                    gO.SetActive(true);
                }
            }
            //remove the quest from the list
            currentQuests.Remove(activeQuest);
            //if there is another quest activate the next quest on the list
            if (currentQuests.Count != 0)
            {
                activeQuest = currentQuests[0];
                activeQuest.active = true;
                processQuest(activeQuest);
                print(activeQuest.questName);
            }
            //otherwise move on
            else
            {
                activeQuest.active = false;
            }
        }
        UpdateText();
    }
    //Update the text of both the active quests and the other quests
    public void UpdateText()
    {
        if(activeQuest.active)
        {
            text.text = $"{activeQuest.questName}:\n{activeQuest.questDescription}";
            nextQuestText.text = "";
            foreach (QuestInfo quest in currentQuests)
            {
                if(quest != activeQuest)
                {
                    nextQuestText.text += $">{quest.questName}\n";
                }
            }
        }
        if(currentQuests.Count == 0)
        {
            text.text = "No Active Quests";
        }
    }

    //rework this
    public void onQuestClick()
    {
        /*
        foreach(QuestInfo quest in allQuests)
        {
            //quest.questItem.color = quest.currentColor;
        }
        questItem.color = activeColor;
        */
    }
}
