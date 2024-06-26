﻿using ControladorPagamento.Entities.Shared;

namespace ControladorPagamento.Entities;

public class Cliente : EntityBase
{
    public string Nome { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string Email { get; set; } = null!;
    public virtual ICollection<Pedido> Pedidos { get; set; } = null!;
}
