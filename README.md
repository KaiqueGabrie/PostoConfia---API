Guia de Execução e Configuração
Para que qualquer pessoa execute este projeto, é necessário realizar uma série de passos práticos de configuração do ambiente:

1. Configuração do Banco de Dados
O projeto requer um servidor MySQL acessível. O primeiro passo é criar um banco de dados vazio (ex: postoconfia_db). Em seguida, o arquivo appsettings.json deve ser editado para atualizar a ConnectionStrings com os parâmetros corretos do servidor (endereço, porta, nome do banco, usuário e senha). Após configurar a string de conexão, a estrutura das tabelas é criada utilizando o Entity Framework Core, geralmente executando o comando Update-Database no Console do Gerenciador de Pacotes ou dotnet ef database update na linha de comando.

2. Compilação e Início
Com o Visual Studio ou o .NET SDK instalado, o projeto deve ter suas dependências restauradas (NuGet packages) e ser compilado. Ao iniciar o projeto (via F5 ou dotnet run), a aplicação ASP.NET Core inicia o servidor local. 

3. Teste e Uso através do Swagger UI
O projeto é iniciado abrindo automaticamente a interface de documentação Swagger UI no navegador. Esta interface é a principal ferramenta para testar o sistema.

O fluxo de teste recomendado é sequencial:

Cadastro de Dados Base: Usar o CombustivelController (POST /Criar-Combustivel) para registrar os tipos de combustível e o UsuarioController (POST /Criar-Usuario) para cadastrar um usuário de teste, anotando seus respectivos IDs.

Criação do Posto: Usar o PostoDeCombustivelController (POST /Criar-Posto) para adicionar um novo posto ao sistema.

Registro de Interações:

Registrar preços utilizando o PrecoController (POST /Postos/{postoId}/Precos/Registrar), vinculando o ID do posto e o ID do combustível.

Registrar notas utilizando o PostoDeCombustivelController (POST /Postos/{id}/Avaliar), usando o ID do usuário e a nota.

Consulta: Finalmente, usar o GET /Postos/{id} para verificar se o posto está retornando corretamente a AvaliacaoMedia recalculada e o histórico de preços e comentários, comprovando o funcionamento integrado de todo o sistema.
