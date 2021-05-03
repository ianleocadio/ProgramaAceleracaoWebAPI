# Programa de Aceleração - Web API

## ASP.NET 5 & EntityFramework Core

## Pré-requisitos
* Visual Studio 2019

## Provedores
* InMemory
* SQLServer
## Startup

* Abra a solution no Visual Studio 2019;
* Selecione o projeto `WebAPI` como Startup Project;
* Execute a aplicação;
* Abra no navegador http://localhost:8080

## Swagger Documentation
Para facilitar o uso da API foi disponibilizado a interface fornecida pelo Swagger.
para acessar basta abrir o endereço base da aplicação http://localhost:8080

___
## Executando para InMemory
Não forneça ConnectionStrings no `appsettings.json`. Por padrão a aplicação irá iniciar com o banco em memória.
## Executando para SQLServer
### Pré-requisitos
* Instância de SQLServer configurada

### Configurando para SQLServer

Adicione no `appsettings.json` a connectionString:
```json
{
  "ConnectionStrings": {
    "SQLServer": "Server=localhost\\SQLEXPRESS;Database=PAWebApiDatabase;Trusted_Connection=True;"
  }
}
```
Ou caso precise informar usuário e senha:
```json
{
  "ConnectionStrings": {
    "SQLServer": "Server=localhost\\SQLEXPRESS;Database=PAWebApiDatabase;User Id=admin;Password=admin;",
  }
}
```

Agora basta executar os passos para iniciar a aplicação.