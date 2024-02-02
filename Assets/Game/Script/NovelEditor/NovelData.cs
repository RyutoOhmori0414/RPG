using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NovelData
{
    [SerializeField]
    private Sprite _actorSprite;
    [SerializeField]
    private string _actorName;
    [SerializeField]
    private string _text;
    [SerializeField]
    private AudioClip _voice;
    
    public Sprite ActorSprite => _actorSprite;
    public string ActorName => _actorName;
    public string Text => _text;
    public AudioClip Voice => _voice;
}
