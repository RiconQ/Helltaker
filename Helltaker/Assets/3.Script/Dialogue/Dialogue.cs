using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("캐릭터 이름")]
    public string name;

    [Tooltip("대사 내용")]
    public string[] contexts;

    [Tooltip("이벤트 번호")]
    public string[] eventNum;

    [Tooltip("스킵 라인")]
    public string[] skipLine;

    [Tooltip("사망 화면")]
    public string[] showDeath;

    [Tooltip("스테이지 클리어 여부")]
    public string[] clearStage;

    [Tooltip("초상화")]
    public string[] portrait;

}

[System.Serializable]
public class DialogueEvent
{
    public string name;

    public Vector2Int line;
    public Dialogue[] dialogues;
}

[System.Serializable]
public class EventSelect
{
    // ID
    public string ID;

    // 선택지 대화
    public string[] select;

    // 선택지 이동 라인
    public string[] lineToMove;
}

[System.Serializable]
public class SelectClass
{
    //public string name;

    public Vector2Int line;
    public EventSelect[] selects;
}