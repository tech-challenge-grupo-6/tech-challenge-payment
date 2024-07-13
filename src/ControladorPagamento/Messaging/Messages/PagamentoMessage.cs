

using Newtonsoft.Json;

namespace ControladorPagamento.Messaging.Messages;

public class PagamentoMessage
{
    [JsonProperty("orderId")]
    public required string OrderId { get; set; }
    
    [JsonProperty("status")]
    public bool Status {  get; set; }   

}

