﻿using ControladorPagamento.Entities.Shared;

namespace ControladorPagamento.Entities;

public class Usuario : EntityBase
{
    public string Nome { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string Email { get; set; } = null!;
}
