using Application.Models;
using Application.ModelsDTO;
using Application.Orchestrations.Interfaces;
using Application.Services.Interfaces;
using ChatApi.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatApi.Hubs;

public class ChatsHub : Hub, IChatsHub
{

    private IGenericOrchestrator<IChatsAccessService> GenericOrchestrator { get; init; }


    public ChatsHub(IGenericOrchestrator<IChatsAccessService> genericOrchestrator)
    {
        GenericOrchestrator = genericOrchestrator;
    }

    public override async Task OnConnectedAsync()
    {
        if (Context.ConnectionId is null)
        {
            new Exception("Context is null");
        }

        if (Guid.TryParse(Context?.User?.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, out Guid userId) == false)
        {
            throw new Exception("User id is null or incorrect");
        }


        HashSet<GroupIdentifier> groupsContainsTheUserIdentifiers =
            [.. (await GenericOrchestrator.ExecuteAsync(
                    service => service.ChatsContainsTheUserAsync(userId))
                ).Select(g => new GroupIdentifier(g.Title, g.Id))
            ];
        foreach (var groupIdentifier in groupsContainsTheUserIdentifiers)
        {
            await Groups.AddToGroupAsync(Context!.ConnectionId!, groupIdentifier.ToString());
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Guid.TryParse(Context?.User?.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, out Guid userId) == false)
        {
            new Exception("User id is null or incorrect");
        }
        HashSet<GroupIdentifier> groupsContainsTheUserIdentifiers =
            [.. (await GenericOrchestrator.ExecuteAsync(
                    service => service.ChatsContainsTheUserAsync(userId))
                ).Select(g => new GroupIdentifier(g.Title, g.Id))
            ];

        foreach (var groupIdentifier in groupsContainsTheUserIdentifiers)
        {
            await Groups.RemoveFromGroupAsync(Context?.ConnectionId ?? throw new Exception(), groupIdentifier.ToString());
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Receiving <see cref="MessageResponseDTO"/> there and send it back for notify about update a message
    /// <b><br/> Convinient call before the response in the controllers</b>
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task UpdateMessageAsync(MessageResponseDTO message)
    {
        GroupIdentifier groupIdentifier = await GenericOrchestrator.ExecuteAsync(
            service => service.GetGroupIdentifierByGroupIdAsync(message.ChatId)
        );
        await Clients.Group(groupIdentifier.ToString()).SendAsync("updateMessage", message);
    }

    /// <summary>
    /// Receiving <see cref="MessageResponseDTO"/> there and send it back for notify about creating a new message
    /// <b><br/> Convinient call before the response in the controllers</b>    
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task SendMessageAsync(MessageResponseDTO message)
    {
        GroupIdentifier groupIdentifier = await GenericOrchestrator.ExecuteAsync(
            service => service.GetGroupIdentifierByGroupIdAsync(message.ChatId)
        );
        await Clients.Group(groupIdentifier.ToString()).SendAsync("receiveMessage", message);
    }

    /// <summary>
    /// Receiving <see cref="MessageResponseDTO"/> there and send it back for notify about deleting a message
    /// <b><br/> Convinient call before the response in the controllers</b>
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task DeleteMessageAsync(MessageResponseDTO message)
    {
        GroupIdentifier groupIdentifier = await GenericOrchestrator.ExecuteAsync(
            service => service.GetGroupIdentifierByGroupIdAsync(message.ChatId)
        );
        await Clients.Group(groupIdentifier.ToString()).SendAsync("deleteMessage", message);
    }
}
