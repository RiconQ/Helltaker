using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("ĳ���� �̸�")]
    public string name;

    [Tooltip("��� ����")]
    public string[] contexts;

    [Tooltip("�̺�Ʈ ��ȣ")]
    public string[] eventNum;

    [Tooltip("��ŵ ����")]
    public string[] skipLine;

    [Tooltip("��� ȭ��")]
    public string[] showDeath;

    [Tooltip("�������� Ŭ���� ����")]
    public string[] clearStage;

    [Tooltip("�ʻ�ȭ")]
    public string[] portrait;

    //[Tooltip("�ִϸ��̼�")]
    //public string[] animation;
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

    // ������ ��ȭ
    public string[] select;

    // ������ �̵� ����
    public string[] lineToMove;
}

[System.Serializable]
public class SelectClass
{
    //public string name;

    public Vector2Int line;
    public EventSelect[] selects;
}