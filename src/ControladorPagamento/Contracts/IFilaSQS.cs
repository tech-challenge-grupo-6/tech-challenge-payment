using Amazon.SQS;
using Amazon.SQS.Model;

namespace ControladorPagamento.Contracts;

public interface IFilaSQS
{
    Task<SendMessageResponse> EnviarMensagem(IAmazonSQS client, string queueUrl, string messageBody, Dictionary<string, MessageAttributeValue> messageAttributes);
    Task<ReceiveMessageResponse> ReceberEDeletarMensagem(IAmazonSQS client, string queueUrl);
    Task<SendMessageResponse> EnviarMensagemSimples(IAmazonSQS client, string queueUrl, string messageBody);
}
