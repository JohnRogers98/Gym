using Gym.Chat.Domain._Common;
using Gym.Domain.ChatContext.Entities;
using Gym.Domain.ChatContext.Events;
using Gym.Domain.ChatContext.ValueObjects;

namespace Gym.Domain.ChatContext;

public class DirectChat : AggregateRoot
{
    public ChatId Id { get; }

    public IReadOnlySet<Participant> Participants { get; }

    public MessageSequence LastSequence { get; private set; }

    private DirectChat(ChatId id, IReadOnlySet<Participant> participants, MessageSequence lastSequence)
    {
        Id = id;
        Participants = participants;
        LastSequence = lastSequence;
    }

    public static DirectChat Create(ChatId id, IReadOnlySet<Participant> participants)
    {
        DirectChat directChat = new(id, participants, MessageSequence.Zero);
        directChat.AddDomainEvent(DirectChatCreatedDomainEvent.Create(directChat.Id));
        return directChat;
    }
        

    public static DirectChat Restore(ChatId id, IReadOnlySet<Participant> participants, MessageSequence lastSequence) =>
        new(id, participants, lastSequence);
}