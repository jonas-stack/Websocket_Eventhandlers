using System.Security.Authentication;
using Fleck;
using WebSocketBoilerplate;

namespace WebSockets_EventHandlers.EventHandlers;

public class ClientWantsToSignInDto : BaseDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class ServerAuthenticatesClient : BaseDto
{
    public string Jwt { get; set; }
}


public class ClientWantsToSignInEventHandler(SecurityService securityService) : BaseEventHandler<ClientWantsToSignInDto>
{
    public async override Task Handle(ClientWantsToSignInDto dto, IWebSocketConnection socket)
    {
        const string username = "Bob";
        const string password = "S3cret!";
        const string salt = "imagineasuperlongsalt";
        const string hashedPasswordWithSalt = "M6DAxHjzYVmKgk334dpsNG19X/M269DXr5kxC/EVsd6v9NlIEw7G/qRA32UL4xmdWU6lKSGV1+nL8q+a35ZdWw==";

        if(dto.Username.ToLower() != username.ToLower())
            throw new AuthenticationException("you shall not pass");
        
        if (securityService.Hash(dto.Password + salt) != hashedPasswordWithSalt)
            throw new AuthenticationException("you shall not pass");

        socket.SendDto(new ServerAuthenticatesClient()
        {
            Jwt = securityService.GenerateJwt(dto.Username) //all is good so you get a jwt
        });
    }
}