using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MainStoryFirst : StoryManager
{
    public GameObject personOld;
    async public void Start()
    {
        await MainQuestStart();
        //personOld.transform.Rotate(0f, 90f, 0f);
        StartCoroutine(MoveAndLookAtTarget(new Vector3(7, 0, 1)));
    }
    async public Task MainQuestStart()
    {
        await StartStory("MainStory/MS_1", "MainFirst");
        return;
    }
    private IEnumerator MoveAndLookAtTarget(Vector3 targetPosition)
    {
        Debug.Log(targetPosition);
        personOld.GetComponent<Animator>().SetBool("isWalk", true);

        while (Vector3.Distance(personOld.transform.position, targetPosition) > 0.1f)
        {
            // 목표를 향해 천천히 회전
            Vector3 direction = (targetPosition - personOld.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            personOld.transform.rotation = Quaternion.Slerp(personOld.transform.rotation, lookRotation, Time.deltaTime * 2.0f);

            // 목표를 향해 이동
            //personOld.transform.position = Vector3.MoveTowards(personOld.transform.position, targetPosition, 2.0f * Time.deltaTime);

            yield return null;  // 다음 프레임까지 대기
        }
    }
}
