using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestSetting : MonoBehaviour
{
    public GameObject popupQuest;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;

    public TextMeshProUGUI questReward;
    public Button questOKBtn;
    public void SettingQuestUI(string title, string description, string reward)
    {
        questTitle.text = title;
        questDescription.text = description;
        questReward.text = reward;
    }
    public void ClickQuest()
    {

    }
    public void PopupQuest(bool isOn, string filename)
    {
        if (isOn == false)
        {
            popupQuest.SetActive(false);
        }
        else
        {
            // string path = Path.Combine(Application.dataPath, "scripts", "Quest", "QuestInfo.json");

            // // 파일의 텍스트를 string으로 저장
            // string jsonData = File.ReadAllText(path);
            // // 이 Json데이터를 역직렬화하여 playerData에 넣어줌
            // StoryContainer storyData = JsonUtility.FromJson<StoryContainer>(jsonData);
            // popupQuest.SetActive(true);

        }
    }
}
