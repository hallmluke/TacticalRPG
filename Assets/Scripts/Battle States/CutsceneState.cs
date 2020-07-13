using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CutsceneState : BattleState
{
    ConversationController conversationController;
    ConversationData data;
    protected override void Awake()
    {
        base.Awake();
        conversationController = owner.GetComponentInChildren<ConversationController>();
        SpeakerData example = new SpeakerData();

        example.messages = new List<string>();
        example.messages.Add("Write some code AJ");
        data = ScriptableObject.CreateInstance<ConversationData>();
        data.list = new List<SpeakerData>();
        data.list.Add(example);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (data)
            Resources.UnloadAsset(data);
    }
    public override void Enter()
    {
        base.Enter();
        conversationController.Show(data);
    }
    protected override void AddListeners()
    {
        base.AddListeners();
        ConversationController.completeEvent += OnCompleteConversation;
    }
    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        ConversationController.completeEvent -= OnCompleteConversation;
    }
    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        base.OnFire(sender, e);
        conversationController.Next();
    }
    void OnCompleteConversation(object sender, System.EventArgs e)
    {
        owner.ChangeState<SelectUnitState>();
    }
}