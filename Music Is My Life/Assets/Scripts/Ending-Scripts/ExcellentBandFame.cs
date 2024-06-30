using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExcellentBandFame : MonoBehaviour
{
    public float typingSpeed; //타이핑 속도, 높을 수록 속도가 느림
    public float nextSceneSpeed; //다음 페이지로 넘어가는 속도, 높을 수록 속도가 느림

    public TMP_Text targetTxt;
    public TMP_Text targetTxt2;

    RectTransform rectTransform;
    RectTransform rectTransform2;
    TextMeshProUGUI textMeshPro;

    public string[] Dialogue;
    string[] dialogues;

    int dialogNum = 0;

    public GameObject[] illustrationObjects; // 일러스트 게임 오브젝트 배열
    private int currentIllustrationIndex = 0; // 현재 일러스트 인덱스

    public Button nextButton;
    public Button endButton; // 다음 일러스트로 넘어가는 버튼
    SceneMove sceneMover; // SceneMove 클래스의 인스턴스 생성

    public GameObject tourYellow;

    public AudioClip Sound1;
    public AudioClip Sound2;
    public AudioClip Sound3;

    private AudioSource audioSource;

    public GameObject[] Light;
    public GameObject audience;

    void Awake()
    {
        rectTransform = targetTxt.GetComponent<RectTransform>();
        rectTransform2 = targetTxt2.GetComponent<RectTransform>();
        textMeshPro = targetTxt.GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.clip = Sound1;
        audioSource.Play();
        StartTalk(Dialogue);
        if (illustrationObjects.Length == 1)
        {
            // 바로 End 버튼 활성화
            endButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);

        }
        else
        { endButton.gameObject.SetActive(false);

            // 모든 일러스트를 비활성화
            for (int i = 1; i < illustrationObjects.Length; i++)
            {
                illustrationObjects[i].SetActive(false);
            }

            RectTransform buttonRectTransform = nextButton.GetComponent<RectTransform>();
            buttonRectTransform.SetAsLastSibling();

            // 첫 번째 일러스트만 활성화
            illustrationObjects[0].SetActive(true);

            // 다음 버튼에 클릭 이벤트 추가
            nextButton.onClick.AddListener(NextIllustration);

            // TextMeshPro의 TMP_Text 컴포넌트 가져오기


            // SceneMove 스크립트를 찾아 인스턴스 생성
            sceneMover = FindObjectOfType<SceneMove>();
        }
    }

    void StartTalk (string[] talks)
    {
        dialogues = talks;
        StartCoroutine(Typing(dialogues[dialogNum]));
    }
    
    IEnumerator Typing(string talk)
    {
        targetTxt.text = null;

        if (talk.Contains("  "))
        {
            talk = talk.Replace("  ", "\n");
        }
        for (int i = 0; i < talk.Length; i++)
        {
            targetTxt.text += talk[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(nextSceneSpeed);
        //마지막 일러스트가 아닐 경우 다음 다이얼로그 진행
        if (currentIllustrationIndex != illustrationObjects.Length - 1) NextTalk();
    }

    void NextTalk()
    {
        targetTxt.text = null;
        dialogNum++;

        if (dialogNum == dialogues.Length)
        {
            dialogNum = 0;
            return;
        }
        NextIllustration();
        TypingAndIllustration();
        StartCoroutine(Typing(dialogues[dialogNum]));
    }

    // 다음 일러스트로 넘어가는 함수
    void NextIllustration()
    {
        // 현재 일러스트 숨기기
        illustrationObjects[currentIllustrationIndex].SetActive(false);

        // 다음 일러스트 인덱스 계산
        currentIllustrationIndex = (currentIllustrationIndex + 1) % illustrationObjects.Length;

        // 다음 일러스트 보이기
        illustrationObjects[currentIllustrationIndex].SetActive(true);

        // 일러스트 개수가 1개일 경우
        if (illustrationObjects.Length == 0)
        {
            // 바로 End 버튼 활성화
            endButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);

        }
        else
        {
            // 마지막 일러스트인 경우
            if (currentIllustrationIndex == illustrationObjects.Length - 1)
            {
                // 버튼 텍스트를 변경

                // Next 버튼 비활성화
                nextButton.gameObject.SetActive(false);

                // End 버튼 활성화
                endButton.gameObject.SetActive(true);
            }
            else
            {
                // 버튼 텍스트를 변경

                // Next 버튼 활성화
                // nextButton.gameObject.SetActive(true);
            }
        }
    }

    void TypingAndIllustration()
    {
        switch (currentIllustrationIndex)
        {   
            case 1:
                rectTransform.anchoredPosition = new Vector2 (140, -241);
                break;
            case 2:
                rectTransform.anchoredPosition = new Vector2 (85, -235);
                break;
            case 3:
                textMeshPro.color = Color.white;
                StartCoroutine(TrunYellow());
                rectTransform.anchoredPosition = new Vector2 (235, -221);
                break;
            case 4: //첫 티켓팅
                tourYellow.SetActive(false);
                textMeshPro.color = Color.black;
                rectTransform.anchoredPosition = new Vector2 (-10, 267);
                StartCoroutine(Talk("할수있다!\n할수있다!", 1f, new Vector2(528, -65)));
                break;
            case 5:
                targetTxt2.text = null;
                rectTransform.anchoredPosition = new Vector2 (597, 60);
                break;
            case 6:
                textMeshPro.fontSize = 15;
                rectTransform.anchoredPosition = new Vector2 (215, -155);
                break;
            case 7: //취켓팅
                nextSceneSpeed = 2.5f;
                textMeshPro.fontSize = 6;
                rectTransform.anchoredPosition = new Vector2 (-20, -265);
                StartCoroutine(Talk("이대로 포기할 순 없지\n내가 꼭 가고만다", 2f, new Vector2(528, -50)));
                break;
            case 8: //예매 성공
                nextSceneSpeed = 1.5f;
                targetTxt2.text = null;
                rectTransform.anchoredPosition = new Vector2 (-20, 270);
                break;
            case 9: //공연장면
                textMeshPro.color = Color.white;
                StartCoroutine(ChangeLight());
                audioSource.Stop();
                audioSource.clip = Sound2;
                audioSource.Play();
                rectTransform.anchoredPosition = new Vector2 (-10, 267);
                break;
            case 11: //thanks to 자르반
                audioSource.Stop();
                audioSource.clip = Sound3;
                audioSource.Play();
                rectTransform.anchoredPosition = new Vector2 (-10, 255);
                textMeshPro.fontStyle = FontStyles.Bold;
                textMeshPro.fontStyle = FontStyles.Italic;
                textMeshPro.fontSize = 10;
                targetTxt.text = null;
                break;
            case 12: //효자밴드
                textMeshPro.fontStyle = FontStyles.Normal;
                textMeshPro.fontSize = 6;
                rectTransform.anchoredPosition = new Vector2 (-10, -235);
                break;
        }
    }

    IEnumerator TrunYellow()
    {

        for (int i = 0; i < 5; i++)
        {
            tourYellow.SetActive(true);
            yield return new WaitForSeconds(0.55f);
            tourYellow.SetActive(false);
            if (i != 4)
                yield return new WaitForSeconds(0.55f);           
        }
    }

    IEnumerator ChangeLight()
    {
        //6번 반복
        for (int i = 0; i < 6; i++)
        {
            int randNum = Random.Range(0, 5);
            yield return new WaitForSeconds(0.7f);
            Light[randNum].SetActive(true);
            audience.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            Light[randNum].SetActive(false);
            audience.SetActive(false);
        }
    }

    IEnumerator Talk(string txt, float waitTime, Vector2 position)
    {   
        rectTransform2.anchoredPosition = position;
        yield return new WaitForSeconds(waitTime);
        targetTxt2.text = null;
        for (int i = 0; i < txt.Length; i++)
        {
            targetTxt2.text += txt[i];
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(nextSceneSpeed);
    }
}





