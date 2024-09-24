using Godot;
using System;

public partial class ItemDetailsScreen : Control
{
    private Message_TextBar titleTextBar;
    private Message_TextBar bottomTextBar;

    public override void _Ready()
    {
        titleTextBar = (Message_TextBar)FindChild("Title");
        bottomTextBar = (Message_TextBar)FindChild("Message_TextBar");
    }
    public void inicializarTelaDetalhes(BucketItem item)
    {
        titleTextBar.ExibirMensagem(item.nome, Colors.White);
        //inicializar descrição
    }
}
