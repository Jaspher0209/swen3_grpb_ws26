using Models;

namespace PaperlessREST.Api.Dtos;

public class MessageDto
{
    public string Message { get; set; }
}

public static class MessageDtoExtensions
{
    public static MessageDto ToDto(this Message message)
    {
        return new MessageDto
        {
            Message = message.message
        };
    }
    
    public static Message ToModel(this MessageDto messageDto)
    {
        return new Message
        {
            message = messageDto.Message
        };
    }
}