using System.Data.SqlTypes;

public class BucketItem
{
    public string nome {get; private set;}
    public string descricao {get; private set;}

    public BucketItem(string nome, string descricao = "")
    {
        this.nome = nome;
        this.descricao = descricao;
    }

    public void atualizarDados(string novoNome = null, string novaDescricao = null)
    {
        if(novoNome != null && novoNome.Trim() != string.Empty)
        {
            this.nome = novoNome;
        }

        if(novaDescricao != null)
        {
            this.descricao = novaDescricao;
        }
    }
}